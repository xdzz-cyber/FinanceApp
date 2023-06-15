using System.Security.Claims;
using Application.Budget.Queries.GetBudgets;
using Application.Coins.Queries.GetCoins;
using Application.Common.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebMVC.Models;

namespace WebMVC.Controllers;

public class CartController : BaseController
{
    private readonly IMemoryCache _cache;
    private const string CoinsCacheKey = "CoinsData";

    public CartController(IMemoryCache cache)
    {
        _cache = cache;
    }

    [HttpGet]
    public async Task<IActionResult> Cart([FromQuery] string coinsIds)
    {
        // Get coins from the database
        // Check if coins data is available in the cache
        if (!_cache.TryGetValue(CoinsCacheKey, out List<CoinDto>? coins))
        {
            // Coins data not found in the cache, retrieve it and store in the cache
            coins = await Mediator.Send(new GetCoins());
            _cache.Set(CoinsCacheKey, coins, TimeSpan.FromDays(7)); // Adjust the cache duration as per your requirement
        }
        
        var budgetDtos = await Mediator.Send(new GetBudgets()
        {
            UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        });
        
        return View(new CartPageVm()
        {
            Coins = coins!.Where(c => coinsIds.Contains(c.Id.ToString())).ToList(),
            Budgets = budgetDtos
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> AddToCart([FromBody] AddCoinsVm addCoinsVm)
    {
        var _ = "Add to cart";
        return Content(_);
    }
}