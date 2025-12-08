using Contracts;

namespace LoggerService.Models;

public record ChangeLog
{
    public long Id { get; set; }
    public required string EntityType { get; set; }
    public required string EntityId { get; set; }
    public required OperationType Operation { get; set; }
    public required Guid ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; }
    public string? CorrelationId { get; set; }
    public List<PropertyChange> PropertyChanges { get; set; } = new();
}

public class PropertyChange
{
    public long Id { get; set; }
    public long ChangeLogId { get; set; }
    public required string PropertyName { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public ChangeLog? ChangeLog { get; set; }
}
