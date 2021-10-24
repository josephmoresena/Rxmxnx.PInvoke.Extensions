using System;

namespace PInvoke.Extensions
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
        public static Byte[] AsBytes<T>(this T value)
            where T : unmanaged
            => NativeUtilities.AsBytes(value);
    }
}
