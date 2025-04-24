using System.Text.Json.Serialization;

namespace DataHarvester.Models;

public record SpotifyWebApiClientToken
{
    [JsonPropertyName("access_token")]
    public required string Access_token { get; init; }

    [JsonPropertyName("token_type")]
    public required string Token_type { get; init; }

    [JsonPropertyName("expires_in")]
    public int Expires_in { get; init; }
    [JsonPropertyName("expires_at")]
    public DateTime Expires_at { get; set; }
}


