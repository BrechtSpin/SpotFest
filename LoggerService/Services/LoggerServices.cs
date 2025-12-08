using Contracts;
using LoggerService.Data.Repositories;
using LoggerService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading;

namespace LoggerService.Services;

public class LoggerServices(
    LoggerContext loggerContext
    ) : ILoggerServices
{
    private readonly LoggerContext _loggerContext = loggerContext;

    public async Task ChangeLogMessageReceived(ChangeLogMessage message)
    {
        var changeLog = new ChangeLog
        {
            EntityType = message.EntityType,
            EntityId = message.EntityId,
            Operation = message.Operation,
            ChangedBy = message.ChangedBy,
            ChangedAt = message.ChangedAt,
            CorrelationId = message.CorrelationId,
            PropertyChanges = message.PropertyChanges.Select(pc => new PropertyChange
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
        if (Guid.TryParse(guid, out Guid userId))
        {
            return await _loggerContext.ChangeLogs
                .Include(cl => cl.PropertyChanges)
                .Where(cl => cl.ChangedBy == userId)
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
