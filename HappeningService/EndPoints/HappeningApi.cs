using HappeningService.DTO;
using HappeningService.Services;
using HappeningService.Services.Hubs;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HappeningService.EndPoints;

public static class HappeningApi
{
    public static void MapHappeningApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/happening");

        group.MapPost("/new", CreateHappening);
        group.MapGet("", GetHappeningFullNoSlug);
        group.MapGet("/s", GetHappeningsSearch);
        group.MapGet("/{slug}", GetHappeningFull);
        group.MapGet("/artist/{guid}", GetHappeningsOfArtist);
        //group.MapGet("/current", GetHappeningsCurrentTimeframe);
        group.MapHub<HappeningsCurrentTimeframeHub>("/currenthub");
    }

    private static async Task<IResult> CreateHappening(
        IHappeningServices service,
        IHappeningsCurrentTimeframeService currentTimeframeService,
        CreateHappeningDTO dto)
    {
        var Slug = await service.CreateHappeningAsync(dto);
        await currentTimeframeService.OnChangedDataAsync();
        return Results.Created($"/happening/{Slug}", new { slug = Slug });
    }
    private static async Task<IResult> GetHappeningFullNoSlug(
        IHappeningServices service)
    {
        var happeningFull = await service.GetHappeningFullNoSlugAsync();

        if (happeningFull is null) return Results.NotFound();
        return Results.Ok(happeningFull);
    }
    private static async Task<IResult> GetHappeningFull(
        IHappeningServices service,
        string slug)
    {
        var happeningFull = await service.GetHappeningFullAsync(slug);

        if (happeningFull is null) return Results.NotFound();
        return Results.Ok(happeningFull);
    }
    private static async Task<IResult> GetHappeningsOfArtist(
        IHappeningServices service,
        string guid)
    {
        var happeningsOfArtist = await service.GetHappeningsOfArtistAsync(guid);
        return Results.Ok(happeningsOfArtist);
    }
    private static async Task<IResult> GetHappeningsSearch(
        [FromServices] IHappeningServices services,
       [FromQuery(Name = "year")] int year,
       [FromQuery(Name = "month")] int month,
       [FromQuery(Name = "index")] int index)
    {
        var happenings = await services.GetHappeningsSearch(year, month, index);
        return Results.Ok(happenings);
    }
}
