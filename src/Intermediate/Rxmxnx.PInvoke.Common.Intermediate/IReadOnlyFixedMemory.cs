namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a read-only fixed block of memory.
/// </summary>
public interface IReadOnlyFixedMemory : IFixedPointer
{
    /// <summary>
    /// Gets a read-only binary span over the fixed block of memory.
    /// </summary>
    ReadOnlySpan<Byte> Bytes { get; }

    /// <summary>
    /// Creates a new instance of <see cref="IReadOnlyFixedContext{Byte}"/> from the current instance.
    /// </summary>
    /// <returns>An instance of <see cref="IReadOnlyFixedContext{Byte}"/>.</returns>
    IReadOnlyFixedContext<Byte> AsBinaryContext();
}

/// <summary>
/// Interface representing a read-only fixed block of memory.
/// </summary>
/// <typeparam name="T">Type of objects in the fixed memory block.</typeparam>
public interface IReadOnlyFixedMemory<T> : IReadOnlyFixedMemory where T : unmanaged
{
    /// <summary>
    /// Gets a read-only <typeparamref name="T"/> span over the fixed block of memory.
    /// </summary>
    ReadOnlySpan<T> Values { get; }
}