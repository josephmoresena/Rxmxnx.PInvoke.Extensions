namespace Rxmxnx.PInvoke;

/// <summary>
/// Interface representing a context from a block of fixed memory.
/// </summary>
/// <typeparam name="T">Type of objects in the fixed memory block.</typeparam>
public interface IFixedContext<T> : IReadOnlyFixedContext<T>, IFixedMemory<T> where T : unmanaged
{
    /// <summary>
    /// Reinterprets the <typeparamref name="T"/> fixed memory block as 
    /// a <typeparamref name="TDestination"/> memory block.
    /// </summary>
    /// <typeparam name="TDestination">Type of objects in the reinterpreted memory block.</typeparam>
    /// <param name="residual">Output. Residual memory from the transformation.</param>
    /// <returns>An instance of <see cref="IFixedContext{TDestination}"/>.</returns>
    IFixedContext<TDestination> Transformation<TDestination>(out IFixedMemory residual) where TDestination : unmanaged;

    /// <summary>
    /// Reinterprets the <typeparamref name="T"/> fixed memory block as 
    /// a <typeparamref name="TDestination"/> memory block.
    /// </summary>
    /// <typeparam name="TDestination">Type of objects in the reinterpreted memory block.</typeparam>
    /// <param name="residual">Output. Residual read-only memory from the transformation.</param>
    /// <returns>An instance of <see cref="IReadOnlyFixedContext{TDestination}"/>.</returns>
    new IFixedContext<TDestination> Transformation<TDestination>(out IReadOnlyFixedMemory residual) where TDestination : unmanaged;
}