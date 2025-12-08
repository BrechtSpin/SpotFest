using LoggerService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoggerService.Data.Repositories.Configuration;

internal class ChangeLogConfiguration : IEntityTypeConfiguration<ChangeLog>
{
    public void Configure(EntityTypeBuilder<ChangeLog> builder)
    {
        builder.HasKey(cl => cl.Id);

        builder.HasMany(cl => cl.PropertyChanges)
            .WithOne(pc => pc.ChangeLog)
            .HasForeignKey(pc => pc.ChangeLogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(cl => cl.EntityType)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(cl => cl.EntityId)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(cl => cl.Operation)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(cl => cl.ChangedBy)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(cl => cl.ChangedAt)
            .IsRequired();


        builder.HasIndex(e => e.EntityType);
        builder.HasIndex(e => e.EntityId);
        builder.HasIndex(e => e.ChangedBy);
        builder.HasIndex(e => e.ChangedAt);
    }
}