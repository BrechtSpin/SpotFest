using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserAuthService.Models;
using UserAuthService.Data.Repositories.Configuration;

namespace UserAuthService.Data.Repositories;

public class UserAuthContext(DbContextOptions<UserAuthContext> options) : IdentityDbContext(options)
{

    public DbSet<SpotFestUser> AppUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new AppUserConfiguration());
    }
}
