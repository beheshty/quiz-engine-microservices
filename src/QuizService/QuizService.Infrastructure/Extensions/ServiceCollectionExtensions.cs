using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuizService.Infrastructure.Data;

namespace QuizService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseSettings = configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>();
        
        services.AddDbContext<QuizDbContext>(options =>
            options.UseNpgsql(databaseSettings?.ConnectionString ?? throw new InvalidOperationException("Database connection string is not configured.")));

        return services;
    }
} 