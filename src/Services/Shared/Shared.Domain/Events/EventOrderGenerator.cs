using System.Threading;

namespace BuildingBlocks.Domain.Events;

/// <summary>
/// Generates sequential event order numbers in a thread-safe manner.
/// </summary>
public static class EventOrderGenerator
{
    private static long _lastOrder;

    /// <summary>
    /// Gets the next event order number.
    /// </summary>
    /// <returns>A unique, sequential event order number.</returns>
    public static long GetNext()
    {
        return Interlocked.Increment(ref _lastOrder);
    }
}