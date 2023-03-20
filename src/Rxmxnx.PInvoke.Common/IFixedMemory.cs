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