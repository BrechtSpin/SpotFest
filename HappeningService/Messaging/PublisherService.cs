using Contracts;
using MassTransit;

namespace HappeningService.Messaging;

public partial class PublisherService(IPublishEndpoint publish,
    IRequestClient<ArtistFetchSummariesRequest> ArtistsFetchSummaryRPC) : IPublisherService
{
    private readonly IPublishEndpoint _publish = publish;
    private readonly IRequestClient<ArtistFetchSummariesRequest> _ArtistsFetchSummaryRPC = ArtistsFetchSummaryRPC;
}
