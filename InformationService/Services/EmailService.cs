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
    private readonly ILogger<EmailService> _logger;
    private readonly ResiliencePipeline _retryStrategy;
    private readonly EmailSettings _emailSettings;

    public EmailService(
        ILogger<EmailService> logger,
        EmailSettings emailSettings)
    {
        _logger = logger;
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
            OnRetry = args =>
            {
                _logger.LogError(args.Outcome.Exception!.Message);
                return default;
            }
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
        email.From.Add(MailboxAddress.Parse(_emailSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(toEmail));
        if (replyToMail == "") { replyToMail = _emailSettings.SenderEmail; }
        email.ReplyTo.Add(MailboxAddress.Parse(replyToMail));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };


        await _retryStrategy.ExecuteAsync(async ct =>
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _emailSettings.SmtpServer,
                _emailSettings.SmtpPort,
                SecureSocketOptions.StartTls,
                ct);
            await smtp.AuthenticateAsync(
                _emailSettings.SenderEmail,
                _emailSettings.PassCode,
                ct);
            await smtp.SendAsync(email, ct);
            await smtp.DisconnectAsync(true, ct);
        }, cancellationToken);
    }
}
