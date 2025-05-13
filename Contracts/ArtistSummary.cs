namespace Contracts;

public record ArtistSummary
{
    public required Guid Guid { init; get; }
    public required string Name { init; get; }
    public required string PictureSmallUrl { init; get; }
}
