using HappeningService.Data.Repositories;
using HappeningService.DTO;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace HappeningService.Services.Hubs;

public class HappeningsCurrentTimeframeService(
    IHubContext<HappeningsCurrentTimeframeHub> hub,
        IDbContextFactory<HappeningContext> contextFactory,
    IMemoryCache cache) : IHappeningsCurrentTimeframeService
{
    private readonly IHubContext<HappeningsCurrentTimeframeHub> _hub = hub;
    private readonly IDbContextFactory<HappeningContext> _contextFactory = contextFactory;
    private readonly IMemoryCache _cache = cache;
    private const string cacheKey = $"{nameof(HappeningsCurrentTimeframeService)}.lastSnapshot";

    public async Task OnConnectedAsync()
    {
        var current = _cache.Get<List<HappeningSummaryDTO>>(cacheKey);
        if (current == null)
        {
            current = await GetHappeningsCurrentTimeframeAsync();
            _cache.Set(cacheKey, current);
        }
    }
    public List<HappeningSummaryDTO> LoadData()
    {
        return _cache.Get<List<HappeningSummaryDTO>>(cacheKey) ?? new List<HappeningSummaryDTO>();
    }
    public async Task OnChangedDataAsync()
    {
        var last  = _cache.Get<List<HappeningSummaryDTO>>(cacheKey);
        var current = await GetHappeningsCurrentTimeframeAsync();
        
        if (last == null || !Enumerable.SequenceEqual(current, last))
        {
            _cache.Set(cacheKey, current);
            await _hub.Clients.All.SendAsync("HappeningsUpdated", current);
        }
    }

    private async Task<List<HappeningSummaryDTO>> GetHappeningsCurrentTimeframeAsync()
    {
        await using var _happeningContext = _contextFactory.CreateDbContext();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var CurrentHappenings = _happeningContext.Happenings
            .Where(x => x.StartDate <= today && x.EndDate >= today)
            .OrderBy(x => x.StartDate);

        var UpcomingHappenings = _happeningContext.Happenings
            .Where(x => x.StartDate > today)
            .OrderBy(x => x.StartDate)
            .Take(5);

        return CurrentHappenings.Concat(UpcomingHappenings)
            .Select(h => new HappeningSummaryDTO
            {
                Name = h.Name,
                Slug = h.Slug,
                StartDate = h.StartDate,
                EndDate = h.EndDate
            }).ToList();
    }

}
