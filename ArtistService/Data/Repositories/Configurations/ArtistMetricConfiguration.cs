using ArtistService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArtistService.Data.Repositories.Configurations
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
            builder.HasIndex(h => h.ArtistGuid);
            builder.Property(m => m.ArtistGuid).IsRequired();

            builder.Property(m => m.Date)
                   .HasColumnType("datetime")
                   .IsRequired();

            builder.HasIndex(m => new { m.ArtistGuid, m.DateDay })
                   .IsUnique();
            builder.Property(m => m.DateDay)
                   .HasColumnType("Date")
                   .HasComputedColumnSql("DATE(`Date`)", stored: true);
        }
    }
}
