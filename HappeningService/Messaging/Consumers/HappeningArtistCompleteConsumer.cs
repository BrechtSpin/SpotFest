using Contracts;
using HappeningService.Services;
using MassTransit;

namespace HappeningService.Messaging.Consumers;

public class HappeningArtistCompleteConsumer(IHappeningServices happeningServices) : IConsumer<HappeningArtistComplete>
{
    private readonly IHappeningServices _happeningServices = happeningServices;
    public async Task Consume(ConsumeContext<HappeningArtistComplete> context)
    {
        await _happeningServices.CreateHappeningArtistAsync(context.Message);
    }
}
