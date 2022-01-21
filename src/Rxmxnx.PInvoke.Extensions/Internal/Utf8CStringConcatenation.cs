using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        internal delegate Task WriteDelegate(Utf8CStringConcatenation helper, CString value);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="separator">Text separator.</param>
        private Utf8CStringConcatenation(CString? separator)
            : base(separator, InitalJoinAsync, ConcatAsync)
        {
        }

        /// <summary>
        /// Writes the concatenation of given text collection and given initial value into the buffer.
        /// </summary>
        /// <param name="initial">Initial CString to concatenate.</param>
        /// <param name="values">Next values.</param>
        private async Task WriteAsync(CString? initial, IEnumerable<CString>? values)
        {
            await this.WriteAsync(initial);
            if (values != default)
                await this.WriteAsync(values);
        }

        /// <summary>
        /// Writes a text value into the buffer.
        /// </summary>
        /// <param name="value">Text value.</param>
        private async Task WriteAsync(CString? value)
        {
            if (!IsEmpty(value))
#pragma warning disable CS8604
                await this._write(this, value);
#pragma warning restore CS8604
        }

        /// <summary>
        /// Writes the concatenation of given text collection into the buffer.
        /// </summary>
        /// <param name="values">Texts collection.</param>
        private async Task WriteAsync(IEnumerable<CString> values)
        {
            foreach (CString value in values)
                await this.WriteAsync(value);
        }

        protected override Boolean IsEmpty(CString? value) => CString.IsNullOrEmpty(value);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="values">Next values.</param>
        /// <param name="initial">Initial CString to concatenate.</param>
        /// <returns>
        /// A task that represents the asynchronous concat operation. The value of the TResult
        /// parameter contains the concatenation with UTF-8 encoding.
        /// </returns>
        public static Task<Byte[]?> ConcatAsync(IEnumerable<CString>? values, CString? initial = default)
            => JoinAsync(default, values, initial);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="separator"><see cref="Byte"/> used as text separator.</param>
        /// <param name="values">Next values.</param>
        /// <returns>
        /// A task that represents the asynchronous join operation. The value of the TResult
        /// parameter contains the concatenation with UTF-8 encoding.
        /// </returns>
        public static Task<Byte[]?> JoinAsync(Byte separator, IEnumerable<CString>? values)
            => JoinAsync(new CString(separator, 1), values);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="separator"><see cref="CString"/> used as text separator.</param>
        /// <param name="values">Next values.</param>
        /// <param name="initial">Initial CString to concatenate.</param>
        /// <returns>
        /// A task that represents the asynchronous read operation. The value of the TResult
        /// parameter contains the concatenation with UTF-8 encoding.
        /// </returns>
        public async static Task<Byte[]?> JoinAsync(CString? separator, IEnumerable<CString>? values, CString? initial = default)
        {
            using Utf8CStringConcatenation helper = new(separator);
            await helper.WriteAsync(initial, values);
            return helper.ToArray();
        }

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="values">Next values.</param>
        /// <param name="initial">Initial CString to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? Concat(IEnumerable<CString>? values, CString? initial = default)
            => Join(default, values, initial);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="separator"><see cref="Byte"/> used as text separator.</param>
        /// <param name="values">Next values.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? Join(Byte separator, IEnumerable<CString>? values)
            => Join(new CString(separator, 1), values);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="separator"><see cref="CString"/> used as text separator.</param>
        /// <param name="values">Next values.</param>
        /// <param name="initial">Initial CString to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? Join(CString? separator, IEnumerable<CString>? values, CString? initial = default)
        {
            using Utf8CStringConcatenation helper = new(separator);
            Task.Run(() => helper.WriteAsync(initial, values)).Wait();
            return helper.ToArray();
        }

        /// <summary>
        /// Method for initial writing of a concatenation with separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8CStringConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private async static Task InitalJoinAsync(Utf8CStringConcatenation helper, CString value)
        {
            helper._write = Join;
            await value.WriteAsync(helper._mem, false);
        }

        /// <summary>
        /// Method for next writing of a concatenation without separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8CStringConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private static async Task ConcatAsync(Utf8CStringConcatenation helper, CString value)
            => await value.WriteAsync(helper._mem, false);

        /// <summary>
        /// Method for next writing of a concatenation with separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8CStringConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private static async Task Join(Utf8CStringConcatenation helper, CString value)
        {
#pragma warning disable CS8602
            await helper._separator.WriteAsync(helper._mem, false);
#pragma warning restore CS8602
            await value.WriteAsync(helper._mem, false);
        }
    }
}
