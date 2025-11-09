using System.ComponentModel.DataAnnotations;

namespace UserAuthService.DTO
{
    public class LoginDTO
    {
        [Required, StringLength(200)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(200, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;
    }
}
