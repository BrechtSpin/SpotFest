using Contracts;
using DataHarvester.SpotifyWeb;
using MassTransit;

namespace DataHarvester.Messaging.Consumers
{
    public class ArtistSpotifyRequestConsumer(SpotifyHarvester spotifyHarvester) : IConsumer<ArtistSpotifyRequest>
    {
        private readonly SpotifyHarvester _spotifyHarvester = spotifyHarvester;
        public async Task Consume(ConsumeContext<ArtistSpotifyRequest> context)
        {
            var response = await _spotifyHarvester.GetArtistFromSpotify(context.Message);
            await context.RespondAsync<ArtistSpotifyResponse>(response);
        }
    }
}
