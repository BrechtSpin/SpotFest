using HappeningService.Models;

namespace HappeningService.Services;
public interface IHappeningServices
{
    public Task HappeningArtistStoreInDB(HappeningArtist happeningArtist);
}
