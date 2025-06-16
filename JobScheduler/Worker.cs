using Microsoft.EntityFrameworkCore;
using System.Reflection;
using JobScheduler.Messaging;
using Contracts;
using JobScheduler.Models;
using JobScheduler.Data.Repositories;

namespace JobScheduler;

public class JobSchedulerWorker(
    ILogger<JobSchedulerWorker> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    private static readonly string _projectName = Assembly.GetExecutingAssembly().GetName().Name!;
    private readonly ILogger<JobSchedulerWorker> _logger = logger;
    private readonly IServiceScopeFactory _scopeFactory = scopeFactory;


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(
            "{time}: {projectName} worker has started.",
            DateTimeOffset.UtcNow,
        _projectName);

        using var scope = _scopeFactory.CreateScope();
        {
            var schedulerContext = scope.ServiceProvider.GetService<JobSchedulerContext>();
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
    public async Task CreateTask(DateTime now)
    {
        using var scope = _scopeFactory.CreateScope();
        var schedulerContext = scope.ServiceProvider.GetService<JobSchedulerContext>();
        var publisherService = scope.ServiceProvider.GetService<IPublisherService>();

        var LastTaskDate  = schedulerContext!.SchedulerLogs
            .OrderByDescending(l => l.JobDate)
           .FirstOrDefault();

        if (LastTaskDate is null || LastTaskDate.JobDate.Date < now.Date)
        {
            var schedulerJob = new SchedulerJob { JobDate = now, Type = "artistmetric", Mode = "all" };
            await publisherService!.ArtistMetricDataTaskPublisher(schedulerJob);

            schedulerContext.SchedulerLogs.Add(new JobSchedulerLog { JobDate = DateTime.UtcNow });
            await schedulerContext.SaveChangesAsync();

            _logger.LogInformation("{time}: {projectName} created a task",
                now,
                _projectName);
        }
    }
}
