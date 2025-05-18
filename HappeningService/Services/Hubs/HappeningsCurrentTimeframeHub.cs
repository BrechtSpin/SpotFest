using HappeningService.DTO;
using Microsoft.AspNetCore.SignalR;

namespace HappeningService.Services.Hubs;

public class HappeningsCurrentTimeframeHub(
    IHappeningsCurrentTimeframeService currentTimeframeService) : Hub
{
    private readonly IHappeningsCurrentTimeframeService _currentTimeframeService = currentTimeframeService;

    public override async Task OnConnectedAsync()
    {
        await _currentTimeframeService.OnConnectedAsync();
        await base.OnConnectedAsync();
    }

    public List<HappeningSummaryDTO> LoadData()
    {
        return _currentTimeframeService.LoadData();
    }
}
