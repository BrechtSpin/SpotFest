using ArtistService.Services;
using Contracts;
using MassTransit;

namespace ArtistService.Messaging.Consumers;

public class ArtistMetricHarvestResponseConsumer(
    ILogger<ArtistMetricHarvestResponseConsumer> logger,
    IArtistServices artistServices) : IConsumer<ArtistMetric>
{
    private readonly ILogger _logger = logger;
    private readonly IArtistServices _artistServices = artistServices;
    public async Task Consume(ConsumeContext<ArtistMetric> context)
    {
        await _artistServices.ArtistMetricStoreInDB(context.Message);
    }
}
