using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using QuizService.Application.Grpc;
using Shared.Question.Grpc.Proto;

namespace QuizService.Application.Common.Grpc;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQuestionGrpcClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<QuestionServiceSettings>(configuration.GetSection(QuestionServiceSettings.SectionName));

        services.AddGrpcClient<Questions.QuestionsClient>((serviceProvider, options) =>
        {
            var settings = serviceProvider.GetRequiredService<IOptions<QuestionServiceSettings>>().Value;
            options.Address = new Uri(settings.GrpcUrl);
        });

        services.AddScoped<IQuestionGrpcClient, QuestionGrpcClient>();

        return services;
    }
} 