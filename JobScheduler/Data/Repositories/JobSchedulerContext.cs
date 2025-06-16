using Microsoft.EntityFrameworkCore;
using JobScheduler.Models;
using System.Diagnostics.CodeAnalysis;
namespace JobScheduler.Data.Repositories;

public class JobSchedulerContext : DbContext
{
    [SetsRequiredMembers]
    public JobSchedulerContext(DbContextOptions<JobSchedulerContext> options) : base(options)
    {
        SchedulerLogs = Set<JobSchedulerLog>();
    }

    public required DbSet<JobSchedulerLog> SchedulerLogs { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobSchedulerLog>().HasKey(x => x.Guid);
        modelBuilder.Entity<JobSchedulerLog>().HasIndex(x => x.JobDate);
    }
}
