using DataHarvester.Models;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DataHarvester.SpotifyWeb;

public class SpotifyWebApiClientTokenClient
{

    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;
    private const string AuthUri = "https://accounts.spotify.com/api/token/";
    private readonly HttpContent _content;
    private SpotifyWebApiClientToken? Token;
    private readonly SemaphoreSlim _semaphore = new(1, 1); // 1 tokenrequest to server at a time

    public SpotifyWebApiClientTokenClient(ILogger<SpotifyWebApiClientTokenClient> logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://accounts.spotify.com/api/token/");
        _content = AuthBody();
    }

    public async Task<SpotifyWebApiClientToken> GetTokenAsync()
    {
        await _semaphore.WaitAsync();   // only one request to get a new token should be sent
        if (Token == null || DateTime.UtcNow >= Token.Expires_at)
        {
            HttpResponseMessage response = await _httpClient.PostAsync(AuthUri, _content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                using StreamReader reader = new(await response.Content.ReadAsStreamAsync());
                var message = await reader.ReadToEndAsync();
                Token = JsonSerializer.Deserialize(message, SerializerContext.Default.SpotifyWebApiClientToken)!;
                Token.Expires_at = DateTime.UtcNow + TimeSpan.FromSeconds(Token.Expires_in - 30);
            }
            else
            {
                //TODO other status-codes see documentation
                _logger.LogCritical("");
                throw new NotImplementedException();
            }
        }
        _semaphore.Release();
        return Token;
    }

    private StringContent AuthBody()
    {
        var clientId = Environment.GetEnvironmentVariable("SpotifyAPIClientId");
        var clientSecret = Environment.GetEnvironmentVariable("SpotifyAPIClientSecret");
        if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
        {
            _logger.LogCritical("SpotifyAPIClientId and SpotifyAPIClientSecret environment variables need to be set, see the spotify web-api on how to obtain these");
            Environment.Exit(1);
        }
        return new StringContent(
            $"grant_type=client_credentials&client_id={clientId}&client_secret={clientSecret}",
            Encoding.UTF8,
            "application/x-www-form-urlencoded"
        );
    }
}
