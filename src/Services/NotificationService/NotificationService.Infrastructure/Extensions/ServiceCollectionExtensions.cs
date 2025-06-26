using Microsoft.Extensions.DependencyInjection;
using NotificationService.Infrastructure.Abstractions;
using NotificationService.Infrastructure.Services;
using NotificationService.Infrastructure.Configurations;

namespace NotificationService.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailSender(this IServiceCollection services, Action<MailSenderOptions> options)
        {
            var mailOptions = new MailSenderOptions();
            options.Invoke(mailOptions);

            services.AddSingleton<IEmailSender>(provider =>
            {
                return new MailKitEmailSender(mailOptions);
            });

            return services;
        }
    }
}
