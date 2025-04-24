using Contracts;

namespace ArtistService.Messaging;

public partial class PublisherService
{
    public Task ArtistMetricTaskPublisher(ArtistIdMap artistIdMap)
    {
        return _publish.Publish(artistIdMap);
    }

}
