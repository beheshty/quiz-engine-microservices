using MongoDB.Driver;
using QuestionService.Domain.Entities;
using QuestionService.Domain.Enums;
using QuestionService.Domain.Repositories;
using QuestionService.Infrastructure.Repositories.Base;

namespace QuestionService.Infrastructure.Repositories;

public class QuestionRepository : MongoRepository<Question, Guid>, IQuestionRepository
{
    private readonly IMongoCollection<Question> _collection;

    public QuestionRepository(IMongoCollection<Question> collection) : base(collection)
    {
        _collection = collection;
    }

    public async Task<(IEnumerable<Question> Items, int TotalCount)> GetFilteredListAsync(
        string? searchText = null,
        DifficultyLevel? difficultyLevel = null,
        int pageNumber = 0,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var filterBuilder = Builders<Question>.Filter;
        var filter = filterBuilder.Empty;

        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var searchFilter = filterBuilder.Regex(q => q.Text, new MongoDB.Bson.BsonRegularExpression(searchText, "i"));
            filter = filterBuilder.And(filter, searchFilter);
        }

        if (difficultyLevel.HasValue)
        {
            var difficultyFilter = filterBuilder.Eq(q => q.DifficultyLevel, difficultyLevel.Value);
            filter = filterBuilder.And(filter, difficultyFilter);
        }

        var totalCount = await _collection.CountDocumentsAsync(filter, cancellationToken: cancellationToken);
        var items = await _collection
            .Find(filter)
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        return (items, (int)totalCount);
    }
} 