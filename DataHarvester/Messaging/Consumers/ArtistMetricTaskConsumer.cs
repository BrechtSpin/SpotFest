using Contracts;
using DataHarvester.SpotifyWeb;
using MassTransit;

namespace DataHarvester.Messaging.Consumers
{
    public class ArtistMetricTaskConsumer(SpotifyHarvester spotifyHarvester) :IConsumer<ArtistIdMap>
    {
        private readonly SpotifyHarvester _spotifyHarvester = spotifyHarvester;

        public async Task Consume(ConsumeContext<ArtistIdMap> context)
        {
            await _spotifyHarvester.FetchAndStoreSpotifyMetric(context.Message);
        }
    }
}
