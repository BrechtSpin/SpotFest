using ArtistService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtistService.Repositories.Configurations
{
    public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder.HasMany(s => s.Metrics)
                   .WithOne(m => m.Artist)
                   .HasForeignKey(m => m.ArtistGuid)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasKey(a => a.Guid);
            builder.Property(a => a.Guid).IsRequired();

            builder.HasIndex(a => a.SpotifyId).IsUnique();
            builder.Property(a => a.SpotifyId)
                   .HasMaxLength(24);

            builder.Property(a => a.Name)
                   .HasMaxLength(255);
        }
    }
}
