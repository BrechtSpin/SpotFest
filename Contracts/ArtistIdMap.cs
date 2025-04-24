namespace Contracts;

public record ArtistIdMap
{
    public required Guid ArtistGuid { get; init; }
    public string? SpotifyId { get; init; }
}
