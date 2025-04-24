using DataHarvester.Messaging;
using Contracts;

namespace DataHarvester.SpotifyWeb;

public class SpotifyHarvester(
    ISpotifyWebScraper scraper,
    ISpotifyWebApiClient webAPI,
    IPublisherService publisherService)
{
    private readonly ISpotifyWebScraper _scraper = scraper;
    private readonly ISpotifyWebApiClient _webAPI = webAPI;
    private readonly IPublisherService _publisherService = publisherService;

    public async Task FetchAndStoreSpotifyMetric(ArtistIdMap artistIdMap)
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
            await _publisherService.ArtistMetricFetchedDataPublisher(NewMetric);
        }
    }
}
