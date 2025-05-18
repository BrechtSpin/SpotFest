using System.ComponentModel.DataAnnotations;

namespace HappeningService.DTO;

public record CreateHappeningDTO : IValidatableObject
{
    [Required, StringLength(200, MinimumLength = 2)]
    public required string Name { get; init; }
    [Required]
    public required DateOnly StartDate { get; init; }
    public DateOnly? EndDate { get; init; }

    public List<HappeningArtistIncompleteDTO> HappeningArtists { get; init; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext ctx)
    {
        if (EndDate.HasValue && EndDate.Value < StartDate)
        {
            yield return new ValidationResult(
                "EndDate cannot be before StartDate.",
                new[] { nameof(EndDate), nameof(StartDate) }
            );
        }
    }
}
