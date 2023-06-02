using Microsoft.AspNetCore.Identity;

namespace Domain;

public class ApplicationUser : IdentityUser
{
    public IEnumerable<Budget>? Budgets { get; set; }
}