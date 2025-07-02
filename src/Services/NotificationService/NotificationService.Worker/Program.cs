using BuildingBlocks.EventBus.Distributed.RabbitMQ.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NotificationService.Application;
using NotificationService.Application.Grpc;
using NotificationService.Infrastructure.Extensions;
using NotificationService.Worker.Quizzes;

Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("appsettings.json");
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
    })
    .Build().Run();