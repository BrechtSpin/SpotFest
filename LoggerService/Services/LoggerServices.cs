using Contracts;
using LoggerService.Data.Repositories;
using LoggerService.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace LoggerService.Services;

public class LoggerServices(
    LoggerContext loggerContext
    ) : ILoggerServices
{
    private readonly LoggerContext _loggerContext = loggerContext;

    public async Task ChangeLogMessageReceived(ChangeLogMessage message)
    {
        Guid.TryParse(Activity.Current?.GetBaggageItem(BaggageKeys.UserGuid), out Guid changedBy);
        var changeLog = new ChangeLog
        {
            EntityType = message.EntityType,
            EntityId = message.EntityId,
            Operation = message.Operation,
            ChangedBy = changedBy,
            ChangedAt = message.ChangedAt,
            CorrelationId = Activity.Current?.TraceId.ToString(),
            PropertyChanges = message.PropertyChanges!.Select(pc => new PropertyChange
            {
                PropertyName = pc.PropertyName,
                OldValue = pc.OldValue,
                NewValue = pc.NewValue
            }).ToList()
        };
        _loggerContext.Add(changeLog);
        await _loggerContext.SaveChangesAsync();
    }

    public async Task<List<ChangeLog>> GetChangeLogsByUserAsync(string guid)
    {
        if (Guid.TryParse(guid, out Guid userGuid))
        {
            return await _loggerContext.ChangeLogs
                .Include(cl => cl.PropertyChanges)
                .Where(cl => cl.ChangedBy == userGuid)
                .OrderByDescending(cl => cl.ChangedAt)
                .ToListAsync();
        }
        return new List<ChangeLog>();
    }
    public async Task<List<ChangeLog>> GetChangeLogsByEntityAsync(
        string entityType,
        string entityId)
    {
        return await _loggerContext.ChangeLogs
            .Include(cl => cl.PropertyChanges)
            .Where(cl => cl.EntityType == entityType && cl.EntityId == entityId)
            .OrderByDescending(cl => cl.ChangedAt)
            .ToListAsync();
    }
}
