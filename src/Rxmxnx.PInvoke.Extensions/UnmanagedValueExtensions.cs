using System;
using System.Linq;

using Rxmxnx.PInvoke.Extensions.Internal;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Provides a set of extensions for basic operations with <see cref="ValueType"/> <see langword="unmanaged"/> values.
    /// </summary>
    public static class UnmanagedValueExtensions
    {
        /// <summary>
        /// Retrieves a <see cref="Byte"/> array from the given <typeparamref name="T"/> value.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <param name="value"><typeparamref name="T"/> value.</param>
        /// <returns><see cref="Byte"/> array.</returns>
        public static Byte[] AsBytes<T>(this T value) where T : unmanaged
            => NativeUtilities.AsBytes(value);

        /// <summary>
        /// Creates an array of <typeparamref name="TDestination"/> values from an array of 
        /// <typeparamref name="TSource"/> values. 
        /// </summary>
        /// <typeparam name="TDestination">Destination <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <param name="array">Array of <typeparamref name="TSource"/> values.</param>
        /// <returns>Array of <typeparamref name="TDestination"/> values.</returns>
        public static TDestination[]? AsValues<TSource, TDestination>(this TSource[] array)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            if (array == default)
                return default;

            if (!array.Any())
                return Array.Empty<TDestination>();

            return new ArrayValueConversion<TSource, TDestination>(array);
        }
    }
}
