using System.Text.Json;
using Application.Card.Commands.UpdateCard;
using Application.Card.Queries.GetCards;
using Application.Common.Constants;
using Application.Common.Dtos;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using WebMVC.Hubs;

namespace WebMVC.BackgroundJobs;

public class HangfireRemoteApiCallJob
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _bankingApiUrl;
    private readonly IHubContext<BankingHub> _hubContext;
    private readonly IMediator _mediator;

    public HangfireRemoteApiCallJob(HttpClient httpClient, IConfiguration configuration, 
        IHubContext<BankingHub> hubContext, IMediator mediator)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _bankingApiUrl = "https://api.monobank.ua/personal/client-info";
        _hubContext = hubContext;
        _mediator = mediator;
    }

    [AutomaticRetry(Attempts = 3)] // Optional: Configures automatic retries
    public async Task<string> MakeRemoteApiCall()
    {
        // Make request to banking api and before it add header with token
        _httpClient.DefaultRequestHeaders.Add("X-Token", _configuration["MonobankToken"]);
        var response = await _httpClient.GetAsync(_bankingApiUrl);
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            
            // First of all update redis cards info
            var client = JsonSerializer.Deserialize<ClientDto>(content);
            
            var cardIndex = 0;
            
            var cachedCards = await _mediator.Send(new GetCards());

            foreach (var card in client!.Accounts)
            {
                var matchedCard = cachedCards.FirstOrDefault(cachedCard => cachedCard.Id == card.Id);

                var updateAmount = matchedCard!.UpdateAmount;

                var stripeId = matchedCard.StripeId;
                
                if (matchedCard.InitialAmount != card.Balance)
                {
                    var updateCard = new UpdateCard
                    {
                        Id = card.Id,
                        InitialAmount = card.Balance
                    };
                    
                    if (matchedCard.StripeId != card.StripeId)
                    {
                        updateCard.StripeId = CardsIdsConstants.CardsId[cardIndex];
                    }
                    
                    var updatedCard = await _mediator.Send(updateCard);
                    
                    updateAmount = updatedCard!.UpdateAmount;
                    stripeId = updatedCard.StripeId;
                }
                    
                card.Balance = (card.Balance / 4000) + updateAmount / 100; // To convert to dollars
                card.StripeId = stripeId;
            }
            
            foreach (var jar in client.Jars)
            {
                var matchedJar = cachedCards.FirstOrDefault(cachedCard => cachedCard.Id == jar.Id);

                var updateAmount = matchedJar!.UpdateAmount;
                
                var stripeId = matchedJar.StripeId;
                
                if (matchedJar.InitialAmount != jar.Balance)
                {
                    var updateJar = new UpdateCard
                    {
                        Id = jar.Id,
                        InitialAmount = jar.Balance
                    };
                    
                    if (matchedJar.StripeId != jar.StripeId)
                    {
                        matchedJar.StripeId = CardsIdsConstants.CardsId[cardIndex];
                    }
                    
                    var updatedJar = await _mediator.Send(updateJar);
                    
                    updateAmount = updatedJar!.UpdateAmount;
                    
                    stripeId = updatedJar.StripeId;
                }
                    
                jar.Balance = (jar.Balance / 4000) + updateAmount / 100; // To convert to dollars
                jar.StripeId = stripeId;
            }

            content = JsonSerializer.Serialize(client);

            await _hubContext.Clients.All.SendAsync("ReceiveBankingInfo", content);

            return content;
        }
        
        return string.Empty;
    }
}