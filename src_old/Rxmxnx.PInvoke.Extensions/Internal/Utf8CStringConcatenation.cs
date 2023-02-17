using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    /// <summary>
    /// This class helps to concatenate <see cref="CString"/> using UTF-8 encoding..
    /// </summary>
    internal sealed class Utf8CStringConcatenation :
        Utf8Concatenation<CString, Utf8CStringConcatenation.WriteDelegate>
    {
        /// <summary>
        /// Delegate for writing text into the buffer.
        /// </summary>
        /// <param name="helper"><see cref="Utf8CStringConcatenation"/> object.</param>
        /// <param name="value"></param>
        internal delegate void WriteDelegate(Utf8CStringConcatenation helper, CString value);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="separator">Text separator.</param>
        private Utf8CStringConcatenation(CString? separator)
            : base(separator, InitalJoin, Concat)
        {
        }

        /// <summary>
        /// Writes the concatenation of given text collection and given initial value into the buffer.
        /// </summary>
        /// <param name="initial">Initial CString to concatenate.</param>
        /// <param name="values">Next values.</param>
        private void Write(CString? initial, IEnumerable<CString?>? values)
        {
            this.Write(initial);
            if (values != default)
                this.Write(values);
        }

        /// <summary>
        /// Writes a text value into the buffer.
        /// </summary>
        /// <param name="value">Text value.</param>
        private void Write(CString? value)
        {
            if (!IsEmpty(value))
                this._write(this, value);
        }

        /// <summary>
        /// Writes the concatenation of given text collection into the buffer.
        /// </summary>
        /// <param name="values">Text collection.</param>
        private void Write(IEnumerable<CString?> values)
        {
            foreach (CString? value in values)
                this.Write(value);
        }

        protected override Boolean IsEmpty([NotNullWhen(false)] CString? value) => CString.IsNullOrEmpty(value);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="values">Next values.</param>
        /// <param name="initial">Initial CString to concatenate.</param>
        /// <returns>The concatenation with UTF-8 encoding.</returns>
        public static CString? Concat(IEnumerable<CString?>? values, CString? initial = default)
            => Join(default, values, initial);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="separator"><see cref="Byte"/> used as text separator.</param>
        /// <param name="values">Next values.</param>
        /// <returns>The concatenation with UTF-8 encoding.</returns>
        public static CString? Join(Byte separator, IEnumerable<CString?>? values)
            => Join(new CString(separator, 1), values);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="separator"><see cref="CString"/> used as text separator.</param>
        /// <param name="values">Next values.</param>
        /// <param name="initial">Initial CString to concatenate.</param>
        /// <returns>The concatenation with UTF-8 encoding.</returns>
        public static CString? Join(CString? separator, IEnumerable<CString?>? values, CString? initial = default)
        {
            using Utf8CStringConcatenation helper = new(separator);
            helper.Write(initial, values);
            return helper.ToArray(true);
        }

        /// <summary>
        /// Method for initial writing of a concatenation with separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8CStringConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private static void InitalJoin(Utf8CStringConcatenation helper, CString value)
        {
            helper._write = Join;
            value.Write(helper._mem, false);
        }

        /// <summary>
        /// Method for next writing of a concatenation without separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8CStringConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private static void Concat(Utf8CStringConcatenation helper, CString value)
            => value.Write(helper._mem, false);

        /// <summary>
        /// Method for next writing of a concatenation with separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8CStringConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private static void Join(Utf8CStringConcatenation helper, CString value)
        {
            helper._separator!.Write(helper._mem, false);
            value.Write(helper._mem, false);
        }
    }
}
