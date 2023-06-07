using System.Text.Json;
using Application.Common.Dtos;
using Application.Interfaces;
using Domain;

namespace Persistence;

public static class DbInitializer
{
    public static async Task Initialize(ApplicationDbContext context)
    {
        //context.Database.EnsureDeleted();
        await context.Database.EnsureCreatedAsync();

        if (!context.Coins.Any())
        {
            await SeedCoins(context);
        }
    }
    
    // Seed coins.
    private static async Task SeedCoins(IApplicationDbContext context, CancellationToken cancellationToken = default)
    {
        var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("https://api.coincap.io/v2/assets", cancellationToken);
        var coins = await response.Content.ReadAsStringAsync(cancellationToken);
        var parsedCoins = JsonSerializer.Deserialize<CoinListDto>(coins);
        
        await context.Coins.AddRangeAsync(parsedCoins!.Data.Select(coin => new Coin
        {
            Id = Guid.NewGuid(),
            Name = coin.Name,
            Symbol = coin.Symbol,
            PriceUsd = Convert.ToDouble(coin.PriceUsd),
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        }).ToArray(), cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}