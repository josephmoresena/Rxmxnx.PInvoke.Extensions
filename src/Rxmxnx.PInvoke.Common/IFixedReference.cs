namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a fixed memory reference.
/// </summary>
/// <typeparam name="T">Type of referenced value.</typeparam>
public interface IFixedReference<T> : IReferenceable<T>, IReadOnlyFixedReference<T>, IFixedMemory where T : unmanaged
{
    /// <summary>
    /// Performs a reinterpretation of <typeparamref name="T"/> fixed memory reference as 
    /// <typeparamref name="TDestination"/> memory reference.
    /// </summary>
    /// <typeparam name="TDestination">Type of the reinterpreded memory reference.</typeparam>
    /// <param name="residual">Output. Residual memory from transformation.</param>
    /// <returns>
    /// A <see cref="IFixedReference{TDestination}"/> instance.
    /// </returns>
    IFixedReference<TDestination> Transformation<TDestination>(out IFixedMemory residual) where TDestination : unmanaged;

    /// <summary>
    /// Performs a reinterpretation of <typeparamref name="T"/> fixed memory reference as 
    /// <typeparamref name="TDestination"/> memory reference.
    /// </summary>
    /// <typeparam name="TDestination">Type of the reinterpreded memory reference.</typeparam>
    /// <param name="residual">Output. Residual read-only memory from transformation.</param>
    /// <returns>
    /// A <see cref="IReadOnlyFixedReference{TDestination}"/> instance.
    /// </returns>
    new IFixedReference<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual) where TDestination : unmanaged;
}
