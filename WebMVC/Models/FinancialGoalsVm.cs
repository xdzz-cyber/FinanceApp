namespace WebMVC.Models;

public class FinancialGoalsVm
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    public string Type { get; set; } = null!;
    public decimal TargetAmount { get; set; }
    
    public decimal CurrentAmount { get; set; }
    public string BudgetName { get; set; } = null!;
    
    public DateTime TargetDate { get; set; }
}