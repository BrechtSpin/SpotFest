namespace UserAuthService.DTO
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public DateTime Expiration { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
