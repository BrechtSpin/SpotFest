using LoggerService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoggerService.Data.Repositories.Configuration;

internal class PropertyChangeConfiguration : IEntityTypeConfiguration<PropertyChange>
{
    public void Configure(EntityTypeBuilder<PropertyChange> builder)
    {
        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.PropertyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(pc => pc.OldValue)
            .HasMaxLength(4000);

        builder.Property(pc => pc.NewValue)
            .HasMaxLength(4000);


        builder.HasIndex(e => e.PropertyName);
    }
}