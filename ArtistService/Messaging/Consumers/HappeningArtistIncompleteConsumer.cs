using ArtistService.Services;
using Contracts;
using MassTransit;

namespace ArtistService.Messaging.Consumers;

public class HappeningArtistIncompleteConsumer(IArtistServices artistServices) : IConsumer<HappeningArtistIncomplete>
{
    private readonly IArtistServices _artistServices = artistServices;
    public async Task Consume(ConsumeContext<HappeningArtistIncomplete> context)
    {
       await _artistServices.ResolveHappeningArtist(context.Message);
    }
}
