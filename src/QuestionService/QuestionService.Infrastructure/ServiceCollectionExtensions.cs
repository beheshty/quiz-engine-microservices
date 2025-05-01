using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using QuestionService.Domain.Entities;
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
        // Configure MongoDB settings
        services.Configure<MongoDbSettings>(
            configuration.GetSection("MongoDB"));

        // Register MongoDbContext as singleton
        services.AddSingleton<MongoDbContext>();

        // Register repositories as scoped
        services.AddScoped<IQuestionRepository, QuestionRepository>();

        return services;
    }
} 