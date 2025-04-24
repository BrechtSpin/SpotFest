namespace ArtistService.Models;

public record ArtistMetric
{
    public Guid Guid { get; set; } = new Guid();
    public Guid ArtistGuid { get; set; }
    public DateTime Date { get; set; }
    public required int Followers { get; set; }
    public required int Popularity { get; set; }
    public long Listeners { get; set; }
    public Artist Artist { get; set; }
}
