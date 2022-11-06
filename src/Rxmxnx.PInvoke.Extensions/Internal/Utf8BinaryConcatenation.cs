using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    /// <summary>
    /// This class helps to concatenate UTF-8 texts using UTF-8 encoding..
    /// </summary>
    internal sealed class Utf8BinaryConcatenation :
        Utf8Concatenation<Byte[], Utf8BinaryConcatenation.WriteDelegate>
    {
        /// <summary>
        /// Delegate for writing text into the buffer.
        /// </summary>
        /// <param name="helper"><see cref="Utf8StringAsyncConcatenation"/> object.</param>
        /// <param name="value"></param>
        internal delegate void WriteDelegate(Utf8BinaryConcatenation helper, ReadOnlySpan<Byte> value);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="separator">UTF-8 text separator.</param>
        private Utf8BinaryConcatenation(ReadOnlySpan<Byte> separator)
            : base(!separator.IsEmpty ? separator.ToArray() : default, InitalJoin, Concat)
        {
        }

        /// <summary>
        /// Writes the concatenation of given text collection into the buffer.
        /// </summary>
        /// <param name="values">UTF-8 text collection.</param>
        private void Write(IEnumerable<Byte[]?> values)
        {
            foreach (Byte[]? value in values)
                if (!this.IsEmpty(value))
                    Write(PrepareUtf8Text(value));
        }

        /// <summary>
        /// Writes a text value into the buffer.
        /// </summary>
        /// <param name="value">Text value.</param>
        private void Write(ReadOnlySpan<Byte> value)
        {
            if (!value.IsEmpty)
                this._write(this, value);
        }

        protected override Boolean IsEmpty([NotNullWhen(false)] Byte[]? value)
            => value == default || !value.Any();

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any UTF-8 text passed.
        /// </summary>
        /// <param name="values">Next values.</param>
        /// <param name="initial">Initial Byte[] to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? Concat(IEnumerable<Byte[]?>? values, Byte[]? initial = default)
            => Join(default(Byte[]), values, initial);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any UTF-8 text passed.
        /// </summary>
        /// <param name="separator">UTF-8 char used as text separator.</param>
        /// <param name="values">Next values.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? Join(Byte separator, IEnumerable<Byte[]?>? values)
            => Join(stackalloc Byte[] { separator }, values);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any UTF-8 text argument.
        /// </summary>
        /// <param name="separator">UTF-8 value used as text separator.</param>
        /// <param name="values">Next UTF-8 values.</param>
        /// <param name="initial">Initial UTF-8 text to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? Join(ReadOnlySpan<Byte> separator, IEnumerable<Byte[]?>? values, Byte[]? initial = default)
        {
            if (values != null)
            {
                using Utf8BinaryConcatenation helper = new(separator);
                helper.Write(PrepareUtf8Text(initial));
                helper.Write(values);
                return helper.ToArray();
            }
            return default;
        }

        /// <summary>
        /// Method for initial writing of a concatenation with separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8BinaryConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private static void InitalJoin(Utf8BinaryConcatenation helper, ReadOnlySpan<Byte> value)
        {
            helper._mem.Write(value);
            helper._write = Join;
        }

        /// <summary>
        /// Method for next writing of a concatenation without separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8BinaryConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private static void Concat(Utf8BinaryConcatenation helper, ReadOnlySpan<Byte> value)
            => helper._mem.Write(value);

        /// <summary>
        /// Method for next writing of a concatenation with separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8BinaryConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private static void Join(Utf8BinaryConcatenation helper, ReadOnlySpan<Byte> value)
        {
            helper._mem.Write(helper._separator);
            helper._mem.Write(value);
        }
    }
}