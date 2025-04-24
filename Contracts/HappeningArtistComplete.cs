namespace Contracts;

public record HappeningArtistComplete
{
    public Guid HappeningGuid { get; set; }
    public Guid ArtistGuid { get; set; }
}
