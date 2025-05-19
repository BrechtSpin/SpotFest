using Contracts;
using Contracts.Utils;
using HappeningService.Data.Repositories;
using HappeningService.DTO;
using HappeningService.Messaging;
using HappeningService.Models;
using Microsoft.EntityFrameworkCore;

namespace HappeningService.Services;

public class HappeningServices(HappeningContext happeningContext,
    IPublisherService publisherService) : IHappeningServices
{
    private readonly HappeningContext _happeningContext = happeningContext;
    private readonly IPublisherService _publisherService = publisherService;

    public async Task<HappeningWithArtistSummaries?> GetHappeningFullNoSlugAsync()
    {
        Happening? happening = null;
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        happening = await _happeningContext.Happenings
            .Include(h => h.HappeningArtists)
            .Where(h => today >= h.StartDate)
            .OrderBy(h => h.StartDate)
            .FirstOrDefaultAsync();

        if(happening == null) { happening = await _happeningContext.Happenings
            .Include(h => h.HappeningArtists)
            .OrderBy(h => h.StartDate)
            .FirstOrDefaultAsync();}
        if(happening == null) return null;

        return await GetHappeningFullByHappening(happening);
    }
    public async Task<HappeningWithArtistSummaries?> GetHappeningFullAsync(string slug)
    {
        var happening = await _happeningContext.Happenings
            .Include(h => h.HappeningArtists)
            .Where(h => h.Slug == slug)
            .FirstOrDefaultAsync();

        if (happening is null) return null;

        return await GetHappeningFullByHappening(happening);
    }
    private async Task<HappeningWithArtistSummaries?> GetHappeningFullByHappening(Happening happening)
    {
        var artistSums = await _publisherService.GetArtistsSummaryRPCAsync(
            happening.HappeningArtists
                     .Select(ha => ha.ArtistGuid)
                     .ToArray());
        return new HappeningWithArtistSummaries
        {
            Name = happening.Name,
            Slug = happening.Slug,
            StartDate = happening.StartDate,
            EndDate = happening.EndDate,
            ArtistSummaries = artistSums!
        };
    }
    public async Task<string> CreateHappeningAsync(CreateHappeningDTO createHappeningDTO)
    {
        Happening happening = new()
        {
            Name = createHappeningDTO.Name,
            Slug = $"{StringExtensions.ToKebabCase(createHappeningDTO.Name)}-{createHappeningDTO.StartDate.Year}",
            StartDate = createHappeningDTO.StartDate,
            EndDate = (DateOnly)(createHappeningDTO.EndDate is null || createHappeningDTO.EndDate < createHappeningDTO.StartDate
                        ? createHappeningDTO.StartDate : createHappeningDTO.EndDate)
        };

        _happeningContext.Happenings.Add(happening);
        await _happeningContext.SaveChangesAsync();

        foreach (HappeningArtistIncompleteDTO hapArt in createHappeningDTO.HappeningArtists)
        {
            await _publisherService.HappeningArtistIncompletePublisher(new HappeningArtistIncomplete
            {
                HappeningGuid = happening.Guid,
                SpotifyId = hapArt.Spotifyid ?? "",
                Name = hapArt.Name
            });
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
}