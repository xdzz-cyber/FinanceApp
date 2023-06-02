using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class FinancialGoalConfiguration : IEntityTypeConfiguration<FinancialGoal>
{
    public void Configure(EntityTypeBuilder<FinancialGoal> builder)
    {
        // Financial goal belongs to a specific budget.
        builder.HasOne(fg => fg.Budget)
            .WithMany(b => b.FinancialGoals)
            .HasForeignKey(fg => fg.BudgetId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}