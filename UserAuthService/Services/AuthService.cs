using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserAuthService.DTO;
using UserAuthService.Models;

namespace UserAuthService.Services;

public class AuthService(
    UserManager<SpotFestUser> userManager,
    SignInManager<SpotFestUser> signInManager,
    IConfiguration configuration) : IAuthService
{
    private readonly UserManager<SpotFestUser> _userManager = userManager;
    private readonly SignInManager<SpotFestUser> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;
    public async Task<(bool, string)> Register(RegisterDTO registerDTO)
    {
        SpotFestUser newSpotFestUser = new()
        {
            UserName = registerDTO.Email,
            Email = registerDTO.Email,
            FirstName = registerDTO.FirstName,
            LastName = registerDTO.LastName,
            CreatedAt = DateTime.UtcNow,
        };

        var result = await _userManager.CreateAsync(newSpotFestUser, registerDTO.Password);

        if (result.Succeeded) return (true, "Registration has completed successfully");
        var errorCode = result.Errors.First().Code;
        string errorResponse;
        switch (errorCode)
        {
            case "DuplicateEmail":
            case "DuplicateUserName":
                errorResponse = "The email has already been registered";
                break;
            default:
                errorResponse = "Registration failed try again later";
                break;
        }
        return (false, errorResponse);
    }

    public async Task<(AuthResponseDto authResponseDto, string? token)> LoginAttempt(LoginDTO loginDTO)
    {
        var user = await _userManager.FindByEmailAsync(loginDTO.Email);
        var authResponse = new AuthResponseDto { Email = loginDTO.Email };
        var failureMessage = "Invalid email or password";

        if (user is null)
        {
            authResponse.Error = failureMessage;
        }
        else
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (result.Succeeded)
            {
                authResponse.Success = true;
                return (authResponse, GenerateJwtToken(user));
            }
            else
            {
                authResponse.Error = failureMessage;
            }
        }
        return (authResponse, null);
    }

    private string GenerateJwtToken(SpotFestUser user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var jwtIssuer = _configuration["JWT:ISSUER"];
        var jwtAudience = _configuration["JWT:AUDIENCE"];
        var jwtKey = _configuration["JWT:KEY"];

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<AuthResponseDto> GetCurrentUser(string? userId)
    {
        var authResponse = new AuthResponseDto();
        if (string.IsNullOrWhiteSpace(userId))
        {
            authResponse.Error = "NotFound";
        }
        else
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                authResponse.Error = "NotFound";
            }
            else
            {
                authResponse.Success = true;
                authResponse.Email = user.Email!;
            }
        }
        return authResponse;
    }
}
