using MimeKit;
using MailKit.Net.Smtp;
using Polly;
using Polly.Retry;
using Microsoft.AspNetCore.SignalR;
using MailKit.Security;
using InformationService.Models;

namespace InformationService.Services;
public class EmailService : IEmailService
{
    private readonly ResiliencePipeline _retryStrategy;
    private readonly EmailTokenClient _emailtokenClient;
    private readonly EmailSettings _emailSettings;

    public EmailService(
        EmailTokenClient emailTokenClient,
        EmailSettings emailSettings)
    {
        _emailtokenClient = emailTokenClient;
        _emailSettings = emailSettings;

        var Retries = new RetryStrategyOptions
        {
            MaxRetryAttempts = 20,
            DelayGenerator = args =>
            {
                var BaseDelay = TimeSpan.FromMinutes(1);
                return new ValueTask<TimeSpan?>(BaseDelay * Math.Pow(2, args.AttemptNumber));
            },
            MaxDelay = TimeSpan.FromMinutes(60),
        };
        _retryStrategy = new ResiliencePipelineBuilder()
            .AddRetry(Retries)
            .Build();
    }

    public async Task SendEmailAsync(
        string toEmail,
        string subject,
        string body,
        string replyToMail = "",
        CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse(_emailSettings.FromAddress));
        email.To.Add(MailboxAddress.Parse(toEmail));
        if (replyToMail != "") email.ReplyTo.Add(MailboxAddress.Parse(replyToMail));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };


        await _retryStrategy.ExecuteAsync(async ct =>
        {
            var token = await _emailtokenClient.GetTokenAsync();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _emailSettings.SmtpServer,
                _emailSettings.SmtpPort,
                SecureSocketOptions.StartTls, ct);
            await smtp.AuthenticateAsync(
                new SaslMechanismOAuth2(
                    _emailSettings.FromAddress,
                    token), ct);
            await smtp.SendAsync(email, ct);
            await smtp.DisconnectAsync(true, ct);
        }, cancellationToken);
    }
}
