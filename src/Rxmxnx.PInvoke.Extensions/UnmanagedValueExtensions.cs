using System;

namespace Rxmxnx.PInvoke.Extensions.Tests
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

        /// <summary>
        /// Creates an array of <typeparamref name="TTo"/> values from 
        /// an array of <typeparamref name="TFrom"/> values. 
        /// </summary>
        /// <typeparam name="TTo">Destination <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <typeparam name="TFrom">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <param name="array">Array of <typeparamref name="TFrom"/> values.</param>
        /// <returns>Array of <typeparamref name="TTo"/> values.</returns>
        public static TTo[]? AsValues<TFrom, TTo>(this TFrom[] array)
            where TFrom : unmanaged
            where TTo : unmanaged
        {
            if (array != default)
            {
                Span<TFrom> initialSpan = array.AsSpan();
                IntPtr ptr = initialSpan.AsIntPtr();
                Int32 length = array.Length * NativeUtilities.SizeOf<TFrom>();
                Int32 step = NativeUtilities.SizeOf<TTo>();
                Int32 count = length / step;
                Int32 module = length % step;
                TTo[] values = new TTo[count + (count != 0 ? 1 : 0)];
                Span<TTo> finalSpan = values.AsSpan();
                ptr.AsReadOnlySpan<TTo>(count).CopyTo(finalSpan);
                if (module != 0)
                {
                    Byte[] missing = new Byte[step];
                    ptr.AsReadOnlySpan<Byte>(count).Slice(length - module, module).CopyTo(missing);
                    finalSpan[^1] = missing.AsValue<TTo>();
                }
            }
            return default;
        }
    }
}
