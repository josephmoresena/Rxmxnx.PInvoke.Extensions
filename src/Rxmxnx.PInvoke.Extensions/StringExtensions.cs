using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Rxmxnx.PInvoke.Extensions.Internal;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Provides a set of extensions for basic operations with <see cref="String"/> and <see cref="CString"/> 
    /// instances.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Encodes the UTF-16 text using the UTF-8 charset and retrieves the read-only span which 
        /// references to UTF-8 text.
        /// </summary>
        /// <param name="str"><see cref="String"/> representation of UTF-16 text.</param>
        /// <returns>The read-only span which references to UTF-8 text.</returns>
        public static ReadOnlySpan<Byte> AsUtf8Span(this String? str) => str?.AsUtf8();

        /// <summary>
        /// Encodes the UTF-16 text using the UTF-8 charset and retrieves the <see cref="Byte"/> array with 
        /// UTF-8 text.
        /// </summary>
        /// <param name="str"><see cref="String"/> representation of UTF-16 text.</param>
        /// <returns><see cref="Byte"/> array with UTF-8 text.</returns>
        public static Byte[]? AsUtf8(this String? str) => !String.IsNullOrEmpty(str) ? Encoding.UTF8.GetBytes(str) : default;

        /// <summary>
        /// Prevents the garbage collector from relocating <paramref name="str"/> and fixes its memory 
        /// address until <paramref name="action"/> finish.
        /// </summary>
        /// <param name="str">A UTF-16 text instance.</param>
        /// <param name="action">A <see cref="ReadOnlyFixedAction{Char, TArg}"/> delegate.</param>
        public static void WithSafeFixed(this String? str, ReadOnlyFixedAction<Char> action)
        {
            unsafe
            {
                fixed (Char* ptr = str)
                {
                    if (str is null)
                        return;

                    FixedContext<Char> ctx = new(ptr, str.Length, true);
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
        /// Prevents the garbage collector from relocating <paramref name="str"/> and fixes its memory 
        /// address until <paramref name="action"/> finish.
        /// </summary>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <param name="str">A UTF-16 text instance.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="action">A <see cref="ReadOnlyFixedAction{Char, TArg}"/> delegate.</param>
        public static void WithSafeFixed<TArg>(this String? str, TArg arg, ReadOnlyFixedAction<Char, TArg> action)
        {
            unsafe
            {
                fixed (Char* ptr = str)
                {
                    if (str is null)
                        return;

                    FixedContext<Char> ctx = new(ptr, str.Length, true);
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
        /// Prevents the garbage collector from relocating <paramref name="str"/> and fixes its memory 
        /// address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="str">A UTF-16 text instance.</param>
        /// <param name="func">A <see cref="ReadOnlyFixedFunc{Char, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult? WithSafeFixed<TResult>(this String? str, ReadOnlyFixedFunc<Char, TResult> func)
        {
            unsafe
            {
                fixed (Char* ptr = str)
                {
                    if (str is null)
                        return default;

                    FixedContext<Char> ctx = new(ptr, str.Length, true);
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
        /// Prevents the garbage collector from relocating <paramref name="str"/> and fixes its memory 
        /// address until <paramref name="func"/> finish.
        /// </summary>
        /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
        /// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
        /// <param name="str">A UTF-16 text instance.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="func">A <see cref="ReadOnlyFixedFunc{Char, TArg, TResult}"/> delegate.</param>
        /// <returns>The result of <paramref name="func"/> execution.</returns>
        public static TResult? WithSafeFixed<TArg, TResult>(this String? str, TArg arg, ReadOnlyFixedFunc<Char, TArg, TResult> func)

        {
            unsafe
            {
                fixed (Char* ptr = str)
                {
                    if (str is null)
                        return default;

                    FixedContext<Char> ctx = new(ptr, str.Length, true);
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
        /// Concatenates the members of a collection of <see cref="String"/>.
        /// </summary>
        /// <param name="values">A collection that contains the strings to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? ConcatUtf8(this IEnumerable<String> values)
            => Utf8StringConcatenation.Concat(values);

        /// <summary>
        /// Concatenates the members of a collection of <see cref="String"/>.
        /// </summary>
        /// <param name="values">A collection that contains the strings to concatenate.</param>
        /// <returns>
        /// A task that represents the asynchronous concat operation. The value of the TResult
        /// parameter contains the concatenation with UTF-8 encoding.
        /// </returns>
        public static Task<Byte[]?> ConcatUtf8Async(this IEnumerable<String> values) => Utf8StringAsyncConcatenation.ConcatAsync(values);

        /// <summary>
        /// Concatenates the members of a collection of <see cref="CString"/>.
        /// </summary>
        /// <param name="values">A collection that contains the strings to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static CString? Concat(this IEnumerable<CString> values)
            => Utf8CStringConcatenation.Concat(values);

        /// <summary>
        /// Concatenates the members of a collection of <see cref="CString"/>.
        /// </summary>
        /// <param name="values">A collection that contains the strings to concatenate.</param>
        /// <returns>
        /// A task that represents the asynchronous concat operation. The value of the TResult
        /// parameter contains the concatenation with UTF-8 encoding.
        /// </returns>
        public static async Task<CString?> ConcatAsync(this IEnumerable<CString> values) => await Utf8CStringAsyncConcatenation.ConcatAsync(values);
    }
}
