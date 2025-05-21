using System.ComponentModel.DataAnnotations;

namespace InformationService.DTO;

public record ContactFormDTO
{
    [Required]
    [StringLength(100,MinimumLength = 1)]
    public required string Name { get; init; }
    [Required]
    [StringLength(100, MinimumLength = 5)]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public required string Email { get; init; }
}
