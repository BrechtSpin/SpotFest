namespace ArtistService.DTO;

public class ArtistMetricDTO
{
    public DateTime Date { get; set; }
    public long Listeners { get; set; }
    public long Followers { get; set; }
    //9/3/2026 deprecated fields from spotify. may come back later? unlikely
    //public long Popularity { get; set; }
}
