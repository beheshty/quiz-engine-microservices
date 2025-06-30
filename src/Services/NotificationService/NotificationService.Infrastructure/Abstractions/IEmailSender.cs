namespace NotificationService.Infrastructure.Abstractions
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = false, CancellationToken cancellationToken = default);
    }
}