using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Tree;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
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
    }

    private static async Task<IResult> Register(
        IAuthService authService,
        RegisterDTO registerDTO)
    {
        var result = await authService.Register(registerDTO);
        if (result.Item1) return Results.Ok();
        return Results.Problem(result.Item2);
    }

    private static async Task<IResult> Login(
        IAuthService authService,
        LoginDTO loginDTO
        )
    {
        if (await authService.LoginAttempt(loginDTO))
        {
            return Results.Ok();
        }
        else
        {
            return Results.Unauthorized();
        }

    }
}
