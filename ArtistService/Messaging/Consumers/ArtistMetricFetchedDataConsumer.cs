using ArtistService.Services;
using Contracts;
using MassTransit;

namespace ArtistService.Messaging.Consumers;

public class ArtistMetricFetchedDataConsumer(
    ILogger<ArtistMetricFetchedDataConsumer> logger,
    IArtistServices artistServices) : IConsumer<ArtistMetric>
{
    private readonly ILogger _logger = logger;
    private readonly IArtistServices _artistServices = artistServices;
    public async Task Consume(ConsumeContext<ArtistMetric> context)
    {
        var thisClass= this.ToString();
        _logger.LogDebug("Consuming {thisClass}", thisClass);
        await _artistServices.ArtistMetricStoreInDB(context.Message);
    }
}
