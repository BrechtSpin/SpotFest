using HappeningService.DTO;
using Contracts;

namespace HappeningService.Services;
public interface IHappeningServices
{
    public Task<HappeningWithArtistSummaries?> GetHappeningFullNoSlugAsync();
    public Task<HappeningWithArtistSummaries?> GetHappeningFullAsync(string slug);
    public Task<string> CreateHappeningAsync(CreateHappeningDTO createHappeningDTO);
    public Task CreateHappeningArtistAsync(HappeningArtistComplete happeningArtist);
}
