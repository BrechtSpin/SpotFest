namespace Contracts;

public record ArtistFetchSummariesResponse
{
    public Guid CorrelationId { get; init; }
    public ArtistSummary[]? ArtistSummaries { get; init; }
}
