using InformationService.Models;
using Microsoft.Extensions.Options;

namespace InformationService.Extension;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddEnvironmentSettings(this IServiceCollection services)
    {
        services.Configure<EmailSettings>(options =>
        {
        options.SmtpServer = Environment.GetEnvironmentVariable("MAIL_SMTPSERVER")
                ?? throw new InvalidOperationException("Environment variable 'MAIL_SMTPSERVER' is required");
        options.SmtpPort = int.TryParse(Environment.GetEnvironmentVariable("MAIL_PORT"), out var port) ? port : 587;
        options.SenderEmail = Environment.GetEnvironmentVariable("MAIL_SENDER")
            ?? throw new InvalidOperationException("Environment variable 'MAIL_SENDER' is required");
        options.PassCode = Environment.GetEnvironmentVariable("MAIL_PASSCODE")
            ?? throw new InvalidOperationException("Environment variable 'MAIL_PASSCODE' is required");
        options.ReplyToEmail = Environment.GetEnvironmentVariable("MAIL_REPLYTO")
            ?? "";
        });

        services.AddSingleton(sp => sp.GetRequiredService<IOptions<EmailSettings>>().Value);
        return services;
    }
}
