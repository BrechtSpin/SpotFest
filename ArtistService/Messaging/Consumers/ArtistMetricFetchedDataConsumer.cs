using ArtistService.Services;
using Contracts;
using MassTransit;

namespace ArtistService.Messaging.Consumers;

public class ArtistMetricFetchedDataConsumer(IArtistServices artistServices) : IConsumer<ArtistMetric>
{
    private readonly IArtistServices _artistServices = artistServices;
    public async Task Consume(ConsumeContext<ArtistMetric> context)
    {
        await _artistServices.ArtistMetricStoreInDB(context.Message);
    }
}
