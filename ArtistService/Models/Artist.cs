namespace ArtistService.Models;

public record Artist
{
    public Guid Guid { get; set; } = new Guid();
    public string SpotifyId { get; set; }
    public string Name { get; set; }

    public ICollection<ArtistMetric> Metrics { get; set; }
}
