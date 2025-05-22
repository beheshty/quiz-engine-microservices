using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QuizService.Domain.Repositories;
using QuizService.Infrastructure.Data;
using QuizService.Infrastructure.Repositories;

namespace QuizService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(options => 
            configuration.GetSection(DatabaseSettings.SectionName).Bind(options));
        
        services.AddDbContext<QuizDbContext>((serviceProvider, options) =>
        {
            var dbSettings = serviceProvider.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            options.UseNpgsql(dbSettings.ConnectionString ?? throw new InvalidOperationException("Database connection string is not configured."));
        });

        // Register repositories
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IUserQuizRepository, UserQuizRepository>();

        return services;
    }
} 