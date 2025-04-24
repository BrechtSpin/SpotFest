using DataHarvester.Models;
using MassTransit;
using MassTransit.JobService.Messages;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace DataHarvester.SpotifyWeb;

public class SpotifyWebApiClient : ISpotifyWebApiClient
{
    private readonly HttpClient _httpClient;
    private const string ApiUri = "https://api.spotify.com/v1";
    private SpotifyWebApiClientTokenClient _tokenClient;

    public SpotifyWebApiClient(HttpClient httpclient, SpotifyWebApiClientTokenClient spotifyWebApiClientTokenClient)
    {
        _httpClient = httpclient;
        _tokenClient = spotifyWebApiClientTokenClient;
    }

    private async Task<string> GetSpotifyRequestAsync(string requestUri)
    {
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
            //case HttpStatusCode.BadRequest or HttpStatusCode.NotFound:
            //    {
            //        throw new ArgumentException("invalid SpotifyArtistUUID");
            //    }
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
    public async Task<List<SpotifyArtist>> GetArtistsByNameAsync(string artistName)
    {
        var message = await GetSpotifyRequestAsync(
            $"{ApiUri}/search?q={Uri.EscapeDataString(artistName)}&type=artist&limit=10");
        var artistResponse = JsonSerializer.Deserialize(message, SerializerContext.Default.SpotifyArtistResponse)!;
        return artistResponse?.Artists?.Items ?? [];
    }

}
