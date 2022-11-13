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
        /// <typeparam name="TArg">Type of state object.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> writable memory block.</param>
        /// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
        /// <param name="func">A <see cref="SpanTransformFunc{TDestination, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
#pragma warning disable S2436
        public static TResult Transform<TSource, TDestination, TArg, TResult>(this Span<TSource> span, TArg arg, SpanTransformFunc<TDestination, TArg, TResult> func)
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
        /// <param name="func">A <see cref="ReadOnlySpanTransformFunc{TDestination, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult Transform<TSource, TDestination, TArg, TResult>(this ReadOnlySpan<TSource> span, TArg arg, ReadOnlySpanTransformFunc<TDestination, TArg, TResult> func)
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
