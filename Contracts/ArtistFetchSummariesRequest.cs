namespace Contracts;

public record ArtistFetchSummariesRequest
{
    public Guid CorrelationId { get; init; }
    public Guid[] ArtistGuids { get; init; } = [];
}
