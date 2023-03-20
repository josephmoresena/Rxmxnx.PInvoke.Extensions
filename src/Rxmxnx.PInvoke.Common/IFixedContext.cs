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
    /// <returns>A <see cref="ITransformationContext{T,TDestination}"/> instance.</returns>
    new ITransformationContext<T, TDestination> Transformation<TDestination>() where TDestination : unmanaged;
}