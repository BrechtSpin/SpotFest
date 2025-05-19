using DataHarvester.Models;
using MassTransit;
using System.Threading.Tasks;
using MassTransit.JobService.Messages;
using Polly;
using Polly.Retry;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.RateLimiting;

namespace DataHarvester.SpotifyWeb;

public class SpotifyWebApiClient : ISpotifyWebApiClient
{
    private static readonly Random Jitterer = new();
    private readonly HttpClient _httpClient;
    private readonly SlidingWindowRateLimiter _rateLimiter;
    private readonly ResiliencePipeline<HttpResponseMessage> _pipeline;
    private const string ApiUri = "https://api.spotify.com/v1";
    private readonly SpotifyWebApiClientTokenClient _tokenClient;

    public SpotifyWebApiClient(
        HttpClient httpclient,
        SpotifyRateLimiter spotifyRateLimiter,
        SpotifyWebApiClientTokenClient spotifyWebApiClientTokenClient)
    {
        _httpClient = httpclient;
        _rateLimiter = spotifyRateLimiter.GetSlidingWindowRateLimiter();
        _tokenClient = spotifyWebApiClientTokenClient;
        _pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddRetry(new()
            {
                ShouldHandle = args => ValueTask.FromResult(
                    args.Outcome.Exception is not null ||
                    args.Outcome.Result?.StatusCode == HttpStatusCode.TooManyRequests),
                MaxRetryAttempts = 5,
                DelayGenerator = args =>
                {
                    var jitter = Jitterer.NextDouble() + 1; //1 to 2
                    return new ValueTask<TimeSpan?>(
                        TimeSpan.FromSeconds(10 * jitter * Math.Pow(2, args.AttemptNumber)));
                }
            })
            .Build();
    }

    private async Task<string> GetSpotifyRequestAsync(string requestUri)
    {
        // limiter before token
        using var lease = await _rateLimiter.AcquireAsync(1);
        var token = await _tokenClient.GetTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
        request.Headers.Authorization =
            new AuthenticationHeaderValue(token.Token_type, token.Access_token);

        var response = await _pipeline.ExecuteAsync<HttpResponseMessage>(async ct =>
            await _httpClient.SendAsync(request, ct));

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    using StreamReader reader = new(await response.Content.ReadAsStreamAsync());
                    return await reader.ReadToEndAsync();
                }
            case HttpStatusCode.BadRequest:
                {
                    throw new ArgumentException($"invalid Uri {requestUri}", nameof(requestUri));
                }
            default:
                throw new NotImplementedException(response.StatusCode.ToString());
        }
    }
    public async Task<SpotifyArtist> GetArtistAsync(string spotifyId)
    {
        var message = await GetSpotifyRequestAsync(
            $"{ApiUri}/artists/{spotifyId}");
        return JsonSerializer.Deserialize(message, SerializerContext.Default.SpotifyArtist)!;
    }
    public async Task<List<SpotifyArtist>> GetArtistsByNameAsync(string artistName, int amount = 5)
    {
        var message = await GetSpotifyRequestAsync(
            $"{ApiUri}/search?q={Uri.EscapeDataString(artistName)}&type=artist&limit={amount}");
        var artistResponse = JsonSerializer.Deserialize(message, SerializerContext.Default.SpotifyArtistResponse)!;
        return artistResponse?.Artists?.Items ?? [];
    }

}
