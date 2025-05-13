using Contracts;

namespace HappeningService.Messaging
{
    public interface IPublisherService
    {
        Task HappeningArtistIncompletePublisher(HappeningArtistIncomplete happeningArtistIncomplete);
        Task<ArtistSummary[]?> GetArtistsSummaryRPCAsync(Guid[] guids);
    }
}
