using System.Security.Claims;
using Application.Budget.Queries.GetBudgets;
using Application.Common.Dtos;
using Application.FinancialGoal.Commands.CreateFinancialGoal;
using Application.FinancialGoal.Queries.GetFinancialGoals;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;

namespace WebMVC.Controllers;

public class FinancialGoalController : BaseController
{
    [HttpGet]
    public async Task<IActionResult> FinancialGoals()
    {
        var financialGoals = await Mediator.Send(new GetFinancialGoals());
        return View(financialGoals);
    }
    
    [HttpGet]
    public async Task<IActionResult> AddFinancialGoal()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return View(new AddFinancialGoalVm()
        {
            FinancialGoal = new FinancialGoalDto(),
            // UserId = userId,
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