using Contracts;

namespace HappeningService.Messaging;

public partial class PublisherService
{
    public Task IncompleteHappeningArtistPublisher(Guid happeningGuid, string spotifyId, string name)
    {
        return _publish.Publish(new HappeningArtistIncomplete
        {
            HappeningGuid = happeningGuid,
            SpotifyId = spotifyId,
            Name = name
        });
    }
}
