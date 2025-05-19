using HappeningService.DTO;
using HappeningService.Services;
using HappeningService.Services.Hubs;

namespace HappeningService.EndPoints;

public static class HappeningApi
{
    public static void MapHappeningApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/happening");

        group.MapGet("", GetHappeningFullNoSlug);
        group.MapGet("/{slug}", GetHappeningFull);
        group.MapPost("/new", CreateHappening);
        //group.MapGet("/current", GetHappeningsCurrentTimeframe);
        group.MapHub<HappeningsCurrentTimeframeHub>("/currenthub");
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

    private static async Task<IResult> CreateHappening(
        IHappeningServices service,
        IHappeningsCurrentTimeframeService currentTimeframeService,
        CreateHappeningDTO dto)
    {
        var Slug = await service.CreateHappeningAsync(dto);
        await currentTimeframeService.OnChangedDataAsync();
        return Results.Created($"/happening/{Slug}", new { slug = Slug });
    }
    // deprecated
    //private static async Task<IResult> GetHappeningsCurrentTimeframe(IHappeningServices service)
    //{
    //    var happeningsSummary = await service.GetHappeningsCurrentTimeframeAsync();
    //    return Results.Ok(happeningsSummary);
    //}
}
