using HappeningService.DTO;
using Contracts;

namespace HappeningService.Services;
public interface IHappeningService
{
    public Task<string> CreateHappeningAsync(CreateHappeningDTO createHappeningDTO);
    public Task CreateHappeningArtistAsync(HappeningArtistComplete happeningArtist);
    public Task<List<HappeningSummaryDTO>> GetCurrentAndUpcomingHappeningAsync();
}
