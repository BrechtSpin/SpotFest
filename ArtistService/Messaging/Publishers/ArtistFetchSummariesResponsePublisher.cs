using Contracts;
using MassTransit;

namespace ArtistService.Messaging;

public partial class PublisherService
{ 
    public async Task ArtistFetchSummariesResponsePublisher(ConsumeContext<ArtistFetchSummariesRequest> context, ArtistSummary[] artistSummaries)
    {
        var response =
        new ArtistFetchSummariesResponse
        {
            CorrelationId = context.Message.CorrelationId,
            ArtistSummaries = artistSummaries
        };
        await context.RespondAsync(response);
    }
}
