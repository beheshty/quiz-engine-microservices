using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuestionService.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace QuestionService.Infrastructure.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;
    private readonly MongoDbSettings _settings;

    public MongoDbContext(IOptions<MongoDbSettings> settings, ILogger<MongoDbContext> logger)
    {
        _settings = settings.Value;

        var mongoClient = new MongoClient(settings.Value.ConnectionString);

        _database = mongoClient.GetDatabase(_settings.DatabaseName);
    }

    public IMongoCollection<Question> Questions => _database.GetCollection<Question>(nameof(Questions).ToLower());
}