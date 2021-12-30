using System;
using System.Text;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Provides a set of extensions for basic operations with <see cref="String"/> instances.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Encodes the UTF-16 text using the UTF-8 charset and retrieves the read-only span which 
        /// references to the UTF-8 text.
        /// </summary>
        /// <param name="str"><see cref="String"/> representation of UTF-16 text.</param>
        /// <returns>The read-only span which references to the UTF-8 text.</returns>
        public static ReadOnlySpan<Byte> AsUtf8Span(this String str)
            => !String.IsNullOrEmpty(str) ? Encoding.UTF8.GetBytes(str).AsSpan() : default;
    }
}
