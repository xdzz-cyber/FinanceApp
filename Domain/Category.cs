namespace Domain;

public class Category
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
    public IEnumerable<Transaction>? Transactions { get; set; }
    public IEnumerable<FinancialGoal>? FinancialGoals { get; set; }
}