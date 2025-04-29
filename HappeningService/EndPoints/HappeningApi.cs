using HappeningService.DTO;
using HappeningService.Services;
using HappeningService.Messaging;
using HappeningService.Models;

namespace HappeningService.EndPoints;

public static class HappeningApi
{
    public static void MapHappeningApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/happening");

        group.MapPost("/new", CreateHappening);
        group.MapGet("/current", GetCurrentAndUpcomingHappening);
    }

    private static async Task<IResult> CreateHappening(IHappeningService service, CreateHappeningDTO dto)
    {
        var Slug = await service.CreateHappeningAsync(dto);
        return Results.Created(Slug, null);
    }
    private static async Task<IResult> GetCurrentAndUpcomingHappening(IHappeningService  service)
    {
        var happeningsSummary = await service.GetCurrentAndUpcomingHappeningAsync();
        return Results.Ok(happeningsSummary);
    }
}
