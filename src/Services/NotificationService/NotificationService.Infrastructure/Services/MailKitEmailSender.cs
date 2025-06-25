using MailKit.Net.Smtp;
using MimeKit;
using NotificationService.Infrastructure.Abstractions;

namespace NotificationService.Infrastructure.Services
{
    public class MailKitEmailSender : IEmailSender
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;
        private readonly string _fromAddress;
        private readonly string _fromName;

        public MailKitEmailSender(string smtpHost, int smtpPort, string smtpUser, string smtpPass, string fromAddress, string fromName)
        {
            _smtpHost = smtpHost;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
            _fromAddress = fromAddress;
            _fromName = fromName;
        }

        public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false, CancellationToken cancellationToken = default)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_fromName, _fromAddress));
            message.To.Add(MailboxAddress.Parse(to));
            message.Subject = subject;
            message.Body = isHtml ? new TextPart("html") { Text = body } : new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpHost, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(_smtpUser, _smtpPass, cancellationToken);
            await client.SendAsync(message, cancellationToken);
            await client.DisconnectAsync(true, cancellationToken);
        }
    }
}