using DataHarvester.Messaging;
using Contracts;
using DataHarvester.Models;
using Jint.Parser.Ast;

namespace DataHarvester.SpotifyWeb;

public class SpotifyHarvester(
    ISpotifyWebScraper scraper,
    ISpotifyWebApiClient webAPI,
    IPublisherService publisherService)
{
    private readonly ISpotifyWebScraper _scraper = scraper;
    private readonly ISpotifyWebApiClient _webAPI = webAPI;
    private readonly IPublisherService _publisherService = publisherService;

    public async Task GetSpotifyMetric(ArtistIdMap artistIdMap)
    {
        if (artistIdMap is not null && artistIdMap.SpotifyId is not null)
        {
            var artistData = await _webAPI.GetArtistAsync(artistIdMap.SpotifyId);
            var listeners = await _scraper.GetListenersAsync(artistIdMap.SpotifyId);
            var NewMetric = new ArtistMetric
            {
                ArtistGuid = artistIdMap.ArtistGuid,
                Date = DateTime.UtcNow,
                Followers = artistData.Followers.total,
                Popularity = artistData.Popularity,
                Listeners = listeners
            };
            await _publisherService.ArtistMetricDataResponsePublisher(NewMetric);
        }
    }
    public async Task<ArtistSpotifyResponse> GetArtistFromSpotify(ArtistSpotifyRequest request)
    {
        ArtistSpotifyResponse response;
        SpotifyArtist spotifyArtist;
        if (request.SpotifyId is null || request.SpotifyId == "")
        {
            spotifyArtist = (await _webAPI.GetArtistsByNameAsync(request.Name, 1))[0];
            //what happens with by spotID? if name isn't matched?
            if (spotifyArtist.Name != request.Name) throw new ArgumentException("Name doesn't match");
        }
        else
        {
            spotifyArtist = await _webAPI.GetArtistAsync(request.SpotifyId);
        }

        var images = spotifyArtist.Images
            .OrderBy(x => x.height)
            .ToList();
        response = new()
        {
            Name = spotifyArtist.Name,
            SpotifyId = spotifyArtist.Id,
            PictureSmallUrl = images[0].url,
            PictureMediumUrl = images[1].url,
        };
        return response;
    }

}
