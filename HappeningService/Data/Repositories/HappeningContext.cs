using Microsoft.EntityFrameworkCore;
using HappeningService.Models;
using HappeningService.Data.Repositories.Configuration;

namespace HappeningService.Data.Repositories;

public class HappeningContext(DbContextOptions<HappeningContext> options) : DbContext(options)
{
    public DbSet<Happening> Happenings { get; set; }
    public DbSet<HappeningArtist> HappeningArtists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new HappeningConfiguration());
        modelBuilder.ApplyConfiguration(new HappeningArtistConfiguration());
    }
}