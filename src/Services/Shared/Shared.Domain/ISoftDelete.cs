namespace QuizEngineMicroservices.Shared.Domain;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
}