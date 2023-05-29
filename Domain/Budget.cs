using Microsoft.AspNetCore.Identity;

namespace Domain;

public class Budget
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public decimal Amount { get; set; }

    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    public string? AppUserId { get; set; }
    public ApplicationUser? AppUser { get; set; }
    public IEnumerable<Transaction>? Transactions { get; set; }
    public IEnumerable<FinancialGoal>? FinancialGoals { get; set; }
}