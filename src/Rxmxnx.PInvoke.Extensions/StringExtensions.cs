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
        public static ReadOnlySpan<Byte> AsUtf8Span(this String? str)
            => str?.AsUtf8();

        /// <summary>
        /// Encodes the UTF-16 text using the UTF-8 charset and retrieves the <see cref="Byte"/> array with 
        /// UTF-8 text.
        /// </summary>
        /// <param name="str"><see cref="String"/> representation of UTF-16 text.</param>
        /// <returns><see cref="Byte"/> array with UTF-8 text.</returns>
        public static Byte[]? AsUtf8(this String? str)
            => !String.IsNullOrEmpty(str) ? Encoding.UTF8.GetBytes(str) : default;

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
        public static Task<Byte[]?> ConcatUtf8Async(this IEnumerable<String> values)
            => Utf8StringConcatenation.ConcatAsync(values);

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
        public static async Task<CString?> ConcatAsync(this IEnumerable<CString> values)
            => await Utf8CStringConcatenation.ConcatAsync(values);
    }
}
