using ArtistService.Services;
using Contracts;
using MassTransit;

namespace ArtistService.Messaging.Consumers;

public class ArtistFetchSummariesRequestConsumer(
    IArtistServices artistServices,
    IPublisherService publisherService) : IConsumer<ArtistFetchSummariesRequest>
{
    private readonly IArtistServices _artistServices = artistServices;
    private readonly IPublisherService _publisherService = publisherService;
    public async Task Consume(ConsumeContext<ArtistFetchSummariesRequest> context)
    {
        var artists = await _artistServices.ArtistSummariesFromGuids(context.Message.ArtistGuids);
        await _publisherService.ArtistFetchSummariesResponsePublisher(context, artists);
    }
}
