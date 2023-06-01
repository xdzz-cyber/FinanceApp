using Microsoft.AspNetCore.Identity;

namespace Domain;

public class FinancialGoal
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Description { get; set; } = null!;
    
    public decimal TargetAmount { get; set; }
    
    public DateTime TargetDate { get; set; }
    public Budget Budget { get; set; } = null!;
    public Guid BudgetId { get; set; }
}