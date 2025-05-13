using Contracts;

namespace ArtistService.Messaging;

public partial class PublisherService
{
    public Task ArtistMetricHarvestRequestPublisher(ArtistIdMap artistIdMaps)
    {
        return _publish.Publish(artistIdMaps);
    }
}
