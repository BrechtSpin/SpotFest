using Contracts;
using HappeningService.Models;

namespace HappeningService.DTO;

public class HappeningWithArtistSummaries
{
    public required string Name { get; set; }
    public required string Slug { get; set; }
    public ArtistSummary[] ArtistSummaries { get; set; } = [];
    public required DateOnly StartDate { get; set; }
    public required DateOnly EndDate { get; set; }
}
