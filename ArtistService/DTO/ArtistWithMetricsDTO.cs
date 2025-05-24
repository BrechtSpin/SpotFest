namespace ArtistService.DTO;

public class ArtistWithMetricsDTO
{
    public required Guid ArtistGuid { get; set; }
    public required string Name { get; set; }
    public required string PictureMediumUrl { get; set; }
    public required ArtistMetricDTO[] ArtistMetrics { get; set; } = [];
}
