using System;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// This interface represents a context from read-only memory block fixing.
    /// </summary>
    /// <typeparam name="T">Type of items on the read-only fixed memory block.</typeparam>
    public interface IReadOnlyFixedContext<T> where T : unmanaged
    {
        /// <summary>
        /// A read-only <typeparamref name="T"/> span over the fixed memory block.
        /// </summary>
        ReadOnlySpan<T> Values { get; }
        /// <summary>
        /// A read-only binary span over the fixed memory block.
        /// </summary>
        ReadOnlySpan<Byte> BinaryValues { get; }

        /// <summary>
        /// Performs a reinterpretation of <typeparamref name="T"/> fixed memory block as 
        /// <typeparamref name="TDestination"/> memory block.
        /// </summary>
        /// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
        /// <returns>A <see cref="ITransformationContext{T,TDestination}"/> instance.</returns>
        IReadOnlyTransformationContext<T, TDestination> Transformation<TDestination>() where TDestination : unmanaged;
    }
}
