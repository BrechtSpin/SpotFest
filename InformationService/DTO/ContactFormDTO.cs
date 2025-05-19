namespace InformationService.DTO;

public record ContactFormDTO
{
    public required string  Name { get; init; }
    public required string Email { get; init; }
}
