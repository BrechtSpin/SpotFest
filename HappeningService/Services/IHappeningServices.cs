using HappeningService.DTO;
using Contracts;

namespace HappeningService.Services;
public interface IHappeningServices
{
    public Task<HappeningWithArtistSummaries?> GetHappeningFullNoSlugAsync();
    public Task<HappeningWithArtistSummaries?> GetHappeningFullAsync(string slug);
    public Task<string> CreateHappeningAsync(CreateHappeningDTO createHappeningDTO);
    public Task CreateHappeningArtistAsync(HappeningArtistComplete happeningArtist);
    public Task UpdateHappeningAsync(UpdateHappeningDTO updateHappeningDTO);
    public Task<HappeningSummaryDTO[]> GetHappeningsOfArtistAsync(string ArtistGuid);
    public Task<HappeningSummaryDTO[]> GetHappeningsSearch(int year, int month, int index);
}
