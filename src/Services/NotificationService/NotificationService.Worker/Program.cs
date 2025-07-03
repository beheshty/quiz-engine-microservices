using BuildingBlocks.EventBus.Abstraction.Distributed;
using BuildingBlocks.EventBus.Distributed.RabbitMQ.Extensions;
using Contracts.Events.QuizService.UserQuiz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Application;
using NotificationService.Application.Grpc;
using NotificationService.Infrastructure.Extensions;
using NotificationService.Worker.Quizzes;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json");
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
    {
        IConfiguration config = context.Configuration;
        services.AddApplicationServices();
        services.AddUsersGrpcClient(config["UserService:GrpcUrl"]);
        services.AddEmailSender(o =>
        {
            o.Port = int.Parse(config["SMTP:Port"]);
            o.Host = config["SMTP:Host"];
            o.User = config["SMTP:User"];
            o.Pass = config["SMTP:Pass"];
            o.FromAddress = config["SMTP:FromAddress"];
            o.FromName = config["SMTP:FromName"];
        });
        services.AddRabbitMqMassTransitEventBus(typeof(UserQuizCompletedEventHandler).Assembly, o =>
        {
            o.HostUrl = config["RABBITMQ:HOST"];
            o.Username = config["RABBITMQ:USERNAME"];
            o.Password = config["RABBITMQ:PASSWORD"];
        });
        services.AddScoped<IDistributedEventHandler<UserQuizCompletedIntegrationEvent>, UserQuizCompletedEventHandler>();
    })
    .Build().Run();