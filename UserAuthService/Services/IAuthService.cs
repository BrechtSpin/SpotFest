using UserAuthService.DTO;

namespace UserAuthService.Services;

public interface IAuthService
{
    Task<(bool, string)> Register(RegisterDTO registerDTO);
    Task<(AuthResponseDto authResponseDto, string? token)> LoginAttempt(LoginDTO loginDTO);
}
