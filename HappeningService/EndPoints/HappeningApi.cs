using HappeningService.DTO;
using HappeningService.Services;
using HappeningService.Messaging;
using HappeningService.Models;
using System.Reflection.Metadata.Ecma335;
using MassTransit;

namespace HappeningService.EndPoints;

public static class HappeningApi
{
    public static void MapHappeningApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/happening");

        group.MapGet("", GetHappeningFullNoSlug);
        group.MapGet("/{slug}", GetHappeningFull);
        group.MapPost("/new", CreateHappening);
        group.MapGet("/current", GetCurrentAndUpcomingHappening);
    }

    private static async Task<IResult> GetHappeningFullNoSlug(
        IHappeningService service)
    {
        var happeningFull = await service.GetHappeningFullNoSlugAsync();

        if (happeningFull is null) return Results.NotFound();
        return Results.Ok(happeningFull);
    }
    private static async Task<IResult> GetHappeningFull(
        IHappeningService service,
        string slug)
    {
        var happeningFull = await service.GetHappeningFullAsync(slug);

        if (happeningFull is null) return Results.NotFound();
        return Results.Ok(happeningFull);
    }

    private static async Task<IResult> CreateHappening(IHappeningService service, CreateHappeningDTO dto)
    {
        var Slug = await service.CreateHappeningAsync(dto);
        return Results.Redirect($"/happenings/{Slug}");
        //return Results.Created(Slug, null);
    }
    private static async Task<IResult> GetCurrentAndUpcomingHappening(IHappeningService service)
    {
        var happeningsSummary = await service.GetCurrentAndUpcomingHappeningAsync();
        return Results.Ok(happeningsSummary);
    }
}
