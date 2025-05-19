using Microsoft.EntityFrameworkCore;
using InformationService.Models;
using InformationService.Data.Repositories.Configuration;

namespace InformationService.Data.Repositories;

public class ContactContext(DbContextOptions<ContactContext> options) : DbContext(options)
{
    public DbSet<Contact> Contacts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ContactConfiguration());
    }
}