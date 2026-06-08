using System.Text;
using System.Text.RegularExpressions;
using System.Threading.RateLimiting;
using AngleSharp;
using AngleSharp.Dom;
using Polly;
using Polly.Retry;

namespace DataHarvester.SpotifyWeb;

public class SpotifyWebScraperAngleSharp : ISpotifyWebScraper
{
    private static readonly string BaseUrl = "https://open.spotify.com/artist/";
    private readonly SlidingWindowRateLimiter _rateLimiter;
    private readonly ResiliencePipeline<IDocument> _pipeline;
    private static readonly Random Jitterer = new();
    private static AngleSharp.IConfiguration config = Configuration.Default
        .WithDefaultLoader()
        .WithJs();
    private readonly BrowsingContext context;


    public SpotifyWebScraperAngleSharp(SpotifyWebScraperRateLimiter rateLimiter)
    {
        _rateLimiter = rateLimiter.GetSlidingWindowRateLimiter();
        _pipeline = new ResiliencePipelineBuilder<IDocument>()
            .AddRetry(new RetryStrategyOptions<IDocument>
            {
                ShouldHandle = new PredicateBuilder<IDocument>()
                    .Handle<HttpRequestException>()     //network errors, unavailable etc
                    .HandleResult(doc =>
                    {
                        return doc.QuerySelector("#initialState") is null;
                    }),
                MaxRetryAttempts = 5,
                DelayGenerator = args =>
                {
                    var jitter = Jitterer.NextDouble() + 1; //1 to 2
                    return new ValueTask<TimeSpan?>(
                        TimeSpan.FromSeconds(10 * jitter * Math.Pow(2, args.AttemptNumber)));
                },
                OnRetry = args =>
                {
                    if (args.Outcome.Exception != null)
                    {
                        Console.WriteLine($"[Polly] Retry {args.AttemptNumber} due to exception: {args.Outcome.Exception.Message}");
                    }
                    else
                    {
                        Console.WriteLine($"[Polly] Retry {args.AttemptNumber} due to invalid document content.");
                    }
                    return ValueTask.CompletedTask;
                }
            })
            .Build();
        context = new BrowsingContext(config);
    }

    public async Task<(long listeners, int followers)> GetMetricsAsync(string ArtistSpotUId)
    {
        using var lease = await _rateLimiter.AcquireAsync(1);

        IDocument document = await _pipeline.ExecuteAsync<IDocument>(async ct =>
        {
            return await context.OpenAsync(new Url($"{BaseUrl}{ArtistSpotUId}"), ct);
        });
        await document.WaitForReadyAsync();

        var element = document.QuerySelector("#initialState");
        if (element is null)
        {
            Console.WriteLine($"{DateTime.UtcNow}: {ArtistSpotUId}: failed to resolve");
            return (-1, -1);  // no elements found matching query : failed connection or bad artistId
        }

        var base64ToString = Encoding.UTF8.GetString(Convert.FromBase64String(element.InnerHtml));
        var listenersString = Regex.Match(base64ToString, @"monthlyListeners"":(.*?)\}").Groups[1].Value;
        long.TryParse(listenersString, out long listeners);
        var followersString = Regex.Match(base64ToString, @"followers"":(.*?),").Groups[1].Value;
        int.TryParse(followersString, out int followers);

        return (listeners, followers);
    }
}
