using ArtistService.Messaging.Consumers;
using Contracts;
using MassTransit;
namespace ArtistService.Messaging;

public static class MassTransitConfig
{
    public static void AddMassTransitConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddConsumer<HappeningArtistIncompleteConsumer>();
            config.AddConsumer<ArtistMetricFetchedDataConsumer>();

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