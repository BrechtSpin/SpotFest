using Contracts;
using LoggerService.Models;

namespace LoggerService.Services;

public interface ILoggerServices
{
    public Task ChangeLogMessageReceived(ChangeLogMessage message);
    public Task<List<ChangeLog>> GetChangeLogsByUserAsync(string guid);
    public Task<List<ChangeLog>> GetChangeLogsByEntityAsync(string entityType, string entityId);
}
