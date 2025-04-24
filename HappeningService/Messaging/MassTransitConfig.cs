using HappeningService.Messaging.Consumers;
using MassTransit;
namespace HappeningService.Messaging;

public static class MassTransitConfig
{
    public static void AddMassTransitConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddConsumer<HappeningArtistCompleteConsumer>();

            config.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMQHost = configuration.GetValue<string>("RabbitMq:Host");
                cfg.Host(rabbitMQHost, h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}