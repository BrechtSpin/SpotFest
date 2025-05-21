namespace InformationService.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmailAddress, string subject, string body, string replyToEmailAddress = "", CancellationToken cancellationToken = default);
    }
}
