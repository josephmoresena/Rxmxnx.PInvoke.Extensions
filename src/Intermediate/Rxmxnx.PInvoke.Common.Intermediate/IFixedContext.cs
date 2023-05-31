namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a context from memory block fixing.
/// </summary>
/// <typeparam name="T">Type of items on the fixed memory block.</typeparam>
public interface IFixedContext<T> : IReadOnlyFixedContext<T>, IFixedMemory<T> where T : unmanaged
{
    /// <summary>
    /// Performs a reinterpretation of <typeparamref name="T"/> fixed memory block as 
    /// <typeparamref name="TDestination"/> memory block.
    /// </summary>
    /// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
    /// <param name="residual">Output. Residual memory from transformation.</param>
    /// <returns>A <see cref="IFixedContext{TDestination}"/> instance.</returns>
    IFixedContext<TDestination> Transformation<TDestination>(out IFixedMemory residual) where TDestination : unmanaged;

    /// <summary>
    /// Performs a reinterpretation of <typeparamref name="T"/> fixed memory block as 
    /// <typeparamref name="TDestination"/> memory block.
    /// </summary>
    /// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
    /// <param name="residual">Output. Residual read-only memory from transformation.</param>
    /// <returns>A <see cref="IReadOnlyFixedContext{TDestination}"/> instance.</returns>
    new IFixedContext<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual) where TDestination : unmanaged;
}