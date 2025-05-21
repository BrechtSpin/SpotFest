using InformationService.DTO;
using InformationService.Services;
using MiniValidation;

namespace InformationService.Endpoints;

public static class ContactApi
{
    public static void MapContactApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/info");

        group.MapPost("/contactform", PostContactForm)
            .RequireRateLimiting("GlobalPolicy")
            .RequireRateLimiting("PerIpPolicy");

    }

    public static async Task<IResult> PostContactForm(
        IContactServices service,
        ContactFormDTO dto)
    {
        if (!MiniValidator.TryValidate(dto, out var err))
            return Results.ValidationProblem(err);

        await service.ContactFormReceived(dto);
        return Results.Ok();
    }
}
