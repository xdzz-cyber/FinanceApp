using Microsoft.AspNetCore.Identity;

namespace Domain;

public class Transaction
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }
 
    public DateTime Date { get; set; }
    public Guid? CategoryId { get; set; }
    public Category? Category { get; set; } 
 
    public string? AppUserId { get; set; }

    public ApplicationUser? AppUser { get; set; }
    public Budget? Budget { get; set; }
    public Guid? BudgetId { get; set; }
}