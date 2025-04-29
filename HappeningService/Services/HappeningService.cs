using Contracts;
using Contracts.Utils;
using HappeningService.Data.Repositories;
using HappeningService.DTO;
using HappeningService.Messaging;
using HappeningService.Models;
using Microsoft.EntityFrameworkCore;

namespace HappeningService.Services;

public class HappeningServices(HappeningContext happeningContext, IPublisherService publisherService) : IHappeningService
{
    private readonly HappeningContext _happeningContext = happeningContext;
    private readonly IPublisherService _publisherService = publisherService;


    public async Task<string> CreateHappeningAsync(CreateHappeningDTO createHappeningDTO)
    {
        Happening happening = new()
        {
            Name = createHappeningDTO.Name,
            Slug = $"{StringExtensions.ToKebabCase(createHappeningDTO.Name)}-{createHappeningDTO.StartDate.Year}",
            StartDate = createHappeningDTO.StartDate,
            EndDate = createHappeningDTO.EndDate ?? createHappeningDTO.StartDate
        };
        _happeningContext.Happenings.Add(happening);
        await _happeningContext.SaveChangesAsync();

        foreach (HappeningArtistIncompleteDTO hapArt in createHappeningDTO.HappeningArtists)
        {
            await _publisherService.IncompleteHappeningArtistPublisher(happening.Guid, hapArt.Spotifyid ?? "", hapArt.Name);
        }
        return happening.Slug;
    }
    public async Task CreateHappeningArtistAsync(HappeningArtistComplete happeningArtist)
    {
        HappeningArtist NewHA = new()
        {
            Guid = Guid.NewGuid(),
            HappeningGuid = happeningArtist.HappeningGuid,
            ArtistGuid = happeningArtist.ArtistGuid,
        };

        _happeningContext.HappeningArtists.Add(NewHA);
        await _happeningContext.SaveChangesAsync();
    }

    public async Task<List<HappeningSummaryDTO>> GetCurrentAndUpcomingHappeningAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var CurrentHappenings = _happeningContext.Happenings
            .Where(x => x.StartDate <= today && x.EndDate >= today)
            .OrderBy(x => x.StartDate);

        var UpcomingHappenings = _happeningContext.Happenings
            .Where(x => x.StartDate > today)
            .OrderBy(x => x.StartDate)
            .Take(5);

        return await (CurrentHappenings.Concat(UpcomingHappenings))
            .Select(h => new HappeningSummaryDTO
            {
                Name = h.Name,
                Slug = h.Slug,
                StartDate = h.StartDate,
                EndDate = h.EndDate
            }).ToListAsync();
    }

}
