using System.Collections.Generic;

namespace QuizEngineMicroservices.Shared.Domain.Events;

/// <summary>
/// Defines an interface for entities that can generate domain events.
/// </summary>
public interface IGeneratesDomainEvents
{
    /// <summary>
    /// Gets all domain events (both local and distributed).
    /// </summary>
    IEnumerable<DomainEventRecord> GetAllEvents();

    /// <summary>
    /// Clears all local events.
    /// </summary>
    void ClearLocalEvents();

    /// <summary>
    /// Clears all distributed events.
    /// </summary>
    void ClearDistributedEvents();

    /// <summary>
    /// Clears all events (both local and distributed).
    /// </summary>
    void ClearAllEvents();

    /// <summary>
    /// Checks if there are any events (either local or distributed).
    /// </summary>
    bool HasAnyEvents();
}