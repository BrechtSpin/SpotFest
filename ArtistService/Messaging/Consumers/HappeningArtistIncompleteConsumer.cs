using ArtistService.Services;
using Contracts;
using MassTransit;

namespace ArtistService.Messaging.Consumers;

public class HappeningArtistIncompleteConsumer(
    ILogger<HappeningArtistIncompleteConsumer> logger,
    IArtistServices artistServices) : IConsumer<HappeningArtistIncomplete>
{
    private readonly ILogger _logger = logger;
    private readonly IArtistServices _artistServices = artistServices;
    public async Task Consume(ConsumeContext<HappeningArtistIncomplete> context)
    {
        var thisClass = this.ToString();
        _logger.LogDebug("Consuming {thisClass}", thisClass);
        await _artistServices.ResolveHappeningArtist(context.Message);
    }
}
