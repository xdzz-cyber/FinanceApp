using System.Security.Claims;
using Application.ApplicationUser.Queries.GetUser;
using Application.Budget.Queries.GetBudget;
using Application.Category.Queries.GetCategories;
using Application.Transaction.Commands.CreateTransaction;
using Application.Transaction.Queries.GetTransactions;
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
        var transactions = await Mediator.Send(new GetTransactions() {BudgetId = budgetId});
        var budget = await Mediator.Send(new GetBudget() {Id = budgetId});
        return View(new AddTransactionVm()
        {
            BudgetId = budgetId,
            Categories = categories,
            NetAmount = budget.Amount + transactions.Where(t => t.CategoryId == categories.First(c => c.Name == "Income").Id)
                            .Sum(t => t.Amount) - transactions.Where(t => t.CategoryId == categories.First(c => c.Name == "Expense").Id)
                .Sum(t => t.Amount)
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> AddTransaction(AddTransactionVm vm)
    {
        if (!ModelState.IsValid)
        {
            return View(vm);
        }

        var user = await Mediator.Send(new GetUser() {Email = Email});

        var result = await Mediator.Send(new CreateTransaction()
        {
            Amount = vm.Amount,
            Date = vm.Date,
            CategoryId = vm.CategoryId,
            BudgetId = vm.BudgetId,
            AppUserId = user.Id
        });
        
        // Check if the transaction was created successfully
        if (result == Guid.Empty)
        {
            return View(vm);
        }
        
        return RedirectToAction("Budget", "Budget", new {id = vm.BudgetId});
    }
}