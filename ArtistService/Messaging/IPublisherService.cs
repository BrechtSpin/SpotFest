using Contracts;
using MassTransit;

namespace ArtistService.Messaging;

public interface IPublisherService
{
    public Task HappeningArtistCompletePublisher(HappeningArtistComplete completeHappeningArtist);
    public Task ArtistMetricHarvestRequestPublisher(ArtistIdMap artistIdMap);
    public Task ArtistFetchSummariesResponsePublisher(ConsumeContext<ArtistFetchSummariesRequest> context, ArtistSummary[] artistSummaries);
    public Task<ArtistSpotifyResponse> GetArtistSpotifyAsync(ArtistSpotifyRequest request);
}
