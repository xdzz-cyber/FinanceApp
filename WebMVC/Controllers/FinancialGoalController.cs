using System.Security.Claims;
using Application.Budget.Queries.GetBudgets;
using Application.Category.Queries.GetCategories;
using Application.Common.Dtos;
using Application.Common.Exceptions;
using Application.FinancialGoal.Commands.CreateFinancialGoal;
using Application.FinancialGoal.Queries.GetFinancialGoals;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Controllers;

[Authorize]
public class FinancialGoalController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> FinancialGoals()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var financialGoals = await Mediator.Send(new GetFinancialGoals()
        {
            UserId = userId
        });

        var budgets = await Mediator.Send(new GetBudgets { UserId = userId });

        var financialGoalsVm = (
            from financialGoal in financialGoals
            join budget in budgets on financialGoal.BudgetId equals budget.Id
            select new FinancialGoalsVm
            {
                Name = financialGoal.Name,
                Description = financialGoal.Description,
                TargetAmount = financialGoal.TargetAmount,
                CurrentAmount = financialGoal.CurrentAmount,
                Type = financialGoal.CategoryName,
                BudgetName = budget.Name
            }
        ).ToList();

        return View(financialGoalsVm);
    }
    
    [HttpGet]
    public async Task<IActionResult> AddFinancialGoal()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return View(new AddFinancialGoalVm()
        {
            FinancialGoal = new FinancialGoalDto(),
            Budgets = await Mediator.Send(new GetBudgets {UserId = userId}),
            Categories = await Mediator.Send(new GetCategories())
        });
    }
    
    // Create a new financial goal handler.
    [HttpPost]
    public async Task<IActionResult> AddFinancialGoal(AddFinancialGoalFormHandlerVm addFinancialGoalFormHandlerVm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var createdInstanceId = await Mediator.Send(new CreateFinancialGoal()
            {
                Name = addFinancialGoalFormHandlerVm.FinancialGoal.Name,
                Description = addFinancialGoalFormHandlerVm.FinancialGoal.Description,
                TargetAmount = addFinancialGoalFormHandlerVm.FinancialGoal.TargetAmount,
                TargetDate = addFinancialGoalFormHandlerVm.FinancialGoal.TargetDate,
                BudgetId = addFinancialGoalFormHandlerVm.FinancialGoal.BudgetId,
                CategoryId = Guid.Parse(addFinancialGoalFormHandlerVm.FinancialGoal.CategoryName),
                CurrentAmount = addFinancialGoalFormHandlerVm.FinancialGoal.CurrentAmount
            });

            if (createdInstanceId == Guid.Empty)
            {
                return BadRequest("Failed to create financial goal.");
            }

            return RedirectToAction("FinancialGoals");
        }
        catch (AlreadyExistsException alreadyExistsException)
        {
            return RedirectToAction("Error", "Error", new {message = alreadyExistsException.Message});
        }
    }
}