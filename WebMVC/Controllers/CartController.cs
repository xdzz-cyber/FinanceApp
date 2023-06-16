using System.Security.Claims;
using Application.Budget.Queries.GetBudget;
using Application.Budget.Queries.GetBudgets;
using Application.Category.Queries.GetCategories;
using Application.Coins.Queries.GetCoins;
using Application.Common.Dtos;
using Application.Transaction.Commands.CreateTransaction;
using Domain;
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
        var total = addCoinsVm.Coins.Sum(c => c.Quantity * c.Price);
        
        var categories = await Mediator.Send(new GetCategories());
        
        var expenseCategoryId = categories.First(c => c.Name == "Expense").Id;

        var createTransactionCommand = new CreateTransaction()
        {
            Amount = total,
            Date = DateTime.Now,
            CategoryId = expenseCategoryId,
            AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
            BudgetId = addCoinsVm.budgetId
        };
        
        var createdTransactionId = await Mediator.Send(createTransactionCommand);

        if (createdTransactionId == Guid.Empty)
        {
            return BadRequest();
        }
        var _ = Url.Action("Budget", "Budget", new { id = addCoinsVm.budgetId });
        //return RedirectToAction("Budget", "Budget", new {id = addCoinsVm.budgetId});
        return Json(new { redirectUrl = Url.Action("Budget", "Budget", new { id = addCoinsVm.budgetId }) });
    }
}