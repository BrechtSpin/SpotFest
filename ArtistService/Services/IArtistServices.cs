using ArtistService.DTO;
using Contracts;

namespace ArtistService.Services;

public interface IArtistServices
{
    public Task ResolveHappeningArtist(HappeningArtistIncomplete incompleteHappeningArtist);
    public Task ArtistMetricStoreInDB(ArtistMetric artistMetric);
    public Task ArtistMetricDataJob(SchedulerJob Job);
    public Task<ArtistSummary[]> ArtistSummariesFromGuids(Guid[] guids);
    public Task<ArtistWithMetricsDTO?> GetArtistWithMetrics(Guid guid);


}
