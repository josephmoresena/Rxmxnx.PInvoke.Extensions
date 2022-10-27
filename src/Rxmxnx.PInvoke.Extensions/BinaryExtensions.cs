using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

using Rxmxnx.PInvoke.Extensions.Internal;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Provides a set of extensions for basic operations with <see cref="Byte"/> instances.
    /// </summary>
    public static class BinaryExtensions
    {
        /// <summary>
        /// Retrieves a <typeparamref name="T"/> value from the given <see cref="Byte"/> array.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <param name="array"><see cref="Byte"/> array.</param>
        /// <returns><typeparamref name="T"/> readed value.</returns>
        public static T AsValue<T>(this Byte[] array) where T : unmanaged => array.AsSpan().AsValue<T>();
        /// <summary>
        /// Retrieves a <typeparamref name="T"/> value from the given <see cref="Byte"/> array.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <param name="span"><see cref="Byte"/> span.</param>
        /// <returns><typeparamref name="T"/> readed value.</returns>
        public static T AsValue<T>(this Span<Byte> span) where T : unmanaged => AsValue<T>((ReadOnlySpan<Byte>)span);
        /// <summary>
        /// Retrieves a <typeparamref name="T"/> value from the given <see cref="Byte"/> array.
        /// </summary>
        /// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <param name="span"><see cref="Byte"/> read-only span.</param>
        /// <returns><typeparamref name="T"/> readed value.</returns>
        public static T AsValue<T>(this ReadOnlySpan<Byte> span) where T : unmanaged
        {
            unsafe
            {
                Int32 typeSize = sizeof(T);
                if (span.Length != typeSize)
                    throw new ArgumentException($"The length of parameter {nameof(span)} must be equals to {typeSize}.");

                fixed (Byte* ptr = span)
                    return Unsafe.Read<T>(ptr);
            }
        }

        /// <summary>
        /// Concatenates the members of a collection of UTF-8 texts.
        /// </summary>
        /// <param name="values">A collection that contains the UTF-8 texts to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? ConcatUtf8(this IEnumerable<Byte[]> values) => Utf8BinaryConcatenation.Concat(values);

        #region AsHexString
        /// <summary>
        /// Gets <see cref="String"/> representation of <see cref="Byte"/> array hexadecimal value.
        /// </summary>
        /// <param name="bytes"><see cref="Byte"/> array.</param>
        /// <returns><see cref="String"/> representation of hexadecimal value.</returns>
        public static String AsHexString(this Byte[] bytes) => String.Concat(bytes.Select(b => b.AsHexString()));

        /// <summary>
        /// Gets <see cref="String"/> representation of <see cref="Byte"/> hexadecimal value.
        /// </summary>
        /// <param name="value"><see cref="Byte"/> value.</param>
        /// <returns><see cref="String"/> representation of hexadecimal value.</returns>
        public static String AsHexString(this Byte value) => value.ToString("X2").ToLower();
        #endregion
    }
}
