using Contracts;

namespace ArtistService.Messaging;

public partial class PublisherService
{
    public Task HappeningArtistCompletePublisher(HappeningArtistComplete ha)
    {
        return _publish.Publish(ha);
    }
}