namespace Rxmxnx.PInvoke;

/// <summary>
/// s
/// </summary>
/// <typeparam name="T">Type of items on the read-only fixed memory block.</typeparam>
public interface IReadOnlyFixedContext<T> : IReadOnlyFixedMemory<T>
    where T : unmanaged
{
    /// <summary>
    /// Performs a reinterpretation of <typeparamref name="T"/> fixed memory block as 
    /// <typeparamref name="TDestination"/> memory block.
    /// </summary>
    /// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
    /// <returns>A <see cref="ITransformationContext{T,TDestination}"/> instance.</returns>
    IReadOnlyTransformationContext<T, TDestination> Transformation<TDestination>() where TDestination : unmanaged;
}
