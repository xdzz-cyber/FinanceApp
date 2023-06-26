using System.Text.Json.Serialization;

namespace Application.Common.Dtos;

public class JarDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("sendId")]
    public string SendId { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("currencyCode")]
    public int CurrencyCode { get; set; }

    [JsonPropertyName("balance")]
    public decimal Balance { get; set; }

    [JsonPropertyName("goal")]
    public decimal Goal { get; set; }
    
    [JsonPropertyName("stripeId")] public string? StripeId { get; set; }
}