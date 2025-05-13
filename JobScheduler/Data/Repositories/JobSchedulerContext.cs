using Microsoft.EntityFrameworkCore;
using JobScheduler.Models;
namespace JobScheduler.Data.Repositories;

public class JobSchedulerContext(DbContextOptions<JobSchedulerContext> options) : DbContext(options)
{
    public required DbSet<JobSchedulerLog> SchedulerLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobSchedulerLog>().HasKey(x => x.Guid);
        modelBuilder.Entity<JobSchedulerLog>().HasIndex(x => x.JobDate);
    }
}
