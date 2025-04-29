using Microsoft.Extensions.Options;
using MongoDB.Driver;
using QuestionService.Domain.Entities;

namespace QuestionService.Infrastructure.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbSettings> settings)
    {
        var mongoClient = new MongoClient(settings.Value.ConnectionString);
        _database = mongoClient.GetDatabase(settings.Value.DatabaseName);
    }

    public IMongoCollection<Question> Questions => 
        _database.GetCollection<Question>(nameof(Questions).ToLower());
} 