namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents the reinterpretation of a <typeparamref name="TSource"/> fixed memory 
/// block as a <typeparamref name="TDestination"/> memory block.
/// </summary>
/// <typeparam name="TSource">Type of items on the fixed memory block.</typeparam>
/// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
public interface ITransformationContext<TSource, TDestination> : IReadOnlyTransformationContext<TSource, TDestination>, ITransformedMemory
    where TSource : unmanaged
    where TDestination : unmanaged
{
    /// <summary>
    /// Fixed context instance.
    /// </summary>
    new IFixedContext<TSource> Context { get; }
    /// <summary>
    /// Fixed transformed context instance.
    /// </summary>
    new IFixedContext<TDestination> Transformation { get; }
    /// <summary>
    /// A <typeparamref name="TDestination"/> span over the fixed memory block. 
    /// </summary>
    new Span<TDestination> Values { get; }
}
