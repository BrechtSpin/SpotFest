using System.Text.Json.Serialization;

namespace DataHarvester.Models;

public class SpotifyArtistResponse
{
    [JsonPropertyName("artists")]
    public ArtistsContainer Artists { get; set; }
}
public class ArtistsContainer
{
    [JsonPropertyName("items")]
    public List<SpotifyArtist> Items { get; set; }
}

public class SpotifyArtist
{
    [JsonPropertyName("external_urls")]
    public required ExternalUrls ExternalUrls { get; set; }

    [JsonPropertyName("href")]
    public required string Href { get; set; }

    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("images")]
    public required List<Image> Images { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("type")]
    public required string Type { get; set; }

    [JsonPropertyName("uri")]
    public required string Uri { get; set; }

    //9/3/2026 deprecated fields from spotify. may come back later? unlikely
    //[JsonPropertyName("followers")]
    //public required Followers Followers { get; set; }

    //[JsonPropertyName("genres")]
    //public required List<string> Genres { get; set; }

    //[JsonPropertyName("popularity")]
    //public int Popularity { get; set; }

}


public record ExternalUrls
{
    [JsonPropertyName("spotify")]
    public required string spotify { get; set; }
}

//9/3/2026 deprecated fields from spotify. may come back later? unlikely
//public record Followers
//{
//    [JsonPropertyName("href")]
//    public required object href { get; set; }

//    [JsonPropertyName("total")]
//    public int total { get; set; }
//}

public record Image
{
    [JsonPropertyName("url")]
    public string url { get; set; }

    [JsonPropertyName("height")]
    public int height { get; set; }

    [JsonPropertyName("width")]
    public int width { get; set; }
}