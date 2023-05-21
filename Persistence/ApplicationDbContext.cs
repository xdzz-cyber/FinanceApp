using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.EntityTypeConfigurations;

namespace Persistence;

public class ApplicationDbContext : DbContext,IApplicationDbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<Mock> Mocks { get; set; }
    

    public ApplicationDbContext()
    {
      
    }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions, IConfiguration configuration = null) : base(dbContextOptions)
    {
       _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        if (_configuration != null)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MockConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}