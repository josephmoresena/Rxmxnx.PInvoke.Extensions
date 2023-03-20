namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents the reinterpretation of a fixed memory block.
/// </summary>
public interface ITransformedMemory : IReadOnlyTransformedMemory
{
    /// <summary>
    /// A read-only binary span over the residual fixed memory block. 
    /// </summary>
    new Span<Byte> ResidualBytes { get; }
}
