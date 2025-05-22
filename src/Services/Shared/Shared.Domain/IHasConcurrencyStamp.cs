namespace QuizEngineMicroservices.Shared.Domain;

public interface IHasConcurrencyStamp
{
    public byte[] ConcurrencyStamp { get; set; }

}