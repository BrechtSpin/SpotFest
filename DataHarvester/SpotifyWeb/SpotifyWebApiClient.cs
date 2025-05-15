using DataHarvester.Models;
using MassTransit;
using MassTransit.JobService.Messages;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.RateLimiting;

namespace DataHarvester.SpotifyWeb;

public class SpotifyWebApiClient(
    HttpClient httpclient,
    SpotifyRateLimiter spotifyRateLimiter,
    SpotifyWebApiClientTokenClient spotifyWebApiClientTokenClient) : ISpotifyWebApiClient
{
    private readonly HttpClient _httpClient = httpclient;
    private readonly SlidingWindowRateLimiter _rateLimiter = spotifyRateLimiter.GetSlidingWindowRateLimiter();
    private const string ApiUri = "https://api.spotify.com/v1";
    private readonly SpotifyWebApiClientTokenClient _tokenClient = spotifyWebApiClientTokenClient;

    private async Task<string> GetSpotifyRequestAsync(string requestUri)
    {
        // limiter before token
        using var lease = await _rateLimiter.AcquireAsync(1);
        var token = await _tokenClient.GetTokenAsync();

        var request = new HttpRequestMessage(HttpMethod.Get,
            requestUri);
        request.Headers.Add("Authorization", $"{token.Token_type} {token.Access_token}");
        var response = await _httpClient.SendAsync(request);

        switch (response.StatusCode)
        {
            case HttpStatusCode.OK:
                {
                    using StreamReader reader = new(await response.Content.ReadAsStreamAsync());
                    return await reader.ReadToEndAsync();
                }
            case HttpStatusCode.BadRequest:
                {
                    throw new ArgumentException($"invalid Uri {requestUri}",nameof(requestUri));
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
