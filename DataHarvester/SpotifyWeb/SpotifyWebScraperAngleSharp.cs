using System.Text;
using System.Text.RegularExpressions;
using AngleSharp;
using AngleSharp.Dom;

namespace DataHarvester.SpotifyWeb;

public class SpotifyWebScraperAngleSharp : ISpotifyWebScraper
{
    private static AngleSharp.IConfiguration config = Configuration.Default
        .WithDefaultLoader()
        .WithJs();
    private readonly BrowsingContext context;

    private static readonly string BaseUrl = "https://open.spotify.com/artist/";

    public SpotifyWebScraperAngleSharp()
    {
        context = new BrowsingContext(config);
    }

    public async Task<long> GetListenersAsync(string ArtistSpotUId)
    {
        var document = await context.OpenAsync(new Url($"{BaseUrl}{ArtistSpotUId}"));
        await document.WaitForReadyAsync();

        var element = document.QuerySelector("#initialState");
        if (element is null) return -1;  // no elements found matching query : failed connection or bad artistId

        var base64ToString = Encoding.UTF8.GetString(Convert.FromBase64String(element.InnerHtml));
        var listenersString = Regex.Match(base64ToString, @"monthlyListeners(.*?)\}").Value;
        return int.Parse(Regex.Match(listenersString, @"\d+").Value);
    }
}
