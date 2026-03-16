namespace ArtistService.DTO;

public class ArtistMetricDTO
{
    public DateTime Date { get; set; }
    //9/3/2026 deprecated fields from spotify. may come back later? unlikely
    //public long Followers { get; set; }
    //public long Popularity { get; set; }
    public long Listeners { get; set; }
}
