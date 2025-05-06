namespace QuizService.Application.Grpc;

public class QuestionServiceSettings
{
    public const string SectionName = "QuestionService";
    public string GrpcUrl { get; set; } = string.Empty;
} 