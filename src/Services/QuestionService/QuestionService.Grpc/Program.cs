using QuestionService.Application;
using QuestionService.Infrastructure.Data;
using QuestionService.Infrastructure;

namespace QuestionService.Grpc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.Configure<MongoDbSettings>(
                builder.Configuration.GetSection("MongoDB"));
            builder.Services.AddGrpc();
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<Services.QuestionService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();
        }
    }
}