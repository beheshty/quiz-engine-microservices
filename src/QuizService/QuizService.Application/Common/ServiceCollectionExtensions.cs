using Microsoft.Extensions.DependencyInjection;
using QuizService.Application.Services;

namespace QuizService.Application.Common;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IQuestionService, QuestionService>();
        
        return services;
    }
} 