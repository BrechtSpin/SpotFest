using Contracts;
using MassTransit;

namespace ArtistService.Messaging;

public partial class PublisherService(
    IPublishEndpoint publish,
    ILogger<PublisherService> logger,
    IRequestClient<ArtistSpotifyRequest> artistSpotifyRequestClient) : IPublisherService 
{
    private readonly IPublishEndpoint _publish = publish;
    private readonly ILogger _logger = logger;
    private readonly IRequestClient<ArtistSpotifyRequest> _artistSpotifyRequestClient = artistSpotifyRequestClient;
}
