using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Domain.Budget> Budgets { get; set; }
    
    DbSet<Domain.Transaction> Transactions { get; set; }
    
    DbSet<Domain.Category> Categories { get; set; }
    
    DbSet<Domain.FinancialGoal> FinancialGoals { get; set; }
    
    DbSet<Domain.Coin> Coins { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}