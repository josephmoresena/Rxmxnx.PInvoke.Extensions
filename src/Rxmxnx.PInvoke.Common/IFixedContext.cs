namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface represents a context from memory block fixing.
/// </summary>
/// <typeparam name="T">Type of items on the fixed memory block.</typeparam>
public interface IFixedContext<T> : IReadOnlyFixedContext<T> where T : unmanaged
{
    /// <summary>
    /// A <typeparamref name="T"/> span over the fixed memory block.
    /// </summary>
    new Span<T> Values { get; }
    /// <summary>
    /// A binary span over the fixed memory block.
    /// </summary>
    new Span<Byte> BinaryValues { get; }

    /// <summary>
    /// Performs a reinterpretation of <typeparamref name="T"/> fixed memory block as 
    /// <typeparamref name="TDestination"/> memory block.
    /// </summary>
    /// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
    /// <returns>A <see cref="ITransformationContext{T,TDestination}"/> instance.</returns>
    new ITransformationContext<T, TDestination> Transformation<TDestination>() where TDestination : unmanaged;
}