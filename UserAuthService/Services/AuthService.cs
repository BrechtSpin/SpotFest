using Microsoft.AspNetCore.Identity;
using UserAuthService.DTO;
using UserAuthService.Models;

namespace UserAuthService.Services;

public class AuthService(UserManager<SpotFestUser> userManager) : IAuthService
{
    private UserManager<SpotFestUser> _userManager = userManager;
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

    public Task<bool> LoginAttempt(LoginDTO loginDTO)
    {
        throw new NotImplementedException();
    }

}
