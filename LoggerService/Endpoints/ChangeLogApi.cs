using LoggerService.Models;
using LoggerService.Services;

namespace LoggerService.Endpoints;

public static class ChangeLogEndpoints
{
    public static void MapChangeLogEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/changelogs");

        group.MapGet("/user/{guid}", GetChangeLogsByUser)
            .Produces<List<ChangeLog>>();
        group.MapGet("/entity/{entityType}/{entityId}", GetChangeLogsByEntity)
            .Produces<List<ChangeLog>>();
    }

    private static async Task<IResult> GetChangeLogsByUser(
        ILoggerServices service,
        string guid)
    {
        var changeLogs = await service.GetChangeLogsByUserAsync(guid);
        return Results.Ok(changeLogs);
    }

    private static async Task<IResult> GetChangeLogsByEntity(
        ILoggerServices service,
        string entityType,
        string entityId)
    {
        var changeLogs = await service.GetChangeLogsByEntityAsync(
            entityType,
            entityId);
        return Results.Ok(changeLogs);
    }
}

