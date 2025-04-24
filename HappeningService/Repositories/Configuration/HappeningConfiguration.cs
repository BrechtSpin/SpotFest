using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using HappeningService.Models;

namespace MusicHappenings.Repositories.Configuration;

public class HappeningConfiguration : IEntityTypeConfiguration<Happening>
{
    public void Configure(EntityTypeBuilder<Happening> builder)
    {
        builder.HasMany(h => h.HappeningArtists)
               .WithOne(ha => ha.Happening)
               .HasForeignKey(ha => ha.ArtistGuid);

        builder.HasKey(h => h.Guid);
        builder.Property(h => h.Guid).IsRequired();

        builder.Property(h => h.Name)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(h => h.Slug)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(h => h.StartTime)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(h => h.EndTime)
               .HasColumnType("datetime");
    }
}
