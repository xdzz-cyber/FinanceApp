using System.ComponentModel.DataAnnotations;
using Application.Common.Dtos;

namespace WebMVC.Models;

public class AddFinancialGoalVm
{
    [Required]
    public FinancialGoalDto FinancialGoal { get; set; } = null!;
    public List<BudgetDto>? Budgets { get; set; }
}