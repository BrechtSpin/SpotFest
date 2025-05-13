namespace Contracts;

public record ArtistSpotifyRequest
{
    public string? Name { get; init; }
    public string? SpotifyId { get; init; }
}
