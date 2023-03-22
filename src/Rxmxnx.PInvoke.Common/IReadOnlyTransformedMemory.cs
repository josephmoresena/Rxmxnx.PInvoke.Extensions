namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents the reinterpretation of a read-only fixed memory block.
/// </summary>
public interface IReadOnlyTransformedMemory
{
    /// <summary>
    /// A read-only binary span over the residual fixed memory block. 
    /// </summary>
    ReadOnlySpan<Byte> ResidualBytes { get; }
}

/// <summary>
/// This interface represents the reinterpretation of a read-only fixed memory block.
/// </summary>
/// <typeparam name="TMemory">Type of read-only fixed memory.</typeparam>
public interface IReadOnlyTransformedMemory<out TMemory> : IReadOnlyTransformedMemory
    where TMemory : IReadOnlyFixedMemory
{
    /// <summary>
    /// Read-only fixed transformed memory instance.
    /// </summary>
    TMemory Transformation { get; }
}
