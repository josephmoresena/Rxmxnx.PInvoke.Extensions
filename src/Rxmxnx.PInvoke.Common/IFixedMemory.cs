namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a fixed memory block.
/// </summary>
public interface IFixedMemory : IReadOnlyFixedMemory
{
    /// <summary>
    /// A binary span over the fixed memory block.
    /// </summary>
    new Span<Byte> Bytes { get; }

    /// <summary>
    /// Creates a <see cref="IFixedContext{Byte}"/> instance from current instance.
    /// </summary>
    /// <returns>A <see cref="IFixedContext{Byte}"/> instance.</returns>
    new IFixedContext<Byte> AsBinaryContext() => FixedContext<Byte>.CreateBinaryContext(this);
}

/// <summary>
/// This interface represents a fixed memory block.
/// </summary>
/// <typeparam name="T">Type of fixed memory block.</typeparam>
public interface IFixedMemory<T> : IFixedMemory, IReadOnlyFixedMemory<T> where T : unmanaged
{
    /// <summary>
    /// A <typeparamref name="T"/> span over the fixed memory block.
    /// </summary>
    new Span<T> Values { get; }
}