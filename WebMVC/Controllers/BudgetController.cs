using System.Security.Claims;
using Application.Budget.Commands.AddBudget;
using Application.Budget.Queries.GetBudget;
using Application.Budget.Queries.GetBudgets;
using Application.Category.Queries.GetCategories;
using Application.Common.Dtos;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Controllers;

[Authorize]
public class BudgetController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Budgets()
    {
        var budgets = await Mediator.Send(new GetBudgets {UserId = User.FindFirstValue(ClaimTypes.NameIdentifier)});
        
        return View(budgets);
    }
    
    // View specific budget

    [HttpGet]
    public async Task<IActionResult> Budget(Guid id)
    {
        // Add logic to get budget by id
        var budget = await Mediator.Send(new GetBudget {Id = id});
        //var categoryName = await GetCategoryName(budget.CategoryId);
        var transactions = GetTransactions(budget);
        
        return View(new BudgetVm
        {
            Budget = budget,
            Transactions = transactions,
            TransactionsJson = Newtonsoft.Json.JsonConvert.SerializeObject(transactions)
        });
    }

    [HttpGet]
    public IActionResult AddBudget()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> AddBudget(AddBudgetVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }
        
        var budget = new Budget
        {
            Name = vm.Name,
            Amount = vm.Amount,
            StartDate = vm.StartDate,
            EndDate = vm.EndDate,
            AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        };
        
        var result = await Mediator.Send(new AddBudget
        {
            Name = budget.Name,
            Amount = budget.Amount,
            UserId = budget.AppUserId,
            StartDate = budget.StartDate,
            EndDate = budget.EndDate
        });
        
        // Check if result is not Guid.Empty
        if (result == Guid.Empty)
        {
            return View(vm);
        }
        
        return RedirectToAction("Budget", new {id = result});
    }
    
    private async Task<string> GetCategoryName(Guid categoryId)
    {
        var categories = await Mediator.Send(new GetCategories());
        var categoryName = categories.First(c => c.Id == categoryId).Name;
        return categoryName;
    }
    
    private IEnumerable<TransactionVm> GetTransactions(BudgetDto model)
    {
        var transactions = model.Transactions!.Select(t => new
        TransactionVm {
            Date = t.Date,
            Amount = t.Amount,
            CategoryName = GetCategoryName(t.CategoryId).Result,
        });

        return transactions;

        //return Newtonsoft.Json.JsonConvert.SerializeObject(transactions);
    }
}