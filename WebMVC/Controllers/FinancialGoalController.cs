using Application.Common.Dtos;
using Application.FinancialGoal.Commands.CreateFinancialGoal;
using Application.FinancialGoal.Queries.GetFinancialGoals;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult AddFinancialGoal()
    {
        return View();
    }
    
    // Create a new financial goal handler.
    [HttpPost]
    public async Task<IActionResult> AddFinancialGoal(FinancialGoalDto financialGoalDto)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var createdInstanceId = await Mediator.Send(new CreateFinancialGoal()
        {
            Name = financialGoalDto.Name,
            Description = financialGoalDto.Description,
            TargetAmount = financialGoalDto.TargetAmount,
            TargetDate = financialGoalDto.TargetDate,
            BudgetId = financialGoalDto.BudgetId
        });
        
        // Check if the instance was created successfully.
        if (createdInstanceId == Guid.Empty)
        {
            return BadRequest();
        }
        
        return RedirectToAction("FinancialGoals");
    }
}