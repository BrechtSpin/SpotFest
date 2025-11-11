using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.CompilerServices;
using System.Text;
using UserAuthService.Data.Repositories;
using UserAuthService.Models;

namespace UserAuthService;

public static class AuthConfig
{
    public static void AddAuthIdentityConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<SpotFestUser, IdentityRole>(options =>
        {
            options.User.RequireUniqueEmail = true;

            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;

        })
            .AddEntityFrameworkStores<UserAuthContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JWT:ISSUER"],
                    ValidAudience = configuration["JWT:AUDIENCE"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:KEY"]!))
                };

                //read from http-only cookie
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("SpotFestUser"))
                        {
                            context.Token = context.Request.Cookies["SpotFestUser"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });
    }
}
