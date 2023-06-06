namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a read-only reference to a fixed memory location.
/// </summary>
/// <typeparam name="T">Type of the value referenced in memory.</typeparam>
public interface IReadOnlyFixedReference<T> : IReadOnlyReferenceable<T>, IReadOnlyFixedMemory where T : unmanaged
{
    /// <summary>
    /// Reinterprets the <typeparamref name="T"/> fixed memory reference as a 
    /// <typeparamref name="TDestination"/> memory reference.
    /// </summary>
    /// <typeparam name="TDestination">Type of the reinterpreted memory reference.</typeparam>
    /// <param name="residual">Output. Residual read-only memory left after the transformation.</param>
    /// <returns>
    /// A <see cref="IReadOnlyFixedReference{TDestination}"/> instance.
    /// </returns>
    IReadOnlyFixedReference<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual) where TDestination : unmanaged;
}
