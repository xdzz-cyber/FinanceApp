using System.Diagnostics;
using System.Text.Json;
using Application.Coins.Queries.GetCoins;
using Application.Common.Constants;
using Application.Common.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebMVC.Models;

namespace WebMVC.Controllers;

[Authorize]
public class HomeController : BaseController
{
    
    private readonly IMemoryCache _cache;
    private const string CoinsCacheKey = "CoinsData";
    public HomeController(IMemoryCache cache)
    {
        _cache = cache;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        // Check if coins data is available in the cache
        if (!_cache.TryGetValue(CoinsCacheKey, out List<CoinDto>? coins))
        {
            // Coins data not found in the cache, retrieve it and store in the cache
            coins = await Mediator.Send(new GetCoins());
            _cache.Set(CoinsCacheKey, coins, TimeSpan.FromDays(7)); // Adjust the cache duration as per your requirement
        }

        var paginatedCoins = coins
            .Skip((page - 1) * CoinsConstants.MaxCoinsPerPage)
            .Take(CoinsConstants.MaxCoinsPerPage)
            .ToList();
        
        var totalPages = (int)Math.Ceiling((double) (coins.Count / CoinsConstants.MaxCoinsPerPage));

        return View(new CoinsVm()
        {
            Coins = paginatedCoins,
            TotalPages = totalPages,
            StartPage = Math.Max(page - 2, 1),
            EndPage = Math.Min(page + 2, totalPages),
            CurrentPage = page
        });
    }
    

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}