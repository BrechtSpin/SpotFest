using ArtistService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtistService.Repositories.Configurations
{
    public class ArtistMetricConfiguration : IEntityTypeConfiguration<ArtistMetric>
    {
        public void Configure(EntityTypeBuilder<ArtistMetric> builder)
        {
            builder.HasKey(m => m.Guid);
            builder.Property(m => m.Guid).IsRequired();

            builder.HasOne(m => m.Artist)
                   .WithMany(a => a.Metrics)
                   .HasForeignKey(m => m.ArtistGuid)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Property(m => m.ArtistGuid).IsRequired();

            builder.HasIndex(m => new { m.ArtistGuid, m.Date }) 
                   .IsUnique();
            builder.Property(m => m.Date)
                   .HasColumnType("datetime")
                   .IsRequired();


        }
    }
}
