using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Rxmxnx.PInvoke.Extensions
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
        /// Transforms <paramref name="span"/> to a <see cref="Span{TDestination}"/> instance and 
        /// invokes <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> writable memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="action">A <see cref="SpanTransformAction{TDestination, TArg}"/> delegate.</param>
        public static void Transform<TSource, TDestination, TArg>(this Span<TSource> span, TArg arg, SpanTransformAction<TDestination, TArg> action)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    Span<TDestination> destinationSpan =
                        GetDestinationSpan<TSource, TDestination>(ptr, span.Length, out Span<Byte> residueSpan);
                    action(destinationSpan, arg, residueSpan);
                }
            }
        }

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="ReadOnlySpan{TDestination}"/> instance and 
        /// invokes <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> read-only memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="action">A <see cref="ReadOnlySpanTransformAction{TDestination, TArg}"/> delegate.</param>
        public static void Transform<TSource, TDestination, TArg>(this ReadOnlySpan<TSource> span, TArg arg, ReadOnlySpanTransformAction<TDestination, TArg> action)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    Span<TDestination> destinationSpan =
                        GetDestinationSpan<TSource, TDestination>(ptr, span.Length, out Span<Byte> residueSpan);
                    action(destinationSpan, arg, residueSpan);
                }
            }
        }

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="Span{TDestination}"/> instance and 
        /// invokes <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> writable memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="func">A <see cref="SpanTransfromFunc{TDestination, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult Transform<TSource, TDestination, TArg, TResult>(this Span<TSource> span, TArg arg, SpanTransfromFunc<TDestination, TArg, TResult> func)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    Span<TDestination> destinationSpan =
                        GetDestinationSpan<TSource, TDestination>(ptr, span.Length, out Span<Byte> residueSpan);
                    return func(destinationSpan, arg, residueSpan);
                }
            }
        }

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="ReadOnlySpan{TDestination}"/> instance and 
        /// invokes <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> read-only memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="func">A <see cref="ReadOnlySpanTransfromFunc{TDestination, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult Transform<TSource, TDestination, TArg, TResult>(this ReadOnlySpan<TSource> span, TArg arg, ReadOnlySpanTransfromFunc<TDestination, TArg, TResult> func)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    Span<TDestination> destinationSpan =
                        GetDestinationSpan<TSource, TDestination>(ptr, span.Length, out Span<Byte> residueSpan);
                    return func(destinationSpan, arg, residueSpan);
                }
            }
        }

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="Span{TDestination}"/> instance and 
        /// invokes <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <param name="span">A binary writable memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="action">A <see cref="SpanTransformAction{TDestination, TArg}"/> delegate.</param>
        public static void BinaryTransform<TDestination, TArg>(this Span<Byte> span, TArg arg, SpanTransformAction<TDestination, TArg> action)
            where TDestination : unmanaged
            => span.Transform(arg, action);

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="ReadOnlySpan{TDestination}"/> instance and 
        /// invokes <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <param name="span">A binary read-only memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="action">A <see cref="ReadOnlySpanTransformAction{TDestination, TArg}"/> delegate.</param>
        public static void BinaryTransform<TDestination, TArg>(this ReadOnlySpan<Byte> span, TArg arg, ReadOnlySpanTransformAction<TDestination, TArg> action)
            where TDestination : unmanaged
            => span.Transform(arg, action);

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="Span{Byte}"/> instance and 
        /// invokes <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> writable memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="action">A <see cref="BinarySpanTransformAction{TArg}"/> delegate.</param>
        public static void BinaryTransform<TSource, TArg>(this Span<TSource> span, TArg arg, BinarySpanTransformAction<TArg> action)
            where TSource : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                    action(new(ptr, span.Length / sizeof(TSource)), arg);
            }
        }

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="ReadOnlySpan{Byte}"/> instance and 
        /// invokes <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> read-only memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="action">A <see cref="BinaryReadOnlySpanTransformAction{TArg}"/> delegate.</param>
        public static void BinaryTransform<TSource, TArg>(this ReadOnlySpan<TSource> span, TArg arg, BinaryReadOnlySpanTransformAction<TArg> action)
            where TSource : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                    action(new(ptr, span.Length / sizeof(TSource)), arg);
            }
        }

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="Span{TDestination}"/> instance and 
        /// invokes <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A binary writable memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="func">A <see cref="SpanTransfromFunc{TDestination, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult BinaryTransform<TDestination, TArg, TResult>(this Span<Byte> span, TArg arg, SpanTransfromFunc<TDestination, TArg, TResult> func)
            where TDestination : unmanaged
            => span.Transform(arg, func);

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="ReadOnlySpan{TDestination}"/> instance and 
        /// invokes <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A binary read-only memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="func">A <see cref="ReadOnlySpanTransfromFunc{TDestination, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult BinaryTransform<TDestination, TArg, TResult>(this ReadOnlySpan<Byte> span, TArg arg, ReadOnlySpanTransfromFunc<TDestination, TArg, TResult> func)
            where TDestination : unmanaged
            => span.Transform(arg, func);

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="Span{Byte}"/> instance and 
        /// invokes <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> writable memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="func">A <see cref="BinarySpanTransfromFunc{TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult BinaryTransform<TSource, TArg, TResult>(this Span<TSource> span, TArg arg, BinarySpanTransfromFunc<TArg, TResult> func)
            where TSource : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                    return func(new(ptr, span.Length / sizeof(TSource)), arg);
            }
        }

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="ReadOnlySpan{Byte}"/> instance and 
        /// invokes <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> read-only memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="func">A <see cref="BinaryReadOnlySpanTransfromFunc{TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult BinaryTransform<TSource, TArg, TResult>(this ReadOnlySpan<TSource> span, TArg arg, BinaryReadOnlySpanTransfromFunc<TArg, TResult> func)
            where TSource : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                    return func(new(ptr, span.Length / sizeof(TSource)), arg);
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
        /// Retrieves a <typeparamref name="TDestination"/> span from <paramref name="srcPtr"/> which references
        /// to a <typeparamref name="TSource"/> span whose length is <paramref name="srcPtr"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <param name="srcPtr">Source span pointer.</param>
        /// <param name="srcInput">Source span length.</param>
        /// <param name="residueSpan">Output. The residual span of bytes.</param>
        /// <returns>Destination span.</returns>
        private static unsafe Span<TDestination> GetDestinationSpan<TSource, TDestination>(void* srcPtr, Int32 srcInput, out Span<Byte> residueSpan)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            Int32 byteLength = srcInput * sizeof(TSource);
            Int32 typeSize = sizeof(TDestination);
            Int32 desLength = byteLength / typeSize;
            Int32 resOffset = desLength * typeSize;
            Int32 resLength = byteLength - resOffset;
            IntPtr resPtr = resLength != 0 ? new IntPtr(srcPtr) + resOffset : IntPtr.Zero;

            residueSpan = new(resPtr.ToPointer(), resLength);
            return new(srcPtr, desLength);
        }
    }
}
