using System.Text.Json;
using Application.Card.Commands.UpdateCard;
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
            
            foreach (var card in client!.Accounts)
            {
                await _mediator.Send(new UpdateCard
                {
                    Id = card.Id,
                    InitialAmount = card.Balance
                });
            }

            foreach (var jar in client.Jars)
            {
                await _mediator.Send(new UpdateCard()
                {
                    Id = jar.Id,
                    UpdateAmount = jar.Balance 
                });
            }
            
            // Create new content with replaced cards and jars
            content = JsonSerializer.Serialize(client);
            
            await _hubContext.Clients.All.SendAsync("ReceiveBankingInfo", content);

            return content;
        }
        
        return string.Empty;
    }
}