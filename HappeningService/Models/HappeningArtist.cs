namespace HappeningService.Models;

public class HappeningArtist
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public Guid HappeningGuid { get; set; }
    public Guid ArtistGuid { get; set; }
    public Happening Happening { get; set; } = default!;
    public DateTime Showtime { get; set; }
}
