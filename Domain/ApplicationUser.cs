using Microsoft.AspNetCore.Identity;

namespace Domain;

public class ApplicationUser : IdentityUser
{
    public IEnumerable<Budget>? Budgets { get; set; }
    // public IEnumerable<FinancialGoal>? FinancialGoals { get; set; }
}