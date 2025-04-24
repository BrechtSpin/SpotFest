using Microsoft.AspNetCore.Mvc;
using HappeningService.Models;
using HappeningService.DTO;
using HappeningService.Messaging;
using HappeningService.Repositories;
namespace HappeningService.Controllers;

[ApiController]
[Route("happening")]
public class HappeningController(IPublisherService publisher) : ControllerBase
{
    private readonly IPublisherService _publisher = publisher;

    [HttpPost]
    public async Task<IActionResult> CreateHappening(
        [FromBody] CreateHappeningDTO happeningDTO,
        [FromServices] HappeningContext db)
    {
        Happening happening = new()
        {
            Name = happeningDTO.Name,
            StartTime = happeningDTO.StartDate,
            Slug = $"{StringExtensions.ToKebabCase(happeningDTO.Name)}-{happeningDTO.StartDate.Year}",
            EndTime = happeningDTO.EndDate
        };
        db.Happenings.Add(happening);
        await db.SaveChangesAsync();
        foreach (HappeningArtistIncompleteDTO hapArt in happeningDTO.HappeningArtists)
        {
            await _publisher.IncompleteHappeningArtistPublisher(happening.Guid, hapArt.Spotifyid ?? "", hapArt.Name);
        }
        return Created("${happening.Slug}", happening);
    }
}
