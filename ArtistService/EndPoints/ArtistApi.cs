using ArtistService.Services;
using ArtistService.Messaging;
using ArtistService.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ArtistService.EndPoints;

public static class ArtistApi
{
    public static void MapArtistApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/artist");

        group.MapGet("/{guid}/{name}", GetArtistWithMetrics);
    }

    public static async Task<IResult> GetArtistWithMetrics(
        IArtistServices services,
        Guid guid,
        string name)
    {
        var artistWithMetrics = await services.GetArtistWithMetrics(guid);
        if (artistWithMetrics == null) return Results.NotFound();

        if(!string.Equals(artistWithMetrics.Name, name, StringComparison.OrdinalIgnoreCase))
        {
            return Results.RedirectToRoute(routeValues: new
            {
                guid,
                name = artistWithMetrics.Name
            });
        }
        return Results.Ok(artistWithMetrics);
    }
}
