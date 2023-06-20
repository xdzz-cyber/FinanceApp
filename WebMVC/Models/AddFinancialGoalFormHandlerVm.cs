using System.ComponentModel.DataAnnotations;
using Application.Common.Dtos;

namespace WebMVC.Models;

public class AddFinancialGoalFormHandlerVm
{
    [Required]
    public FinancialGoalDto FinancialGoal { get; set; } = null!;
}