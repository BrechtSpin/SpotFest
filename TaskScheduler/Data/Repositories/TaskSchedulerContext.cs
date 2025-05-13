using Microsoft.EntityFrameworkCore;
using TaskScheduler.Models;
namespace TaskScheduler.Data.Repositories;

public class TaskSchedulerContext(DbContextOptions<TaskSchedulerContext> options) : DbContext(options)
{
    public required DbSet<TaskSchedulerLog> SchedulerLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskSchedulerLog>().HasKey(x => x.Guid);
        modelBuilder.Entity<TaskSchedulerLog>().HasIndex(x => x.TaskDate);
    }
}
