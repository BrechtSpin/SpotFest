using UserAuthService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserAuthService.Data.Repositories.Configuration;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.Property(u => u.FirstName)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(u => u.LastName)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.LastLoginAt)
            .HasDefaultValueSql<DateTime>("CURRENT_TIMESTAMP()");

        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}
