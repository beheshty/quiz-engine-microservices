using Microsoft.Extensions.DependencyInjection;
using QuestionService.Application.Services;

namespace QuestionService.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IQuestionAppService, Services.QuestionAppService>();

        return services;
    }
} 