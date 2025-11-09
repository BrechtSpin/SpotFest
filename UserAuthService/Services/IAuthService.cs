using UserAuthService.DTO;

namespace UserAuthService.Services;

public interface IAuthService
{
    Task<(bool, string)> Register(RegisterDTO registerDTO);
    Task<bool> LoginAttempt(LoginDTO loginDTO);
}
