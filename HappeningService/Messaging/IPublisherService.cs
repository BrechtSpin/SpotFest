namespace HappeningService.Messaging
{
    public interface IPublisherService
    {
        Task IncompleteHappeningArtistPublisher(Guid happeningGuid, string spotifyId, string name);
    }
}
