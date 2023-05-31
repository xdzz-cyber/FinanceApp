using System.Security.Claims;
using Application.Category.Queries.GetCategories;
using Application.Transaction.Commands.CreateTransaction;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebMVC.Models;

namespace WebMVC.Controllers;

[Authorize]
public class TransactionController : BaseController
{
    [HttpGet]
    public async Task<ActionResult> AddTransaction(Guid budgetId)
    {
        var categories = await Mediator.Send(new GetCategories());
        return View(new AddTransactionVm()
        {
            BudgetId = budgetId,
            Categories = categories
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> AddTransaction(AddTransactionVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var result = await Mediator.Send(new CreateTransaction()
        {
            Amount = vm.Amount,
            Date = vm.Date,
            CategoryId = vm.CategoryId,
            BudgetId = vm.BudgetId,
            AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
        });
        
        // Check if the transaction was created successfully
        if (result == Guid.Empty)
        {
            return View(vm);
        }
        
        return RedirectToAction("Budget", "Budget", new {id = vm.BudgetId});
    }
}