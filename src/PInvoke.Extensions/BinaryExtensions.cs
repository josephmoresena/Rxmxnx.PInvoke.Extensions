using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace PInvoke.Extensions
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
        public static T AsValue<T>(this Byte[] array)
            where T : unmanaged
        {
            Int32 typeSize = NativeUtilities.SizeOf<T>();
            if (array.Length != typeSize)
                throw new ArgumentException($"The length of parameter {array} must be equals to {typeSize} .");
            unsafe
            {
                return Unsafe.Read<T>(array.AsSpan<Byte>().AsIntPtr<Byte>().ToPointer());
            }
        }

        /// <summary>
        /// Gets <see cref="String"/> representation of <see cref="Byte"/> array hexadecimal value.
        /// </summary>
        /// <param name="bytes"><see cref="Byte"/> array.</param>
        /// <returns><see cref="String"/> representation of hexadecimal value.</returns>
        public static String AsHexString(this Byte[] bytes)
            => String.Concat(bytes.Select(b => b.AsHexString()));

        /// <summary>
        /// Gets <see cref="String"/> representation of <see cref="Byte"/> hexadecimal value.
        /// </summary>
        /// <param name="value"><see cref="Byte"/> value.</param>
        /// <returns><see cref="String"/> representation of hexadecimal value.</returns>
        public static String AsHexString(this Byte value)
            => value.ToString("X2").ToLower();
    }
}
