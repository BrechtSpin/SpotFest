using HappeningService.Models;
using HappeningService.Repositories;

namespace HappeningService.Services;

public class HappeningServices(HappeningContext happeningContext) : IHappeningServices
{
    private readonly HappeningContext _happeningContext = happeningContext;
    public async Task HappeningArtistStoreInDB(HappeningArtist happeningArtist)
    {
        await _happeningContext.HappeningArtists.AddAsync(happeningArtist);
        await _happeningContext.SaveChangesAsync();
    }
}
