using Microsoft.AspNetCore.Identity;

namespace UserAuthService.Models;

public class AppUser : IdentityUser
{
    // IdentityUser includes:
    // - Id (string)                    -- Gets or sets the primary key for this user.
    // - UserName (string)              -- Gets or sets the user name for this user.
    // - NormalizedUserName 	        -- Gets or sets the normalized user name for this user.
    // - Email (string)                 -- Gets or sets the email address for this user.
    // - NormalizedEmail (string)       -- Gets or sets the normalized email address for this user.
    // - EmailConfirmed (bool)          -- Gets or sets a flag indicating if a user has confirmed their email address.
    // - PasswordHash (string)          -- Gets or sets a salted and hashed representation of the password for this user.
    // - PhoneNumber (string)           -- Gets or sets a telephone number for the user.
    // - PhoneNumberConfirmed (bool)    -- Gets or sets a flag indicating if a user has confirmed their telephone address.
    // - TwoFactorEnabled (bool)        -- Gets or sets a flag indicating if two factor authentication is enabled for this user.
    // - LockoutEnd (DateTimeOffset?)   -- Gets or sets the date and time, in UTC, when any user lockout ends.
    // - LockoutEnabled (bool)          -- Gets or sets a flag indicating if the user could be locked out.
    // - AccessFailedCount (int)        -- Gets or sets the number of failed login attempts for the current user.
    // - SecurityStamp (string)         -- A random value that must change whenever a users credentials change (password changed, login removed)
    // - ConcurrencyStamp (string)      -- A random value that must change whenever a user is persisted to the store

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastLoginAt { get; set; } = DateTime.UtcNow;
}
