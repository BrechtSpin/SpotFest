using Contracts;

namespace ArtistService.Messaging;

public partial class PublisherService
{
    public Task ArtistMetricHarvestRequestPublisher(ArtistIdMap artistIdMap)
    {
        return _publish.Publish(artistIdMap);
    }
}
