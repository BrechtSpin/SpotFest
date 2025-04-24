using HappeningService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MusicHappenings.Repositories.Configuration;

public class HappeningArtistConfiguration : IEntityTypeConfiguration<HappeningArtist>
{
    public void Configure(EntityTypeBuilder<HappeningArtist> builder)
    {
        builder.HasOne(ha => ha.Happening)
               .WithMany(h => h.HappeningArtists)
               .HasForeignKey(ha => ha.HappeningGuid);

        builder.HasKey(ha => ha.Guid);
        builder.Property(ha => ha.Guid).IsRequired();

        builder.Property(ha => ha.HappeningGuid).IsRequired();

        builder.Property(ha => ha.ArtistGuid).IsRequired();

        builder.Property(ha => ha.Showtime)
               .HasColumnType("datetime");
    }
}
