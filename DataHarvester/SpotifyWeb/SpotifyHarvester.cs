using DataHarvester.Messaging;
using Contracts;
using DataHarvester.Models;

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
            //var artistData = await _webAPI.GetArtistAsync(artistIdMap.SpotifyId);
            var metrics = await _scraper.GetMetricsAsync(artistIdMap.SpotifyId);
                var NewMetric = new ArtistMetric
                {
                    Guid = Guid.NewGuid(),
                    ArtistGuid = artistIdMap.ArtistGuid,
                    Date = DateTime.UtcNow,
                    //9/3/2026 deprecated fields from spotify. may come back later? unlikely
                    //Popularity = artistData.Popularity,
                    Listeners = metrics.listeners,
                    Followers = metrics.followers

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
