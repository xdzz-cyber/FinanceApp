using System.Diagnostics;
using System.Text.Json;
using Application.Coins.Queries.GetCoins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Controllers;

[Authorize]
public class HomeController : BaseController
{

    public HomeController()
    {
     
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Make get request to the API to get all coins.
        // var httpClient = new HttpClient();
        // var response = httpClient.GetAsync("https://api.coincap.io/v2/assets").Result;
        // var coins = response.Content.ReadAsStringAsync().Result;
        // var parsedCoins = JsonSerializer.Deserialize<CoinListVm>(coins);
        var coins = await Mediator.Send(new GetCoins());
        return View(coins);
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}