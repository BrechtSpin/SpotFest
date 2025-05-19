using InformationService.DTO;
using InformationService.Services;

namespace InformationService.Endpoints;

public static class ContactApi
{
    public static void MapArtistApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/info");

        group.MapPost("contactform", PostContactForm);

    }

    private static async Task<IResult> PostContactForm(
        IContactServices service,
        ContactFormDTO dto)
    {
        await service.ContactFormReceived(dto);
        return Results.Ok();
    }
}
