using Contracts;
using LoggerService.Services;
using MassTransit;

namespace LoggerService.Messaging.Consumers;

public class ChangeLogMessageConsumer(ILoggerServices loggerServices) : IConsumer<ChangeLogMessage>
{
    private readonly ILoggerServices _loggerServices = loggerServices;
    public async Task Consume(ConsumeContext<ChangeLogMessage> context)
    {
        await _loggerServices.ChangeLogMessageReceived(context.Message);
    }
}
