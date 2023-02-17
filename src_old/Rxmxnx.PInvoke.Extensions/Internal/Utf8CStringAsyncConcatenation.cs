using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    /// <summary>
    /// This class helps to concatenate <see cref="CString"/> using UTF-8 encoding..
    /// </summary>
    internal sealed class Utf8CStringAsyncConcatenation :
        Utf8Concatenation<CString, Utf8CStringAsyncConcatenation.WriteDelegate>
    {
        /// <summary>
        /// Delegate for writing text into the buffer.
        /// </summary>
        /// <param name="helper"><see cref="Utf8CStringAsyncConcatenation"/> object.</param>
        /// <param name="value"></param>
        internal delegate Task WriteDelegate(Utf8CStringAsyncConcatenation helper, CString value);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="separator">Text separator.</param>
        private Utf8CStringAsyncConcatenation(CString? separator)
            : base(separator, InitalJoinAsync, ConcatAsync)
        {
        }

        /// <summary>
        /// Writes the concatenation of given text collection and given initial value into the buffer.
        /// </summary>
        /// <param name="initial">Initial CString to concatenate.</param>
        /// <param name="values">Next values.</param>
        private async Task WriteAsync(CString? initial, IEnumerable<CString?>? values)
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
                await this._write(this, value);
        }

        /// <summary>
        /// Writes the concatenation of given text collection into the buffer.
        /// </summary>
        /// <param name="values">Text collection.</param>
        private async Task WriteAsync(IEnumerable<CString?> values)
        {
            foreach (CString? value in values)
                await this.WriteAsync(value);
        }

        protected override Boolean IsEmpty([NotNullWhen(false)] CString? value) => CString.IsNullOrEmpty(value);

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
        public static Task<CString?> ConcatAsync(IEnumerable<CString?>? values, CString? initial = default)
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
        public static Task<CString?> JoinAsync(Byte separator, IEnumerable<CString?>? values)
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
        public async static Task<CString?> JoinAsync(CString? separator, IEnumerable<CString?>? values, CString? initial = default)
        {
            using Utf8CStringAsyncConcatenation helper = new(separator);
            await helper.WriteAsync(initial, values);
            return helper.ToArray(true);
        }

        /// <summary>
        /// Method for initial writing of a concatenation with separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8CStringAsyncConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private async static Task InitalJoinAsync(Utf8CStringAsyncConcatenation helper, CString value)
        {
            helper._write = Join!;
            await value.WriteAsync(helper._mem, false);
        }

        /// <summary>
        /// Method for next writing of a concatenation without separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8CStringAsyncConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private static async Task ConcatAsync(Utf8CStringAsyncConcatenation helper, CString value)
            => await value.WriteAsync(helper._mem, false);

        /// <summary>
        /// Method for next writing of a concatenation with separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8CStringAsyncConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private static async Task Join(Utf8CStringAsyncConcatenation helper, CString value)
        {
            await helper._separator!.WriteAsync(helper._mem, false);
            await value.WriteAsync(helper._mem, false);
        }
    }
}