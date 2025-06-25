using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Abstractions;
using NotificationService.Infrastructure.Services;
using NotificationService.Infrastructure.Configurations;

namespace NotificationService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEMailSender(this IServiceCollection services, Action<MailSenderOptions> options)
        {
            var mailOptions = new MailSenderOptions();
            options.Invoke(mailOptions);

            services.AddSingleton<IEmailSender>(provider =>
            {
                return new MailKitEmailSender(
                    mailOptions.Host,
                    mailOptions.Port,
                    mailOptions.User,
                    mailOptions.Pass,
                    mailOptions.FromAddress,
                    mailOptions.FromName);
            });

            return services;
        }
    }
}
