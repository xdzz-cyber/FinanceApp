using Microsoft.AspNetCore.SignalR;

namespace WebMVC.Hubs;


public class CoinsHub : Hub
{
    private readonly HttpClient _httpClient;

    public CoinsHub(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task UpdateCoinPrices(List<string> coinNames)
    {
        var prices = await GetCoinPrices(coinNames.Select(name => name.ToLower()).ToList());
        await Clients.All.SendAsync("ReceiveCoinPrices", prices);
    }

    private async Task<string> GetCoinPrices(List<string> coinNames)
    {
        var url = $"https://api.coincap.io/v2/assets?ids={string.Join(",", coinNames.Select(name => name.ToLower()))}";
        var response = await _httpClient.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            return content;
        }

        return string.Empty;
    }
}