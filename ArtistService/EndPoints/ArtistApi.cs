using ArtistService.Services;
using ArtistService.Messaging;
using ArtistService.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace ArtistService.EndPoints;

public static class ArtistApi
{
    public static void MapArtistApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/artist");

        group.MapGet("/s", GetArtistsSearch);
        group.MapGet("/{guid}/{name?}", GetArtistWithMetrics)
            .WithName("GetArtist"); ;
    }

    private static async Task<IResult> GetArtistsSearch(
       [FromServices] IArtistServices services,
       [FromQuery(Name = "query")] string query,
       [FromQuery(Name = "index")] int index)
    {
        var artists = await services.GetArtistsSearch(query, index);
        return Results.Ok(artists);
    }
    private static async Task<IResult> GetArtistWithMetrics(
        IArtistServices services,
        Guid guid,
        string? name)
    {
        var artistWithMetrics = await services.GetArtistWithMetrics(guid);
        if (artistWithMetrics == null) return Results.NotFound();

        if (!string.Equals(artistWithMetrics.Name, name, StringComparison.OrdinalIgnoreCase))
        {
            return Results.RedirectToRoute(
                routeName: "GetArtist",
                routeValues: new
                {
                    guid,
                    name = artistWithMetrics.Name
                });
        }
        return Results.Ok(artistWithMetrics);
    }
}
