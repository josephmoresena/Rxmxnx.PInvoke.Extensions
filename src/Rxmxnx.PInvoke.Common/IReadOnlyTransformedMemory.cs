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
