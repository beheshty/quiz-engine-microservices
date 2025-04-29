using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuestionService.Domain.Repositories;
using QuestionService.Infrastructure.Data;
using QuestionService.Infrastructure.Repositories;

namespace QuestionService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(
            configuration.GetSection(nameof(MongoDbSettings)));

        services.AddSingleton<MongoDbContext>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();

        return services;
    }
} 