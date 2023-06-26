using System.Text.Json;
using Application.Card.Commands.UpdateCard;
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

            foreach (var card in client!.Accounts)
            {
                var updatedCard = await _mediator.Send(new UpdateCard
                {
                    Id = card.Id,
                    StripeId = CardsIdsConstants.CardsId[cardIndex++],
                    InitialAmount = card.Balance
                });// 186
                card.Balance = (card.Balance / 4000) + updatedCard!.UpdateAmount / 100; // To convert to dollars
                card.StripeId = updatedCard.StripeId;
            }
            
            foreach (var jar in client.Jars)
            {
                var updatedJar = await _mediator.Send(new UpdateCard()
                {
                    Id = jar.Id,
                    StripeId = CardsIdsConstants.CardsId[cardIndex++],
                    InitialAmount = jar.Balance 
                });
                jar.Balance = (jar.Balance / 4000) + updatedJar!.UpdateAmount / 100; // To convert to dollars
                jar.StripeId = updatedJar.StripeId;
            }
            
            // Create new content with replaced cards and jars
            //content = JsonSerializer.Serialize(client);
            
            // Serialize in different way
            
            content = JsonSerializer.Serialize(client);

            await _hubContext.Clients.All.SendAsync("ReceiveBankingInfo", content);

            return content;
        }
        
        return string.Empty;
    }
}