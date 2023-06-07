using System.Text.Json.Serialization;

namespace Application.Common.Dtos;

public class CoinListDto
{
    [JsonPropertyName("data")]
    public IEnumerable<CoinDto> Data { get; set; } = null!;
}