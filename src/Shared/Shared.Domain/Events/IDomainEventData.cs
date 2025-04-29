namespace QuizEngineMicroservices.Shared.Domain.Events;

/// <summary>
/// Represents the data for a domain event.
/// </summary>
public interface IDomainEventData
{
    /// <summary>
    /// The actual event data.
    /// </summary>
    object EventData { get; }

    /// <summary>
    /// The order of the event.
    /// </summary>
    long EventOrder { get; }

    /// <summary>
    /// The type of event dispatch (Local or Distributed).
    /// </summary>
    EventPublishType DispatchType { get; }
} 