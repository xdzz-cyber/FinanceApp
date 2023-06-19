using System.Text.Json.Serialization;

namespace Application.Common.Dtos;

public class ClientDto
{
    [JsonPropertyName("clientId")]
    public string ClientId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("webHookUrl")]
    public string WebHookUrl { get; set; }

    [JsonPropertyName("permissions")]
    public string Permissions { get; set; }

    [JsonPropertyName("accounts")]
    public AccountDto[] Accounts { get; set; }

    [JsonPropertyName("jars")]
    public JarDto[] Jars { get; set; }
}