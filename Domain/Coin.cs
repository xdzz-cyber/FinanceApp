namespace Domain;

public class Coin
{
    public Guid Id { get; set; }
    
    public string Name { get; set; } = null!;
    
    public string Symbol { get; set; } = null!;
    // Price in USD.
    public double PriceUsd { get; set; }

    public DateTime CreatedAt { get; set; }
 
    public DateTime UpdatedAt { get; set; }
}