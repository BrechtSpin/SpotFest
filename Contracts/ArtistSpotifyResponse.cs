namespace Contracts;

public record ArtistSpotifyResponse
{
    public required string Name { get; init; }
    public required string SpotifyId { get; init; }
    public required string PictureSmallUrl { get; init; }
    public required string PictureMediumUrl { get; init; }
}
