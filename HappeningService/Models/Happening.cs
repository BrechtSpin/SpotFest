namespace HappeningService.Models;

public class Happening
{
    public Guid Guid { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public string Slug { get; set; } = string.Empty;
    public List<HappeningArtist> HappeningArtists { get; set; } = [];
    public required DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}
