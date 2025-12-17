using Contracts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using HappeningService.Messaging;

namespace HappeningService.Data.Interceptors;

public class ChangeLogInterceptor(
    IServiceProvider serviceProvider
    ) : SaveChangesInterceptor
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private readonly List<ChangeLogMessage> changeLogs = [];

    //pre DB-Saving
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        CaptureChanges(eventData.Context);
        return base.SavingChanges(eventData, result);
    }
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        CaptureChanges(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void CaptureChanges(
        DbContext? context)
    {
        if (context is not null)
        {
            changeLogs.Clear();

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added ||
                    entry.State == EntityState.Modified ||
                    entry.State == EntityState.Deleted)
                {
                    var changeLog = CreateChangeLog(entry);
                    if (changeLog != null)
                    {
                        changeLogs.Add(changeLog);
                    }
                }
            }
        }
    }

    private static ChangeLogMessage? CreateChangeLog(
        EntityEntry entry)
    {
        var entityId = GetEntityId(entry);
        if (entityId == null) return null;


        var operation = entry.State switch
        {
            EntityState.Added => OperationType.Insert,
            EntityState.Modified => OperationType.Update,
            EntityState.Deleted => OperationType.Delete,
            _ => (OperationType?)null
        };
        if (operation == null) return null;

        var propertyChanges = GetPropertyChanges(entry, operation.Value);

        return new ChangeLogMessage
        {
            EntityType = entry.Entity.GetType().Name,
            EntityId = entityId,
            Operation = operation.Value,
            ChangedAt = DateTime.UtcNow,
            PropertyChanges = propertyChanges
        };
    }

    private static List<PropertyChangeDto>? GetPropertyChanges(
        EntityEntry entry,
        OperationType operation)
    {
        return operation switch
        {
            OperationType.Insert => entry.Properties
                .Select(p => new PropertyChangeDto
                {
                    PropertyName = p.Metadata.Name,
                    OldValue = null,
                    NewValue = SerializeValue(p.CurrentValue)
                })
                .ToList(),

            OperationType.Update => entry.Properties
                .Where(p => p.IsModified)
                .Select(p => new PropertyChangeDto
                {
                    PropertyName = p.Metadata.Name,
                    OldValue = SerializeValue(p.OriginalValue),
                    NewValue = SerializeValue(p.CurrentValue)
                })
                .ToList(),

            OperationType.Delete => entry.Properties
                .Select(p => new PropertyChangeDto
                {
                    PropertyName = p.Metadata.Name,
                    OldValue = SerializeValue(p.OriginalValue),
                    NewValue = null
                })
                .ToList(),

            _ => null
        };
    }

    private static string? GetEntityId(EntityEntry entry)
    {
        var keyProperties = entry.Properties
            .Where(p => p.Metadata.IsPrimaryKey())
            .ToList();

        if (keyProperties.Count == 0) return null;

        if (keyProperties.Count == 1)
        {
            return keyProperties[0].CurrentValue?.ToString();
        }

        //composite keys
        var keyValues = keyProperties
            .Select(p => p.CurrentValue?.ToString() ?? "null");
        return string.Join("|", keyValues);
    }

    private static string? SerializeValue(object? value)
    {
        if (value == null) return null;

        // Handle common types directly
        return value switch
        {
            string s => s,
            DateTime dt => dt.ToString("O"), // ISO 8601 format
            DateTimeOffset dto => dto.ToString("O"),
            _ => JsonSerializer.Serialize(value)
        };
    }

    // post DB-saving
    public override int SavedChanges(
        SaveChangesCompletedEventData eventData,
        int result)
    {
        PublishChangesAsync(eventData.Context, CancellationToken.None)
            .GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        await PublishChangesAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }
    private async Task PublishChangesAsync(
       DbContext? context,
       CancellationToken cancellationToken)
    {
        if (context == null) return;

        if (changeLogs != null && changeLogs.Count != 0)
        {
            using var scope = _serviceProvider.CreateScope();
            var publisherService = scope.ServiceProvider.GetRequiredService<IPublisherService>();
            foreach (var changeLog in changeLogs)
            {
                await publisherService.ChangeLogMessagePublisher(changeLog);
            }
            changeLogs.Clear();
        }
    }
}