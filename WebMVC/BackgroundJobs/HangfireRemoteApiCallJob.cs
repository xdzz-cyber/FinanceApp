using Hangfire;
using Microsoft.AspNetCore.SignalR;
using WebMVC.Hubs;

namespace WebMVC.BackgroundJobs;

public class HangfireRemoteApiCallJob
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly string _bankingApiUrl;
    private readonly IHubContext<BankingHub> _hubContext;

    public HangfireRemoteApiCallJob(HttpClient httpClient, IConfiguration configuration, IHubContext<BankingHub> hubContext)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _bankingApiUrl = "https://api.monobank.ua/personal/client-info";
        _hubContext = hubContext;
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
            
            //return JsonSerializer.Deserialize<ClientDto>(content);
            await _hubContext.Clients.All.SendAsync("ReceiveBankingInfo", content);

            return content;
        }
        
        return string.Empty;
    }
}