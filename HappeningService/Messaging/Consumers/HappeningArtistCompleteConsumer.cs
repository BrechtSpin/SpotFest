using Contracts;
using HappeningService.Services;
using MassTransit;

namespace HappeningService.Messaging.Consumers;

public class HappeningArtistCompleteConsumer(IHappeningServices happeningServices) : IConsumer<HappeningArtistComplete>
{
    private readonly IHappeningServices _happeningServices = happeningServices;
    public Task Consume(ConsumeContext<HappeningArtistComplete> context)
    {
        var msg = context.Message;
        _happeningServices.HappeningArtistStoreInDB(new Models.HappeningArtist
        {
            Guid = Guid.NewGuid(),
            HappeningGuid = msg.HappeningGuid,
            ArtistGuid = msg.ArtistGuid,
        });
        return Task.CompletedTask;
    }
}
