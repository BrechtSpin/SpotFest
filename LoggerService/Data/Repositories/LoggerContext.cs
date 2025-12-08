using Microsoft.EntityFrameworkCore;
using LoggerService.Models;
using LoggerService.Data.Repositories.Configuration;


namespace LoggerService.Data.Repositories;

public class LoggerContext(DbContextOptions<LoggerContext> options) : DbContext(options)
{
    public DbSet<ChangeLog> ChangeLogs { get; set; }
    public DbSet<PropertyChange> PropertyChanges { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ChangeLogConfiguration());
        modelBuilder.ApplyConfiguration(new PropertyChangeConfiguration());
    }
}
