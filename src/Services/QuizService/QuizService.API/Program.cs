using QuizService.Application.Common.CQRS;
using QuizService.Application.Common.Grpc;
using QuizService.Application.Common;
using QuizService.Application.Commands.CreateQuiz;
using QuizService.Infrastructure.Extensions;
using QuizService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using BuildingBlocks.EventBus.Domain.Extensions;
using BuildingBlocks.EventBus.Local.Extensions;
using BuildingBlocks.EventBus.Distributed.RabbitMQ.Extensions;
using QuizService.Domain.Entities.QuizManagement;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["JwtSettings:Authority"];
        options.Audience = builder.Configuration["JwtSettings:Audience"];
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.OAuth2,
        Flows = new Microsoft.OpenApi.Models.OpenApiOAuthFlows
        {
            AuthorizationCode = new Microsoft.OpenApi.Models.OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(builder.Configuration["OAuth:AuthorizationUrl"]),
                TokenUrl = new Uri(builder.Configuration["OAuth:TokenUrl"]),
                Scopes = new Dictionary<string, string>
                {
                    { "profile", "User profile" },
                    { "email", "User email" },
                    { "quiz_api", "Quiz API access" },
                    { "roles", "User roles" }
                }
            }
        }
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "oauth2"
                }
            },
            new[] { "profile", "email", "quiz_api", "roles" }
        }
    });
});

// Add infrastructure services
builder.Services.AddLocalEventBus();
builder.Services.AddRabbitMqMassTransitEventBus(typeof(Quiz).Assembly, o =>
{
    o.HostUrl = builder.Configuration["RABBITMQ:HOST"];
    o.Username = builder.Configuration["RABBITMQ:USERNAME"];
    o.Password = builder.Configuration["RABBITMQ:PASSWORD"];
});
builder.Services.AddDomainEventBus(o =>
{
    o.UseLocal = true;
    o.UseDistributed = true;
});
builder.Services.AddInfrastructureServices(builder.Configuration);

// Add gRPC client
builder.Services.AddQuestionGrpcClient(builder.Configuration);

// Add application services
builder.Services.AddApplicationServices();

// Add CQRS services
builder.Services.AddCQRS(typeof(CreateQuizCommand).Assembly);

builder.Services.AddControllers();

var app = builder.Build();

// Add to pipeline
app.UseRouting();
app.UseCors(o =>
{
    o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
});
app.UseAuthentication();
app.UseAuthorization();

// Apply migrations at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<QuizDbContext>();
    dbContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "QuizService.API v1");
        c.OAuthClientId(builder.Configuration["OAuth:ClientId"]);
        c.OAuthAppName("Quiz Service Swagger UI");
        c.OAuthUsePkce();
    });
}

app.MapControllers();

app.Run();

