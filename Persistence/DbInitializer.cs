namespace Persistence;

public static class DbInitializer
{
    public static void Initialize(ApplicationDbContext context)
    {
        //context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }
}