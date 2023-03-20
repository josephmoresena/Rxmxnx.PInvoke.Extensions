namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents the reinterpretation of a read-only
/// <typeparamref name="TSource"/> fixed memory block as a
/// read-only <typeparamref name="TDestination"/> memory block.
/// </summary>
/// <typeparam name="TSource">Type of items on the fixed memory block.</typeparam>
/// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
public interface IReadOnlyTransformationContext<TSource, TDestination> : IReadOnlyTransformedMemory
    where TSource : unmanaged
    where TDestination : unmanaged
{
    /// <summary>
    /// Fixed context instance.
    /// </summary>
    IReadOnlyFixedContext<TSource> Context { get; }
    /// <summary>
    /// Fixed transformed context instance.
    /// </summary>
    IReadOnlyFixedContext<TDestination> Transformation { get; }
    /// <summary>
    /// A read-only <typeparamref name="TDestination"/> span over the fixed memory block. 
    /// </summary>
    ReadOnlySpan<TDestination> Values { get; }
}
