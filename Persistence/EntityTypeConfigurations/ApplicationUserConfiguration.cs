using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        // One-to-Many with Budget: A user can have multiple budgets.
        builder.HasMany<Budget>()
            .WithOne(b => b.AppUser)
            .HasForeignKey(b => b.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // One-to-Many with Transaction: A user can have multiple transactions.
        // builder.HasMany<Transaction>()
        //     .WithOne(t => t.AppUser)
        //     .HasForeignKey(t => t.AppUserId)
        //     .OnDelete(DeleteBehavior.Cascade);
        
        // One-to-Many with FinancialGoal: A user can have multiple financial goals.
        // builder.HasMany<FinancialGoal>()
        //     .WithOne(fg => fg.AppUser)
        //     .HasForeignKey(fg => fg.AppUserId)
        //     .OnDelete(DeleteBehavior.Cascade);
    }
}