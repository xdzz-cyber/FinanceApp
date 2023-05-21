using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityTypeConfigurations;

public class MockConfiguration : IEntityTypeConfiguration<Mock>
{
    public void Configure(EntityTypeBuilder<Mock> builder)
    {
        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.Id).IsUnique();
    }
}