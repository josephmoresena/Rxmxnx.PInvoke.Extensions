using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Provides a set of extensions for basic operations with <see cref="Span{T}"/> and <see cref="ReadOnlySpan{T}"/> instances.
    /// </summary>
    public static partial class MemoryBlockExtensions
    {
        /// <summary>
        /// Creates a <see cref="IntPtr"/> pointer from <see cref="Span{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
        /// </typeparam>
        /// <param name="span">The span from which the pointer is retrieved.</param>
        /// <returns><see cref="IntPtr"/> pointer.</returns>
        public static IntPtr AsIntPtr<T>(this Span<T> span) where T : unmanaged
        {
            unsafe
            {
                return GetConditionalIntPtrZero(span.IsEmpty) ??
                    new IntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)));
            }
        }

        /// <summary>
        /// Creates a <see cref="IntPtr"/> pointer from <see cref="ReadOnlySpan{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
        /// </typeparam>
        /// <param name="readonlySpan">The read-only span from which the pointer is retrieved.</param>
        /// <returns><see cref="IntPtr"/> pointer.</returns>
        public static IntPtr AsIntPtr<T>(this ReadOnlySpan<T> readonlySpan) where T : unmanaged
        {
            unsafe
            {
                return GetConditionalIntPtrZero(readonlySpan.IsEmpty) ??
                    new IntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(readonlySpan)));
            }
        }

        /// <summary>
        /// Creates a <see cref="UIntPtr"/> pointer from <see cref="Span{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
        /// </typeparam>
        /// <param name="span">The span from which the pointer is retrieved.</param>
        /// <returns><see cref="UIntPtr"/> pointer.</returns>
        public static UIntPtr AsUIntPtr<T>(this Span<T> span) where T : unmanaged
        {
            unsafe
            {
                return GetConditionalUIntPtrZero(span.IsEmpty) ??
                    new UIntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)));
            }
        }

        /// <summary>
        /// Creates a <see cref="UIntPtr"/> pointer from <see cref="ReadOnlySpan{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">
        /// <see cref="ValueType"/> of <see langword="unmanaged"/> values contened into the contiguous region of memory.
        /// </typeparam>
        /// <param name="readonlySpan">The read-only span from which the pointer is retrieved.</param>
        /// <returns><see cref="UIntPtr"/> pointer.</returns>
        public static UIntPtr AsUIntPtr<T>(this ReadOnlySpan<T> readonlySpan) where T : unmanaged
        {
            unsafe
            {
                return GetConditionalUIntPtrZero(readonlySpan.IsEmpty) ??
                    new UIntPtr(Unsafe.AsPointer(ref MemoryMarshal.GetReference(readonlySpan)));
            }
        }

        /// <summary>
        /// Gets a <see cref="Nullable{IntPtr}"/> from a given condition. 
        /// </summary>
        /// <param name="condition">Indicates whether the <see cref="IntPtr.Zero"/> must be returned.</param>
        /// <returns>
        /// <see cref="IntPtr.Zero"/> if <paramref name="condition"/> is <see langword="true"/>; otherwise,
        /// <see langword="null"/>.
        /// </returns>
        private static IntPtr? GetConditionalIntPtrZero(Boolean condition) => condition ? IntPtr.Zero : default(IntPtr?);

        /// <summary>
        /// Gets a <see cref="Nullable{UIntPtr}"/> from a given condition. 
        /// </summary>
        /// <param name="condition">Indicates whether the <see cref="UIntPtr.Zero"/> must be returned.</param>
        /// <returns>
        /// <see cref="UIntPtr.Zero"/> if <paramref name="condition"/> is <see langword="true"/>; otherwise,
        /// <see langword="null"/>.
        /// </returns>
        private static UIntPtr? GetConditionalUIntPtrZero(Boolean condition) => condition ? UIntPtr.Zero : default(UIntPtr?);

        /// <summary>
        /// Retrieves a <typeparamref name="TDestination"/> span from <paramref name="sPtr"/> which references
        /// to a <typeparamref name="TSource"/> span whose length is <paramref name="sPtr"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <param name="sPtr">Source span pointer.</param>
        /// <param name="sLength">Source span length.</param>
        /// <param name="rSpan">Output. The residual span of bytes.</param>
        /// <returns>Destination span.</returns>
        private static unsafe Span<TDestination> GetDestinationSpan<TSource, TDestination>(void* sPtr, Int32 sLength, out Span<Byte> rSpan)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            void* resPtr = GetResidualPointer<TSource, TDestination>(sPtr, sLength, out Int32 desLength, out Int32 resLength);
            rSpan = new(resPtr, resLength);
            return new(sPtr, desLength);
        }

        /// <summary>
        /// Retrieves a <typeparamref name="TDestination"/> span from <paramref name="sPtr"/> which references
        /// to a <typeparamref name="TSource"/> read-only span whose length is <paramref name="sPtr"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <param name="sPtr">Source read-only span pointer.</param>
        /// <param name="sLength">Source read-only span length.</param>
        /// <param name="rSpan">Output. The residual span of bytes.</param>
        /// <returns>Destination read-only span.</returns>
        private static unsafe ReadOnlySpan<TDestination> GetDestinationSpan<TSource, TDestination>(void* sPtr, Int32 sLength, out ReadOnlySpan<Byte> rSpan)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            void* resPtr = GetResidualPointer<TSource, TDestination>(sPtr, sLength, out Int32 desLength, out Int32 resLength);
            rSpan = new(resPtr, resLength);
            return new(sPtr, desLength);
        }

        /// <summary>
        /// Retrieves the memory pointer to residual bytes for this transformation.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <param name="sPtr">Source span pointer.</param>
        /// <param name="sLength">Source span length.</param>
        /// <param name="length">Output. Principal span length.</param>
        /// <param name="rLength">Output. Residual span length.</param>
        /// <returns>Memory pointer to residual bytes for this transformation.</returns>
        private static unsafe void* GetResidualPointer<TSource, TDestination>(void* sPtr, Int32 sLength, out Int32 length, out Int32 rLength)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            Int32 byteLength = sLength * sizeof(TSource);
            Int32 typeSize = sizeof(TDestination);
            length = byteLength / typeSize;
            Int32 resOffset = length * typeSize;
            rLength = byteLength - resOffset;
            IntPtr rIntPtr = rLength != 0 ? new IntPtr(sPtr) + resOffset : IntPtr.Zero;

            return (void*)rIntPtr;
        }
    }
}
