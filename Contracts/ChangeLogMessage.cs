namespace Contracts;

public record ChangeLogMessage
{
    public required string EntityType { get; set; }
    public required string EntityId { get; set; }
    public required OperationType Operation { get; set; }
    public DateTime ChangedAt { get; set; }
    public List<PropertyChangeDto>? PropertyChanges { get; set; }
}

public record PropertyChangeDto
{
    public required string PropertyName { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
}

public enum OperationType
{
    Insert,
    Update,
    Delete
}
