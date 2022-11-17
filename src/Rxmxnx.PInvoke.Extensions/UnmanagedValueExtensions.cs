using System;
using System.Linq;

using Rxmxnx.PInvoke.Extensions.Internal;

namespace Rxmxnx.PInvoke.Extensions
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
        public static Byte[] AsBytes<T>(this T value) where T : unmanaged
            => NativeUtilities.AsBytes(value);

        /// <summary>
        /// Creates an array of <typeparamref name="TDestination"/> values from an array of 
        /// <typeparamref name="TSource"/> values. 
        /// </summary>
        /// <typeparam name="TDestination">Destination <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
        /// <param name="array">Array of <typeparamref name="TSource"/> values.</param>
        /// <returns>Array of <typeparamref name="TDestination"/> values.</returns>
        public static TDestination[]? AsValues<TSource, TDestination>(this TSource[] array)
            where TSource : unmanaged
            where TDestination : unmanaged
        {
            if (array == default)
                return default;

            if (!array.Any())
                return Array.Empty<TDestination>();

            return new ArrayValueConversion<TSource, TDestination>(array);
        }

        /// <summary>
        /// Prevents the garbage collector from relocating <paramref name="array"/> and fixes its memory 
        /// address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the array.</typeparam>
        /// <param name="array">An array of objects of type <typeparamref name="T"/>.</param>
        /// <param name="action">A <see cref="FixedAction{T}"/> delegate.</param>
        public static void WithSafeFixed<T>(this T[]? array, FixedAction<T> action)
            where T : unmanaged
        {
            if (array is not null)
                unsafe
                {
                    fixed (void* ptr = array)
                    {
                        FixedContext<T> ctx = new(ptr, array.Length);
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
        /// Prevents the garbage collector from relocating <paramref name="array"/> and fixes its memory 
        /// address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the array.</typeparam>
        /// <param name="array">An array of objects of type <typeparamref name="T"/>.</param>
        /// <param name="action">A <see cref="ReadOnlyFixedAction{T, TArg}"/> delegate.</param>
        public static void WithSafeFixed<T>(this T[]? array, ReadOnlyFixedAction<T> action)
            where T : unmanaged
        {
            if (array is not null)
                unsafe
                {
                    fixed (void* ptr = array)
                    {
                        FixedContext<T> ctx = new(ptr, array.Length, true);
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
        /// Prevents the garbage collector from relocating <paramref name="array"/> and fixes its memory 
        /// address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the array.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <param name="array">An array of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="action">A <see cref="FixedAction{T, TArg}"/> delegate.</param>
        public static void WithSafeFixed<T, TArg>(this T[]? array, TArg arg, FixedAction<T, TArg> action)
            where T : unmanaged
        {
            if (array is not null)
                unsafe
                {
                    fixed (void* ptr = array)
                    {
                        FixedContext<T> ctx = new(ptr, array.Length);
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
        /// Prevents the garbage collector from relocating <paramref name="array"/> and fixes its memory 
        /// address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the array.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <param name="array">An array of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="action">A <see cref="ReadOnlyFixedAction{T, TArg}"/> delegate.</param>
        public static void WithSafeFixed<T, TArg>(this T[]? array, TArg arg, ReadOnlyFixedAction<T, TArg> action)
            where T : unmanaged
        {
            if (array is not null)
                unsafe
                {
                    fixed (void* ptr = array)
                    {
                        FixedContext<T> ctx = new(ptr, array.Length, true);
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
        /// Prevents the garbage collector from relocating <paramref name="array"/> and fixes its memory 
        /// address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the array.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="array">An array of objects of type <typeparamref name="T"/>.</param>
        /// <param name="func">A <see cref="FixedFunc{T, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult? WithSafeFixed<T, TResult>(this T[]? array, FixedFunc<T, TResult> func)
            where T : unmanaged
        {
            if (array is not null)
                unsafe
                {
                    fixed (void* ptr = array)
                    {
                        FixedContext<T> ctx = new(ptr, array.Length);
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
            return default;
        }

        /// <summary>
        /// Prevents the garbage collector from relocating <paramref name="array"/> and fixes its memory 
        /// address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the array.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="array">An array of objects of type <typeparamref name="T"/>.</param>
        /// <param name="func">A <see cref="ReadOnlyFixedFunc{T, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult? WithSafeFixed<T, TResult>(this T[]? array, ReadOnlyFixedFunc<T, TResult> func)
            where T : unmanaged
        {
            if (array is not null)
                unsafe
                {
                    fixed (void* ptr = array)
                    {
                        FixedContext<T> ctx = new(ptr, array.Length, true);
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
            return default;
        }

        /// <summary>
        /// Prevents the garbage collector from relocating <paramref name="array"/> and fixes its memory 
        /// address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the array.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="array">An array of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="func">A <see cref="FixedFunc{T, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult? WithSafeFixed<T, TArg, TResult>(this T[]? array, TArg arg, FixedFunc<T, TArg, TResult> func)
            where T : unmanaged
        {
            if (array is not null)
                unsafe
                {
                    fixed (void* ptr = array)
                    {
                        FixedContext<T> ctx = new(ptr, array.Length);
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
            return default;
        }

        /// <summary>
        /// Prevents the garbage collector from relocating <paramref name="array"/> and fixes its memory 
        /// address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="T">The type of the objects in the array.</typeparam>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="array">An array of objects of type <typeparamref name="T"/>.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="func">A <see cref="ReadOnlyFixedFunc{T, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult? WithSafeFixed<T, TArg, TResult>(this T[]? array, TArg arg, ReadOnlyFixedFunc<T, TArg, TResult> func)
            where T : unmanaged
        {
            if (array is not null)
                unsafe
                {
                    fixed (void* ptr = array)
                    {
                        FixedContext<T> ctx = new(ptr, array.Length, true);
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
            return default;
        }
    }
}
