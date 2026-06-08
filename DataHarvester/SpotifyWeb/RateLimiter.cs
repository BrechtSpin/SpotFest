using System.Threading.RateLimiting;

namespace DataHarvester.SpotifyWeb;

public class SpotifyWebApiRateLimiter
{
    private readonly SlidingWindowRateLimiter _rateLimiter =
        new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
        {
            // different endpoints may have different limits or have shared limits
            // no exact info is available
            Window = TimeSpan.FromSeconds(30),  //per the api documentation
            SegmentsPerWindow = 150, // .2s "granularity"
            PermitLimit = 100,  //calls per window
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = int.MaxValue, 
        });
    public SlidingWindowRateLimiter GetSlidingWindowRateLimiter() => _rateLimiter; 
}

public class SpotifyWebScraperRateLimiter
{
    private readonly SlidingWindowRateLimiter _rateLimiter = 
        new SlidingWindowRateLimiter(new SlidingWindowRateLimiterOptions
        {   //conservative 1 per second max webpage request
            Window = TimeSpan.FromSeconds(5),
            SegmentsPerWindow = 10, // .5s "granularity"
            PermitLimit = 5,  //calls per window
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = int.MaxValue,
        });
    public SlidingWindowRateLimiter GetSlidingWindowRateLimiter() => _rateLimiter;
}
