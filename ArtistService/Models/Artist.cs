namespace ArtistService.Models;

public record Artist
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public string SpotifyId { get; set; }
    public string Name { get; set; }
    public required string PictureSmallUrl { get; set; }
    public required string PictureMediumUrl { get; set; }

    public ICollection<ArtistMetric> Metrics { get; set; }
}
