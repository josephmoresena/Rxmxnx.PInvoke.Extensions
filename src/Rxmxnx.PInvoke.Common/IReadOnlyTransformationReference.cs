namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents the reinterpretation of a read-only
/// <typeparamref name="TSource"/> fixed memory reference as a read-only
/// <typeparamref name="TDestination"/> memory reference.
/// </summary>
/// <typeparam name="TSource">Type of the fixed memory reference.</typeparam>
/// <typeparam name="TDestination">Type of the reinterpreded memory reference.</typeparam>
public interface IReadOnlyTransformationReference<TSource, TDestination> : IReferenceable<TDestination>, IReadOnlyTransformedMemory
    where TSource : unmanaged
    where TDestination : unmanaged
{
    /// <summary>
    /// Fixed read-only reference instance.
    /// </summary>
    IReadOnlyFixedReference<TSource> Fixed { get; }
    /// <summary>
    /// Fixed read-only transformed reference instance.
    /// </summary>
    IReadOnlyFixedContext<TDestination> Transformation { get; }
}