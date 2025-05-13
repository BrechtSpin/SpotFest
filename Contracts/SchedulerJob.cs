namespace Contracts;

public record SchedulerJob
{
    public DateTime JobDate { get; init; }
    public required string Type { get; init; }
    public required string Mode { get; init; }
}
