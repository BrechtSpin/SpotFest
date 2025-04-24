using Contracts;

namespace ArtistService.Services;

public interface IArtistServices
{
    public Task ResolveHappeningArtist(HappeningArtistIncomplete incompleteHappeningArtist);
    public Task ArtistMetricStoreInDB(ArtistMetric artistMetric);
}
