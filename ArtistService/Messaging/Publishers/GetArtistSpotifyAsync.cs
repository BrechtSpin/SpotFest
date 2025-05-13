using Contracts;
using MassTransit;
using System.Diagnostics;
using System.Text.Json;

namespace ArtistService.Messaging;

public partial class PublisherService
{
    public async Task<ArtistSpotifyResponse> GetArtistSpotifyAsync(ArtistSpotifyRequest request)
    {
        try
        {
            var response = await _artistSpotifyRequestClient.GetResponse<ArtistSpotifyResponse>(request);
            return response.Message;
        }
        catch (RequestFaultException ex)
        {
            _logger.LogError("{timestamp}: Failed to resolve ArtistSpotifyRequest: \n{request}",
                ex.Fault!.Timestamp.ToLongDateString(),
                JsonSerializer.Serialize<ArtistSpotifyRequest>(request));
        }
        catch (Exception ex)
        {
            _logger.LogError("{message}:",
                ex.Message);
            Debugger.Break();
        }
        return new ArtistSpotifyResponse { Name = "", SpotifyId = "", PictureSmallUrl = "", PictureMediumUrl = "" };
    }
}