namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a fixed memory reference.
/// </summary>
/// <typeparam name="T">Type of referenced value.</typeparam>
public interface IFixedReference<T> : IReferenceable<T>, IFixedMemory where T : unmanaged
{
    /// <summary>
    /// Performs a reinterpretation of <typeparamref name="T"/> fixed memory reference as 
    /// <typeparamref name="TDestination"/> memory reference.
    /// </summary>
    /// <typeparam name="TDestination">Type of the reinterpreded memory reference.</typeparam>
    /// <returns>
    /// A <see cref="ITransformationReference{TSource, TDestination}"/> instance.
    /// </returns>
    ITransformationReference<T, TDestination> Transformation<TDestination>() where TDestination : unmanaged;
}
