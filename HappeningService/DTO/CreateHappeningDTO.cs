namespace HappeningService.DTO;

public class CreateHappeningDTO
{
    public required string Name { get; set; }
    public required DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public List<HappeningArtistIncompleteDTO> HappeningArtists { get; set; } = [];
}
