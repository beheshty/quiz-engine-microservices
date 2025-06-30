using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Quiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Application
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IQuizCompletedAppService, QuizCompletedAppService>();
            return services;
        }
    }
}
