namespace QuestionService.Infrastructure.Data;

public class MongoDbSettings
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
    public string QuestionsCollectionName { get; set; } = string.Empty;
} 