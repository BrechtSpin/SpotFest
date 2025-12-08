using Contracts;

namespace HappeningService.Messaging
{
    public interface IPublisherService
    {
        Task ChangeLogMessagePublisher(ChangeLogMessage message);
        Task HappeningArtistIncompletePublisher(HappeningArtistIncomplete happeningArtistIncomplete);
        Task<ArtistSummary[]?> GetArtistsSummaryRPCAsync(Guid[] guids);
    }
}
