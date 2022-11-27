using System;
using System.Buffers;
using System.Runtime.InteropServices;

using Rxmxnx.PInvoke.Extensions.Internal;

namespace Rxmxnx.PInvoke.Extensions
{
    public static partial class MemoryBlockExtensions
    {
        /// <summary>
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="action">A <see cref="FixedAction{T}"/> delegate.</param>
        public static void WithSafeFixed<T>(this Span<T> span, FixedAction<T> action)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    FixedContext<T> ctx = new(ptr, span.Length);
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
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="action">A <see cref="ReadOnlyFixedAction{T, TArg}"/> delegate.</param>
        public static void WithSafeFixed<T>(this Span<T> span, ReadOnlyFixedAction<T> action)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    FixedContext<T> ctx = new(ptr, span.Length, true);
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
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <param name="span">A read-only span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="action">A <see cref="ReadOnlyFixedAction{T, TArg}"/> delegate.</param>
        public static void WithSafeFixed<T>(this ReadOnlySpan<T> span, ReadOnlyFixedAction<T> action)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    FixedContext<T> ctx = new(ptr, span.Length, true);
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
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="action">A <see cref="FixedAction{T, TArg}"/> delegate.</param>
        public static void WithSafeFixed<T, TArg>(this Span<T> span, TArg arg, FixedAction<T, TArg> action)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    FixedContext<T> ctx = new(ptr, span.Length);
                    try
                    {
                        action(ctx, arg);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
                }
            }
        }

        /// <summary>
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="action">A <see cref="SpanAction{T, TArg}"/> delegate.</param>
        public static void WithSafeFixed<T, TArg>(this Span<T> span, TArg arg, SpanAction<T, TArg> action)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                    action(new(ptr, span.Length), arg);
            }
        }

        /// <summary>
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="action">A <see cref="ReadOnlyFixedAction{T, TArg}"/> delegate.</param>
        public static void WithSafeFixed<T, TArg>(this Span<T> span, TArg arg, ReadOnlyFixedAction<T, TArg> action)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    FixedContext<T> ctx = new(ptr, span.Length, true);
                    try
                    {
                        action(ctx, arg);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
                }
            }
        }

        /// <summary>
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <param name="span">A read-only span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="action">A <see cref="ReadOnlyFixedAction{T, TArg}"/> delegate.</param>
        public static void WithSafeFixed<T, TArg>(this ReadOnlySpan<T> span, TArg arg, ReadOnlyFixedAction<T, TArg> action)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    FixedContext<T> ctx = new(ptr, span.Length, true);
                    try
                    {
                        action(ctx, arg);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
                }
            }
        }

        /// <summary>
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <param name="span">A read-only span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="action">A <see cref="ReadOnlySpanAction{T, TArg}"/> delegate.</param>
        public static void WithSafeFixed<T, TArg>(this ReadOnlySpan<T> span, TArg arg, ReadOnlySpanAction<T, TArg> action)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                    action(new(ptr, span.Length), arg);
            }
        }

        /// <summary>
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="func">A <see cref="FixedFunc{T, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult WithSafeFixed<T, TResult>(this Span<T> span, FixedFunc<T, TResult> func)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    FixedContext<T> ctx = new(ptr, span.Length);
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
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="func">A <see cref="ReadOnlyFixedFunc{T, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult WithSafeFixed<T, TResult>(this Span<T> span, ReadOnlyFixedFunc<T, TResult> func)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    FixedContext<T> ctx = new(ptr, span.Length, true);
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
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A read-only span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="func">A <see cref="ReadOnlyFixedFunc{T, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult WithSafeFixed<T, TResult>(this ReadOnlySpan<T> span, ReadOnlyFixedFunc<T, TResult> func)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    FixedContext<T> ctx = new(ptr, span.Length, true);
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
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="func">A <see cref="FixedFunc{T, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult WithSafeFixed<T, TArg, TResult>(this Span<T> span, TArg arg, FixedFunc<T, TArg, TResult> func)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    FixedContext<T> ctx = new(ptr, span.Length);
                    try
                    {
                        return func(ctx, arg);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
                }
            }
        }

        /// <summary>
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="func">A <see cref="ReadOnlyFixedFunc{T, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult WithSafeFixed<T, TArg, TResult>(this Span<T> span, TArg arg, ReadOnlyFixedFunc<T, TArg, TResult> func)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    FixedContext<T> ctx = new(ptr, span.Length, true);
                    try
                    {
                        return func(ctx, arg);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
                }
            }
        }

        /// <summary>
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="func">A <see cref="SpanFunc{T, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult WithSafeFixed<T, TArg, TResult>(this Span<T> span, TArg arg, SpanFunc<T, TArg, TResult> func)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                    return func(new(ptr, span.Length), arg);
            }
        }

        /// <summary>
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A read-only span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="func">A <see cref="ReadOnlyFixedFunc{T, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult WithSafeFixed<T, TArg, TResult>(this ReadOnlySpan<T> span, TArg arg, ReadOnlyFixedFunc<T, TArg, TResult> func)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                {
                    FixedContext<T> ctx = new(ptr, span.Length, true);
                    try
                    {
                        return func(ctx, arg);
                    }
                    finally
                    {
                        ctx.Unload();
                    }
                }
            }
        }

        /// <summary>
        /// Prevents the garbage collector from relocating the block of memory represented by 
        /// <paramref name="span"/> and fixes its memory address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the span.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="span">A read-only span of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="func">A <see cref="ReadOnlySpanFunc{T, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult WithSafeFixed<T, TArg, TResult>(this ReadOnlySpan<T> span, TArg arg, ReadOnlySpanFunc<T, TArg, TResult> func)
            where T : unmanaged
        {
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(span))
                    return func(new(ptr, span.Length), arg);
            }
        }
    }
}
