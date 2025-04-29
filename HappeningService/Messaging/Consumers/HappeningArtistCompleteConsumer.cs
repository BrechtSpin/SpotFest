using Contracts;
using HappeningService.Services;
using MassTransit;

namespace HappeningService.Messaging.Consumers;

public class HappeningArtistCompleteConsumer(IHappeningService happeningServices) : IConsumer<HappeningArtistComplete>
{
    private readonly IHappeningService _happeningServices = happeningServices;
    public async Task Consume(ConsumeContext<HappeningArtistComplete> context)
    {
        await _happeningServices.CreateHappeningArtistAsync(context.Message);
    }
}
