using Contracts;
using DataHarvester.Messaging.Consumers;
using MassTransit;

namespace DataHarvester.Messaging;

public static class MassTransitConfig
{
    public static void AddMassTransitConfiguration(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.AddConsumer<ArtistMetricRequestConsumer>();
            config.AddConsumer<ArtistSpotifyRequestConsumer>();

            config.UsingRabbitMq((context, cfg) =>
            {
                var rabbitMqHost = configuration["RabbitMq:HostName"];
                var rabbitMqUser = configuration["RabbitMq:UserName"];
                var rabbitMqPass = configuration["RabbitMq:Password"];
                cfg.Host(rabbitMqHost, h =>
                {
                    h.Username(rabbitMqUser);
                    h.Password(rabbitMqPass);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}