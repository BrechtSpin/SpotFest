namespace HappeningService.DTO;

public class CreateHappeningDTO
{
    public required string Name { get; set; }
    public required DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public List<HappeningArtistIncompleteDTO> HappeningArtists { get; set; } = [];
}
