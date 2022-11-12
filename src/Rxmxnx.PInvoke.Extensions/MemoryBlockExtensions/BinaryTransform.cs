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
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <param name="span">A binary writable memory block.</param>
        /// <param name="action">A <see cref="TransformationAction{TDestination}"/> delegate.</param>
        public static void BinaryTransform<TDestination>(this Span<Byte> span, TransformationAction<TDestination> action)
            where TDestination : unmanaged
            => span.Transform(action);

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="ReadOnlySpan{TDestination}"/> instance and 
        /// invokes <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TDestination">The type of the objects in the destination span.</typeparam>
        /// <param name="span">A binary read-only memory block.</param>
        /// <param name="action">A <see cref="ReadOnlyTransformationAction{TDestination}"/> delegate.</param>
        public static void BinaryTransform<TDestination>(this ReadOnlySpan<Byte> span, ReadOnlyTransformationAction<TDestination> action)
            where TDestination : unmanaged
            => span.Transform(action);

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="Span{Byte}"/> instance and 
        /// invokes <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> writable memory block.</param>
        /// <param name="action">A <see cref="FixedAction{Byte}"/> delegate.</param>
        public static void BinaryTransform<TSource>(this Span<TSource> span, FixedAction<Byte> action)
            where TSource : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                    action(new(ptr, span.Length * sizeof(TSource)));
            }
        }

        /// <summary>
        /// Transforms <paramref name="span"/> to a <see cref="ReadOnlySpan{Byte}"/> instance and 
        /// invokes <paramref name="action"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the objects in the source span.</typeparam>
        /// <param name="span">A <typeparamref name="TSource"/> read-only memory block.</param>
        /// <param name="action">A <see cref="ReadOnlyFixedAction{Byte}"/> delegate.</param>
        public static void BinaryTransform<TSource>(this ReadOnlySpan<TSource> span, ReadOnlyFixedAction<Byte> action)
            where TSource : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                    action(new(ptr, span.Length * sizeof(TSource)));
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
        /// <param name="action">A <see cref="TransformationAction{TDestination, TArg}"/> delegate.</param>
        public static void BinaryTransform<TDestination, TArg>(this Span<Byte> span, TArg arg, TransformationAction<TDestination, TArg> action)
            where TDestination : unmanaged
            => span.Transform(arg, action);

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
        /// <param name="action">A <see cref="ReadOnlyTransformationAction{TDestination, TArg}"/> delegate.</param>
        public static void BinaryTransform<TDestination, TArg>(this ReadOnlySpan<Byte> span, TArg arg, ReadOnlyTransformationAction<TDestination, TArg> action)
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
                    action(new(ptr, span.Length * sizeof(TSource)), arg);
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
                    action(new(ptr, span.Length * sizeof(TSource)), arg);
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
                    return func(new(ptr, span.Length * sizeof(TSource)), arg);
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
                    return func(new(ptr, span.Length * sizeof(TSource)), arg);
            }
        }
    }
}
