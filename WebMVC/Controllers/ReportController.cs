using Application.Budget.Queries.GetBudgets;
using Application.Category.Queries.GetCategories;
using Application.Common.Dtos;
using Application.FinancialGoal.Queries.GetFinancialGoals;
using Application.Transaction.Queries.GetTransactions;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Controllers;

public class ReportController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> Report()
    {
        var categories = await Mediator.Send(new GetCategories());
        var budgets = await Mediator.Send(new GetBudgets()
        {
            Email = Email
        });

        var data = new ReportVm()
        {
            Budgets = budgets,
            Categories = categories.Select(c => new CategoryDto()
            {
                Id = c.Id,
                Description = c.Description,
                Name = c.Name,
                TransactionsConnected = budgets.SelectMany(b => b.Transactions).Count(t => t.CategoryId == c.Id)
            }).ToList(),
            FinancialGoals = await Mediator.Send(new GetFinancialGoals()
            {
                Email = Email
            })
        };
        
        return View(data);
    }
}