using System.Text.Json;
using Application.Common.Dtos;
using Microsoft.AspNetCore.SignalR;

namespace WebMVC.Hubs;

public class BankingHub : Hub
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public BankingHub(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }
    
    // private async Task UpdateBankingInfo(string items)
    // {
    //    // var items = await GetItems();
    //     await Clients.All.SendAsync("ReceiveBankingInfo", items);
    // }
    
    // Implement notify method to get data from hangfire job
    public async Task Notify(string message)
    {
        await Clients.All.SendAsync("ReceiveBankingInfo", message);
    }

    // private async Task<string> GetItems() // Cards and banks
    // {
    //     // Make request to banking api and before it add header with token
    //     _httpClient.DefaultRequestHeaders.Add("X-Token", _configuration["MonobankToken"]);
    //     var response = await _httpClient.GetAsync(_bankingApiUrl);
    //     
    //     if (response.IsSuccessStatusCode)
    //     {
    //         var content = await response.Content.ReadAsStringAsync();
    //         
    //         //return JsonSerializer.Deserialize<ClientDto>(content);
    //         return content;
    //     }
    //     
    //     return string.Empty;
    // }
}