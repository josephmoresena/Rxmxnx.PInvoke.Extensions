using System;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// This interface represents a context from memory block fixing.
    /// </summary>
    /// <typeparam name="T">Type of items on the fixed memory block.</typeparam>
    public interface IFixedContext<T> where T : unmanaged
    {
        /// <summary>
        /// A <typeparamref name="T"/> span over the fixed memory block.
        /// </summary>
        Span<T> Values { get; }
        /// <summary>
        /// A binary span over the fixed memory block.
        /// </summary>
        Span<Byte> BinaryValues { get; }

        /// <summary>
        /// Performs a reinterpretation of <typeparamref name="T"/> fixed memory block as 
        /// <typeparamref name="TDestination"/> memory block.
        /// </summary>
        /// <typeparam name="TDestination">Type of items on the reinterpreded memory block.</typeparam>
        /// <returns>A <see cref="ITransformationContext{T,TDestination}"/> instance.</returns>
        ITransformationContext<T, TDestination> Transformation<TDestination>() where TDestination : unmanaged;

        /// <summary>
        /// Retrieves the current context as read-only fixed memory block context.
        /// </summary>
        /// <returns>A <see cref="IReadOnlyFixedContext{T}"/> instance.</returns>
        IReadOnlyFixedContext<T> AsReadOnly();
    }
}
