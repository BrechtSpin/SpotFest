using ArtistService.Services;
using ArtistService.Messaging;
using ArtistService.Models;

namespace ArtistService.EndPoints;

public static class ArtistApi
{
    public static void MapHappeningApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/artist");
    }
}
