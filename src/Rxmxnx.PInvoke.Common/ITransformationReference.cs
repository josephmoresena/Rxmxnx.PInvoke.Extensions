namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents the reinterpretation of a <typeparamref name="TSource"/> fixed memory 
/// reference as a <typeparamref name="TDestination"/> memory reference.
/// </summary>
/// <typeparam name="TSource">Type of the fixed memory reference.</typeparam>
/// <typeparam name="TDestination">Type of the reinterpreded memory reference.</typeparam>
public interface ITransformationReference<TSource, TDestination> : IReferenceable<TDestination>, ITransformedMemory
    where TSource : unmanaged
    where TDestination : unmanaged
{
    /// <summary>
    /// Fixed reference instance.
    /// </summary>
    IFixedReference<TSource> Fixed { get; }
    /// <summary>
    /// Fixed transformed reference instance.
    /// </summary>
    IFixedContext<TDestination> Transformation { get; }
}