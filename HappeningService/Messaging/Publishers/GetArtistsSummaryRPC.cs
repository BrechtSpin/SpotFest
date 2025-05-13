using Contracts;
using MassTransit;

namespace HappeningService.Messaging;

public partial class PublisherService
{
    public async Task<ArtistSummary[]?> GetArtistsSummaryRPCAsync(Guid[] artisGuids)
    {
        var request = new ArtistFetchSummariesRequest
        {
            CorrelationId = Guid.NewGuid(),
            ArtistGuids = artisGuids
        };

        var response = await _ArtistsFetchSummaryRPC.GetResponse<ArtistFetchSummariesResponse>(request);
        return response.Message.ArtistSummaries;
    }
}

