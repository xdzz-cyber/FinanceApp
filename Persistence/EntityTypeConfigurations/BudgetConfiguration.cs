using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
{
    public void Configure(EntityTypeBuilder<Budget> builder)
    {
        // Many-to-One with User: A budget belongs to a specific user.
        builder.HasOne(b => b.AppUser)
            .WithMany(u => u.Budgets)
            .HasForeignKey(b => b.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // One budget can have many financial goals.
        builder.HasMany<FinancialGoal>(b => b.FinancialGoals)
            .WithOne(fg => fg.Budget)
            .HasForeignKey(fg => fg.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // One budget can have many transactions.
        builder.HasMany<Transaction>(b => b.Transactions)
            .WithOne(t => t.Budget)
            .HasForeignKey(t => t.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}