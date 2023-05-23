namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a read-only fixed memory block.
/// </summary>
public interface IReadOnlyFixedMemory : IFixedPointer
{
    /// <summary>
    /// A read-only binary span over the fixed memory block.
    /// </summary>
    ReadOnlySpan<Byte> Bytes { get; }

    /// <summary>
    /// Creates a <see cref="IReadOnlyFixedContext{Byte}"/> instance from current instance.
    /// </summary>
    /// <returns>A <see cref="IReadOnlyFixedContext{Byte}"/> instance.</returns>
    IReadOnlyFixedContext<Byte> AsBinaryContext() => FixedContext<Byte>.CreateBinaryContext(this);
}

/// <summary>
/// This interface represents a read-only fixed memory block.
/// </summary>
/// <typeparam name="T">Type of fixed memory block.</typeparam>
public interface IReadOnlyFixedMemory<T> : IReadOnlyFixedMemory where T : unmanaged
{
    /// <summary>
    /// A read-only <typeparamref name="T"/> span over the fixed memory block.
    /// </summary>
    ReadOnlySpan<T> Values { get; }
}