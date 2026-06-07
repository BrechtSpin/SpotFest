namespace Contracts;

public record ArtistMetric
{
    public required Guid Guid { get; set; } = Guid.NewGuid();
    public required Guid ArtistGuid { get; set; }
    public required DateTime Date { get; set; }
    //9/3/2026 deprecated fields from spotify. may come back later? unlikely
    //public required int Popularity { get; set; }
    public long Listeners { get; set; } 
    public int Followers { get; set; }
}
