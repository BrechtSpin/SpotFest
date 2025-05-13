using Contracts;

namespace HappeningService.Messaging;

public partial class PublisherService
{
    public Task HappeningArtistIncompletePublisher(HappeningArtistIncomplete happeningArtistIncomplete)
    {
        return _publish.Publish(happeningArtistIncomplete);
    }
}
