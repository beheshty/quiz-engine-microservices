using MailKit.Net.Smtp;
using MimeKit;
using NotificationService.Infrastructure.Abstractions;
using NotificationService.Infrastructure.Configurations;

namespace NotificationService.Infrastructure.Services
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly MailSenderOptions _mailSenderOptions;

        public MailKitEmailSender(MailSenderOptions mailSenderOptions)
        {
            _mailSenderOptions = mailSenderOptions;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false, CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSenderOptions.FromName, _mailSenderOptions.FromAddress));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = isHtml ? new TextPart("html") { Text = body } : new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_mailSenderOptions.Host, _mailSenderOptions.Port, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(_mailSenderOptions.User, _mailSenderOptions.Pass, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}