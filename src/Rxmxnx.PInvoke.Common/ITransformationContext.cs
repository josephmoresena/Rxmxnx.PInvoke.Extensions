namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents the reinterpretation of a <typeparamref name="TSource"/> fixed memory 
/// block as a <typeparamref name="TDestination"/> memory block.
/// </summary>
/// <typeparam name="TSource">Type of items on the fixed memory block.</typeparam>
/// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
public interface ITransformationContext<TSource, TDestination>
    where TSource : unmanaged
    where TDestination : unmanaged
{
    /// <summary>
    /// Fixed context instance.
    /// </summary>
    IFixedContext<TSource> Context { get; }
    /// <summary>
    /// A <typeparamref name="TDestination"/> span over the fixed memory block. 
    /// </summary>
    Span<TDestination> Values { get; }
    /// <summary>
    /// A binary span over the residual fixed memory block. 
    /// </summary>
    Span<Byte> ResidualBytes { get; }

    /// <summary>
    /// Retrieves the current context as read-only fixed memory block transformation context.
    /// </summary>
    /// <returns>A <see cref="ITransformationContext{TSource, TDestination}"/> instance.</returns>
    IReadOnlyTransformationContext<TSource, TDestination> AsReadOnly();
}
