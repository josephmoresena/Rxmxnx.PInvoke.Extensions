using System;
using System.Runtime.InteropServices;

namespace PInvoke.Extensions
{
    /// <summary>
    /// Provides a set of extensions for basic operations with <see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}"/> instances.
    /// </summary>
    public static class MemoryBlockExtensions
    {
        /// <summary>
        /// Creates a <see cref="IntPtr"/> pointer from <see cref="Span{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
        /// </typeparam>
        /// <param name="span">The span from which the pointer is retrieved.</param>
        /// <returns><see cref="IntPtr"/> pointer.</returns>
        public static IntPtr AsIntPtr<T>(this Span<T> span)
            where T : unmanaged
        {
            if (span.IsEmpty)
                return IntPtr.Zero;
            unsafe
            {
                return new IntPtr(ReferenceExtensions.GetPointerFromRef(ref MemoryMarshal.GetReference(span)));
            }
        }

        /// <summary>
        /// Creates a <see cref="IntPtr"/> pointer from <see cref="ReadOnlySpan{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
        /// </typeparam>
        /// <param name="readonlySpan">The read-only from which the pointer is retrieved.</param>
        /// <returns><see cref="IntPtr"/> pointer.</returns>
        public static IntPtr AsIntPtr<T>(this ReadOnlySpan<T> readonlySpan)
            where T : unmanaged
        {
            if (readonlySpan.IsEmpty)
                return IntPtr.Zero;
            unsafe
            {
                return new IntPtr(ReferenceExtensions.GetPointerFromRef(ref MemoryMarshal.GetReference(readonlySpan)));
            }
        }
    }
}
