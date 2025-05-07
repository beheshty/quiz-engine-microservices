using QuizService.Application.Common.CQRS;
using QuizService.Application.Common.Grpc;
using QuizService.Application.Commands.CreateQuiz;
using QuizService.Infrastructure.Extensions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add infrastructure services
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add gRPC client
builder.Services.AddQuestionGrpcClient(builder.Configuration);

// Add CQRS services
builder.Services.AddCQRS(typeof(CreateQuizCommand).Assembly);

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

