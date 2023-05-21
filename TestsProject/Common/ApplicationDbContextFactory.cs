using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace TestsProject.Common;

public class ApplicationDbContextFactory
{
    public static Guid MockId { get; } = Guid.NewGuid(); //Guid.Parse("b3e3a0a0-0b9a-4b1e-9e5a-9b2b4d6a7c7e");

    public static ApplicationDbContext Create()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        var context = new ApplicationDbContext(options);
        context.Database.EnsureCreated();
        context.Mocks.AddRange(new []
        {
            new Mock{Id = Guid.Parse("b3e3a0a0-0b9a-4b1e-9e5a-9b2b4d6a7c7e")},
            new Mock{Id = Guid.Parse("14b5cfab-e60f-4362-acb0-5ff96ef949a9")}           
        });

        context.SaveChanges();
        return context;
    }
    
    public static void Destroy(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Dispose();
    }
}