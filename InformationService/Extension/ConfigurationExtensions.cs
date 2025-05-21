using InformationService.Models;
using Microsoft.Extensions.Options;

namespace InformationService.Extension;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddEnvironmentSettings(this IServiceCollection services)
    {
        services.Configure<EmailSettings>(options =>
        {
            options.SmtpServer = Environment.GetEnvironmentVariable("MAIL_SMTP_SERVER")
                    ?? throw new InvalidOperationException("Environment variable 'MAIL_SMTP_SERVER' is required");
            options.SmtpPort = int.TryParse(Environment.GetEnvironmentVariable("MAIL_SMTP_PORT"), out var port) ? port : 587;
            options.FromAddress = Environment.GetEnvironmentVariable("FROM_ADDRESS")
                ?? throw new InvalidOperationException("Environment variable 'FROM_ADDRESS' is required");
        });

        services.Configure<TokenClientSettings>(options =>
        {
            options.TenantId = Environment.GetEnvironmentVariable("MAIL_TENANT_ID")
                ?? throw new InvalidOperationException("Environment variable 'MAIL_TENANT_ID' is required");
            options.ClientId = Environment.GetEnvironmentVariable("MAIL_CLIENT_ID")
                ?? throw new InvalidOperationException("Environment variable 'MAIL_CLIENT_ID' is required");
            options.ClientSecret = Environment.GetEnvironmentVariable("MAIL_CLIENT_SECRET")
                ?? throw new InvalidOperationException("Environment variable 'MAIL_CLIENT_SECRET' is required");

            var scopesEnv = Environment.GetEnvironmentVariable("MAIL_SCOPES") ?? string.Empty;
            options.Scopes = scopesEnv.Split(';', StringSplitOptions.RemoveEmptyEntries);
        });


        services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value);
        services.AddSingleton(sp => sp.GetRequiredService<IOptions<TokenClientSettings>>().Value);

        return services;
    }
}
