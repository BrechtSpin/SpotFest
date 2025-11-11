using System.Security.Claims;
using UserAuthService.DTO;
using UserAuthService.Services;

namespace UserAuthService.EndPoints;

public static class AuthApi
{
    public static void MapAuthApiEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/register", Register);
        group.MapPost("/login", Login);
        group.MapPost("/logout", Logout)
            .RequireAuthorization();
        group.MapGet("/me", GetCurrentUser)
            .RequireAuthorization();

    }

    private static async Task<IResult> Register(
        IAuthService authService,
        RegisterDTO registerDTO)
    {
        var result = await authService.Register(registerDTO);
        if (result.Item1) return Results.Ok();
        return Results.BadRequest(result.Item2);
    }

    private static async Task<IResult> Login(
        IAuthService authService,
        HttpResponse httpResponse,
        LoginDTO loginDTO
        )
    {
        var result = await authService.LoginAttempt(loginDTO);
        if(!result.authResponseDto.Success) return Results.BadRequest(result.authResponseDto);

        httpResponse.Cookies.Append("SpotFestUser", result.token!, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(24)
        });
        return Results.Ok(result.authResponseDto);
    }

    private static async Task<IResult> Logout(
        HttpResponse httpResponse
        )
    {
        httpResponse.Cookies.Delete("SpotFestUser");
        return Results.Ok();
    }

    private static async Task<IResult> GetCurrentUser(
        IAuthService authService,
        HttpContext httpContext
        )
    {
        var result = await authService.GetCurrentUser(httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        
        if(result.Success) return Results.Ok(result);

        httpContext.Response.Cookies.Delete("SpotFestUser");
        return Results.Unauthorized();
    }
}
