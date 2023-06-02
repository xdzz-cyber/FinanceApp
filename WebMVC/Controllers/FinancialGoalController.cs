using System.Security.Claims;
using Application.Budget.Queries.GetBudgets;
using Application.Common.Dtos;
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
                TargetDate = financialGoal.TargetDate,
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
            Budgets = await Mediator.Send(new GetBudgets {UserId = userId})
        });
    }
    
    // Create a new financial goal handler.
    [HttpPost]
    public async Task<IActionResult> AddFinancialGoal(AddFinancialGoalVm addFinancialGoalVm)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var createdInstanceId = await Mediator.Send(new CreateFinancialGoal()
        {
            Name = addFinancialGoalVm.FinancialGoal.Name,
            Description = addFinancialGoalVm.FinancialGoal.Description,
            TargetAmount = addFinancialGoalVm.FinancialGoal.TargetAmount,
            TargetDate = addFinancialGoalVm.FinancialGoal.TargetDate,
            BudgetId = addFinancialGoalVm.FinancialGoal.BudgetId
        });
        
        // Check if the instance was created successfully.
        if (createdInstanceId == Guid.Empty)
        {
            return BadRequest();
        }
        
        return RedirectToAction("FinancialGoals");
    }
}