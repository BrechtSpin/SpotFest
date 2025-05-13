using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskScheduler.Messaging;
using Contracts;
using TaskScheduler.Models;
using TaskScheduler.Data.Repositories;

namespace TaskScheduler;

public class TaskSchedulerWorker(
    ILogger<TaskSchedulerWorker> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    private static readonly string _projectName = Assembly.GetExecutingAssembly().GetName().Name!;
    private readonly ILogger<TaskSchedulerWorker> _logger = logger;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "{time}: {projectName} worker has started.",
            DateTimeOffset.UtcNow,
        _projectName);

        using var scope = _scopeFactory.CreateScope();
        {
            var schedulerContext = scope.ServiceProvider.GetService<TaskSchedulerContext>();
            await schedulerContext!.Database.MigrateAsync(CancellationToken.None);
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            await CreateTask(now);

            var delay = now.Date.AddDays(1).AddHours(0) - now;  // Run at 00:00 UTC
            _logger.LogInformation(
                "{time}: {_projectName} sleeping for {delay} hours...",
                DateTimeOffset.UtcNow,
                _projectName,
                delay.TotalHours.ToString()
                );
            await Task.Delay(delay, stoppingToken);
        }
        _logger.LogInformation(
            "{time}: {projectName} worker has stopped."
            , DateTimeOffset.UtcNow
            , _projectName);
    }
    private async Task CreateTask(DateTime now)
    {
        using var scope = _scopeFactory.CreateScope();
        var schedulerContext = scope.ServiceProvider.GetService<TaskSchedulerContext>();
        var publisherService = scope.ServiceProvider.GetService<IPublisherService>();

        var PreviousTaskDate  = schedulerContext!.SchedulerLogs
            .OrderByDescending(l => l.TaskDate)
           .FirstOrDefault();

        if (PreviousTaskDate is null || PreviousTaskDate.TaskDate.Date < now.Date)
        {
            var schedulertask = new SchedulerTask { TaskDateRequest = now, Type = "artistmetric", Mode = "all" };
            await publisherService!.ArtistMetricDataTaskPublisher(schedulertask);

            schedulerContext.SchedulerLogs.Add(new TaskSchedulerLog { TaskDate = DateTime.UtcNow });
            await schedulerContext.SaveChangesAsync();

            _logger.LogInformation("{time}: {projectName} created a task",
                now,
                _projectName);
        }
    }
}
