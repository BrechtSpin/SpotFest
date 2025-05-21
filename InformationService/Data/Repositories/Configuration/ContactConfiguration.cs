using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InformationService.Models;

namespace InformationService.Data.Repositories.Configuration;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {

        builder.HasKey(c => c.Guid);
        builder.Property(c => c.Guid).IsRequired();

        builder.Property(c => c.Date)
               .HasColumnType("datetime")
               .IsRequired();

        builder.Property(c => c.Name)
               .HasMaxLength(200)
               .IsRequired();

        builder.HasIndex(c => c.Email).IsUnique();
        builder.Property(c => c.Email)
               .HasMaxLength(200)
               .IsRequired();
    }
}
