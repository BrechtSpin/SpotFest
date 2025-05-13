using HappeningService.DTO;
using Contracts;
using HappeningService.Models;

namespace HappeningService.Services;
public interface IHappeningService
{
    public Task<HappeningWithArtistSummaries?> GetHappeningFullNoSlugAsync();
    public Task<HappeningWithArtistSummaries?> GetHappeningFullAsync(string slug);
    public Task<string> CreateHappeningAsync(CreateHappeningDTO createHappeningDTO);
    public Task CreateHappeningArtistAsync(HappeningArtistComplete happeningArtist);
    public Task<List<HappeningSummaryDTO>> GetCurrentAndUpcomingHappeningAsync();
}
