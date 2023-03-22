namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a read-only fixed memory reference.
/// </summary>
/// <typeparam name="T">Type of referenced value.</typeparam>
public interface IReadOnlyFixedReference<T> : IReadOnlyReferenceable<T>, IReadOnlyFixedMemory where T : unmanaged
{
    /// <summary>
    /// Performs a reinterpretation of <typeparamref name="T"/> fixed memory reference as 
    /// <typeparamref name="TDestination"/> memory reference.
    /// </summary>
    /// <typeparam name="TDestination">Type of the reinterpreded memory reference.</typeparam>
    /// <returns>
    /// A <see cref="IReadOnlyTransformationReference{TSource, TDestination}"/> instance.
    /// </returns>
    IReadOnlyTransformationReference<T, TDestination> Transformation<TDestination>() where TDestination : unmanaged;
}
