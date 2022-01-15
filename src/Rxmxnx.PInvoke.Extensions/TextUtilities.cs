using System;
using System.Collections.Generic;

using Rxmxnx.PInvoke.Extensions.Internal;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Provides a set of utilities for texts.
    /// </summary>
    public static class TextUtilities
    {
        /// <summary>
        /// Concatenates an array of strings, using the specified separator between each member.
        /// </summary>
        /// <param name="separator">
        /// The text to use as a separator. separator is included in the returned concatenation
        /// only if value has more than one element.
        /// </param>
        /// <param name="values">Next values.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? JoinUtf8(String separator, params String[] values)
            => Utf8StringConcatenation.Join(separator, values);

        /// <summary>
        /// Concatenates an array of strings, using the specified separator between each member.
        /// </summary>
        /// <param name="separator">
        /// The character to use as a separator. separator is included in the returned concatenation
        /// only if value has more than one element.
        /// </param>
        /// <param name="values">Next values.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? JoinUtf8(Char separator, params String[] values)
            => Utf8StringConcatenation.Join(separator, values);

        /// <summary>
        /// Concatenates the members of a collection of <see cref="String"/>, using the specified 
        /// separator between each member.
        /// </summary>
        /// <param name="separator"><see cref="String"/> used as text separator.</param>
        /// <param name="values">A collection that contains the strings to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? JoinUtf8(String separator, IEnumerable<String> values)
            => Utf8StringConcatenation.Join(separator, values);

        /// <summary>
        /// Concatenates the members of a collection of <see cref="String"/>, using the specified 
        /// separator between each member.
        /// </summary>
        /// <param name="separator"><see cref="Char"/> used as text separator.</param>
        /// <param name="values">A collection that contains the strings to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? JoinUtf8(Char separator, IEnumerable<String> values)
            => Utf8StringConcatenation.Join(separator, values);

        /// <summary>
        /// Concatenates an array of UTF-8 texts, using the specified separator between each member.
        /// </summary>
        /// <param name="separator">
        /// The text to use as a separator. separator is included in the returned concatenation
        /// only if value has more than one element.
        /// </param>
        /// <param name="values">Next values.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? JoinUtf8(ReadOnlySpan<Byte> separator, params Byte[][] values)
            => Utf8BinaryConcatenation.Join(separator, values);

        /// <summary>
        /// Concatenates an array of UTF-8 texts, using the specified separator between each member.
        /// </summary>
        /// <param name="separator">
        /// The Byteacter to use as a separator. separator is included in the returned concatenation
        /// only if value has more than one element.
        /// </param>
        /// <param name="values">Next values.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? JoinUtf8(Byte separator, params Byte[][] values)
            => Utf8BinaryConcatenation.Join(separator, values);

        /// <summary>
        /// Concatenates the members of a collection of UTF-8 texts, using the specified 
        /// separator between each member.
        /// </summary>
        /// <param name="separator">UTF-8 text used as text separator.</param>
        /// <param name="values">A collection that contains the UTF-8 texts to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? JoinUtf8(ReadOnlySpan<Byte> separator, IEnumerable<Byte[]> values)
            => Utf8BinaryConcatenation.Join(separator, values);

        /// <summary>
        /// Concatenates the members of a collection of UTF-8 texts, using the specified 
        /// separator between each member.
        /// </summary>
        /// <param name="separator"><see cref="Byte"/> used as text separator.</param>
        /// <param name="values">A collection that contains the UTF-8 texts to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? JoinUtf8(Byte separator, IEnumerable<Byte[]> values)
            => Utf8BinaryConcatenation.Join(separator, values);

        /// <summary>
        /// Concatenates all text parameters passed to this function.
        /// </summary>
        /// <param name="initial">Initial string to concatenate.</param>
        /// <param name="values">A collection that contains the strings to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? ConcatUtf8(String initial, params String[] values)
            => Utf8StringConcatenation.Concat(values, initial);

        /// <summary>
        /// Concatenates all UTF-8 text parameters passed to this function.
        /// </summary>
        /// <param name="initial">Initial string to concatenate.</param>
        /// <param name="values">A collection that contains the UTF-8 texts to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? ConcatUtf8(Byte[] initial, params Byte[][] values)
            => Utf8BinaryConcatenation.Concat(values, initial);
    }
}
