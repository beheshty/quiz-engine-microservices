
namespace QuizEngineMicroservices.Shared.Domain;

[Serializable]
public abstract class AggregateRoot : BasicAggregateRoot, IHasConcurrencyStamp
{
    public virtual byte[] ConcurrencyStamp { get; set; } = Array.Empty<byte>();

    protected AggregateRoot()
    {
    }
}

[Serializable]
public abstract class AggregateRoot<TKey> : BasicAggregateRoot<TKey>, IHasConcurrencyStamp
{
    public virtual byte[] ConcurrencyStamp { get; set; } = Array.Empty<byte>();

    protected AggregateRoot()
    {
    }

    protected AggregateRoot(TKey id)
        : base(id)
    {
    }
}