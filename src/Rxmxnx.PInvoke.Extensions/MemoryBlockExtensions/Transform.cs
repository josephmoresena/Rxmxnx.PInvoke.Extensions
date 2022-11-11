using System;
using System.Runtime.InteropServices;

namespace Rxmxnx.PInvoke.Extensions
{
    public static partial class MemoryBlockExtensions
    {
        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="Span{TDestination}"/> instance and 
        /// invokes <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> writable memory block.</param>
        /// <param name="action">A <see cref="SpanTransformAction{TDestination, TArg}"/> delegate.</param>
        public static void Transform<TSource, TDestination>(this Span<TSource> span, TransformationAction<TDestination> action)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    void* rPtr = GetResidualPointer<TSource, TDestination>(ptr, span.Length, out Int32 length, out Int32 rLength);
                    TransformationContext<TDestination> ctx = new(ptr, length, rPtr, rLength);
                    try
                    {
                        action(ctx);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
                }
            }
        }

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="ReadOnlySpan{TDestination}"/> instance and 
        /// invokes <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> read-only memory block.</param>
        /// <param name="action">A <see cref="ReadOnlySpanTransformAction{TDestination, TArg}"/> delegate.</param>
        public static void Transform<TSource, TDestination>(this ReadOnlySpan<TSource> span, ReadOnlyTransformationAction<TDestination> action)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    void* rPtr = GetResidualPointer<TSource, TDestination>(ptr, span.Length, out Int32 length, out Int32 rLength);
                    ReadOnlyTransformationContext<TDestination> ctx = new(ptr, length, rPtr, rLength);
                    try
                    {
                        action(ctx);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
                }
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
        public static void Transform<TSource, TDestination, TArg>(this Span<TSource> span, TArg arg, TransformationAction<TDestination, TArg> action)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    void* rPtr = GetResidualPointer<TSource, TDestination>(ptr, span.Length, out Int32 length, out Int32 rLength);
                    TransformationContext<TDestination, TArg> ctx = new(ptr, length, rPtr, rLength, arg);
                    try
                    {
                        action(ctx);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
                }
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
        public static void Transform<TSource, TDestination, TArg>(this ReadOnlySpan<TSource> span, TArg arg, ReadOnlyTransformationAction<TDestination, TArg> action)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    void* rPtr = GetResidualPointer<TSource, TDestination>(ptr, span.Length, out Int32 length, out Int32 rLength);
                    ReadOnlyTransformationContext<TDestination, TArg> ctx = new(ptr, length, rPtr, rLength, arg);
                    try
                    {
                        action(ctx);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
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
                    ReadOnlySpan<TDestination> destinationSpan =
                        GetDestinationSpan<TSource, TDestination>(ptr, span.Length, out ReadOnlySpan<Byte> residueSpan);
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
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> writable memory block.</param>
        /// <param name="func">A <see cref="TransformationFunc{TDestination, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult Transform<TSource, TDestination, TResult>(this Span<TSource> span, TransformationFunc<TDestination, TResult> func)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    void* rPtr = GetResidualPointer<TSource, TDestination>(ptr, span.Length, out Int32 length, out Int32 rLength);
                    TransformationContext<TDestination> ctx = new(ptr, length, rPtr, rLength);
                    try
                    {
                        return func(ctx);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
                }
            }
        }

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="ReadOnlySpan{TDestination}"/> instance and 
        /// invokes <paramref name="func"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> read-only memory block.</param>
        /// <param name="func">A <see cref="ReadOnlyTransformationFunc{TDestination, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult Transform<TSource, TDestination, TResult>(this ReadOnlySpan<TSource> span, ReadOnlyTransformationFunc<TDestination, TResult> func)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    void* rPtr = GetResidualPointer<TSource, TDestination>(ptr, span.Length, out Int32 length, out Int32 rLength);
                    ReadOnlyTransformationContext<TDestination> ctx = new(ptr, length, rPtr, rLength);
                    try
                    {
                        return func(ctx);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
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
        /// <param name="func">A <see cref="TransformationFunc{TDestination, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
#pragma warning disable S2436
        public static TResult Transform<TSource, TDestination, TArg, TResult>(this Span<TSource> span, TArg arg, TransformationFunc<TDestination, TArg, TResult> func)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    void* rPtr = GetResidualPointer<TSource, TDestination>(ptr, span.Length, out Int32 length, out Int32 rLength);
                    TransformationContext<TDestination, TArg> ctx = new(ptr, length, rPtr, rLength, arg);
                    try
                    {
                        return func(ctx);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
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
        /// <param name="func">A <see cref="ReadOnlyTransformationFunc{TDestination, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult Transform<TSource, TDestination, TArg, TResult>(this ReadOnlySpan<TSource> span, TArg arg, ReadOnlyTransformationFunc<TDestination, TArg, TResult> func)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    void* rPtr = GetResidualPointer<TSource, TDestination>(ptr, span.Length, out Int32 length, out Int32 rLength);
                    ReadOnlyTransformationContext<TDestination, TArg> ctx = new(ptr, length, rPtr, rLength, arg);
                    try
                    {
                        return func(ctx);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
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
#pragma warning restore S2436
                where TSource : unmanaged
                where TDestination : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    ReadOnlySpan<TDestination> destinationSpan =
                        GetDestinationSpan<TSource, TDestination>(ptr, span.Length, out ReadOnlySpan<Byte> residueSpan);
                    return func(destinationSpan, arg, residueSpan);
                }
            }
        }
    }
}
