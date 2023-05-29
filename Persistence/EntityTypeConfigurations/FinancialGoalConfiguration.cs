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
        
        // Many-to-One with User: A financial goal belongs to a specific user.
        // builder.HasOne(fg => fg.AppUser)
        //     .WithMany(u => u.FinancialGoals)
        //     .HasForeignKey(fg => fg.AppUserId)
        //     .OnDelete(DeleteBehavior.Cascade);
        //
        // Many-to-One with Budget: A financial goal belongs to a specific budget.
        // builder.HasOne(fg => fg.Budget)
        //     .WithMany(b => b.FinancialGoals)
        //     .HasForeignKey(fg => fg.BudgetId)
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}