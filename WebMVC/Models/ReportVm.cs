using Application.Common.Dtos;

namespace WebMVC.Models;

public class ReportVm
{
    // Budgets dto.
    public IEnumerable<BudgetDto> Budgets { get; set; } = null!;
    
    // Categories dto.
    public IEnumerable<CategoryDto> Categories { get; set; } = null!;
    
    // FinancialGoals dto.
    public IEnumerable<FinancialGoalDto> FinancialGoals { get; set; } = null!;
    
}