namespace ArtistService.Models;

public record ArtistMetric
{
    public Guid Guid { get; set; } = new Guid();
    public Guid ArtistGuid { get; set; }
    public DateTime Date { get; set; }
    public DateTime DateDay { get; private set; }
    //9/3/2026 deprecated fields from spotify. may come back later? unlikely
    [Obsolete("deprecated fields from spotify. may come back later? unlikely")]
    public int? Followers { get; set; }
    [Obsolete("deprecated fields from spotify. may come back later? unlikely")]
    public int? Popularity { get; set; }
    public long Listeners { get; set; }
    public Artist Artist { get; set; }
}
