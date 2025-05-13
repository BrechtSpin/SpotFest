using ArtistService.Models;
using ArtistService.Data.Repositories.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ArtistService.Data.Repositories;
public class ArtistServiceContext(DbContextOptions<ArtistServiceContext> options) : DbContext(options)
{
    public DbSet<Artist> Artists { get; set; }
    public DbSet<ArtistMetric> ArtistMetrics { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ArtistConfiguration());
        modelBuilder.ApplyConfiguration(new ArtistMetricConfiguration());
    }
}
