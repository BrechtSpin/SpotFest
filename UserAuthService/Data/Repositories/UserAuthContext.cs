using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserAuthService.Models;
using UserAuthService.Data.Repositories.Configuration;

namespace UserAuthService.Data.Repositories;

public class UserAuthContext(DbContextOptions<UserAuthContext> options) : IdentityDbContext
{

    public DbSet<AppUser> AppUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AppUserConfiguration());
    }
}
