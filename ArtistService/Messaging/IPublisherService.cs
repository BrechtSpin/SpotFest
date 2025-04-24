using Contracts;

namespace ArtistService.Messaging;

public interface IPublisherService
{
    public Task HappeningArtistCompletePublisher(HappeningArtistComplete completeHappeningArtist);
    public Task ArtistMetricTaskPublisher(ArtistIdMap artistIdMap);
}
