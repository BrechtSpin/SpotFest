using HappeningService.DTO;

namespace HappeningService.Services.Hubs
{
    public interface IHappeningsCurrentTimeframeService
    {
        Task OnConnectedAsync();
        Task OnChangedDataAsync();
        List<HappeningSummaryDTO> LoadData();
    }
}
