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
            config.AddConsumer<ArtistMetricHarvestResponseConsumer>();
            config.AddConsumer<ArtistFetchSummariesRequestConsumer>();
            config.AddConsumer<ArtistMetricJobConsumer>();

            config.AddRequestClient<ArtistSpotifyRequest>(timeout: 100000000);

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