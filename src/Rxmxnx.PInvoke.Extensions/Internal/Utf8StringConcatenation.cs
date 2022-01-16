using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    /// <summary>
    /// This class helps to concatenate <see cref="String"/> using UTF-8 encoding..
    /// </summary>
    internal sealed class Utf8StringConcatenation :
        Utf8Concatenation<String, Utf8StringConcatenation.WriteDelegate>, IDisposable
    {
        /// <summary>
        /// Delegate for writing text into the buffer.
        /// </summary>
        /// <param name="helper"><see cref="Utf8StringConcatenation"/> object.</param>
        /// <param name="value"></param>
        internal delegate Task WriteDelegate(Utf8StringConcatenation helper, String? value);

        /// <summary>
        /// <see cref="StreamWriter"/> used as UTF-8 writer.
        /// </summary>
        private readonly StreamWriter _writer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="separator">Text separator.</param>
        private Utf8StringConcatenation(String? separator)
            : base(separator, InitalJoinAsync, ConcatAsync)
        {
            this._writer = new(this._mem, Encoding.UTF8);
            this._writer.AutoFlush = true;
        }

        /// <summary>
        /// Writes the concatenation of given text collection and given initial value into the buffer.
        /// </summary>
        /// <param name="initial">Initial string to concatenate.</param>
        /// <param name="values">Next values.</param>
        private async Task WriteAsync(String? initial, IEnumerable<String>? values)
        {
            await this.WriteAsync(initial);
            if (values != default)
                await this.WriteAsync(values);
        }

        /// <summary>
        /// Writes a text value into the buffer.
        /// </summary>
        /// <param name="value">Text value.</param>
        private async Task WriteAsync(String? value)
        {
            if (!IsEmpty(value))
                await this._write(this, value);
        }

        /// <summary>
        /// Writes the concatenation of given text collection into the buffer.
        /// </summary>
        /// <param name="values">Texts collection.</param>
        private async Task WriteAsync(IEnumerable<String> values)
        {
            foreach (String value in values)
                await this.WriteAsync(value);
        }

        protected override Boolean IsEmpty(String? value)
            => String.IsNullOrEmpty(value);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="values">Next values.</param>
        /// <param name="initial">Initial string to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Task<Byte[]?> ConcatAsync(IEnumerable<String>? values, String? initial = default)
            => JoinAsync(default, values, initial);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="separator"><see cref="Char"/> used as text separator.</param>
        /// <param name="values">Next values.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Task<Byte[]?> JoinAsync(Char separator, IEnumerable<String>? values)
            => JoinAsync(separator.ToString(), values);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="separator"><see cref="String"/> used as text separator.</param>
        /// <param name="values">Next values.</param>
        /// <param name="initial">Initial string to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public async static Task<Byte[]?> JoinAsync(String? separator, IEnumerable<String>? values, String? initial = default)
        {
            using Utf8StringConcatenation helper = new(separator);
            await helper.WriteAsync(initial, values);
            return helper.ToArray();
        }

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="values">Next values.</param>
        /// <param name="initial">Initial string to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? Concat(IEnumerable<String>? values, String? initial = default)
            => Join(default, values, initial);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="separator"><see cref="Char"/> used as text separator.</param>
        /// <param name="values">Next values.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? Join(Char separator, IEnumerable<String>? values)
            => Join(separator.ToString(), values);

        /// <summary>
        /// Creates an <see cref="Byte"/> array which contains the concatenation of any text passed 
        /// using UTF-8 encoding.
        /// </summary>
        /// <param name="separator"><see cref="String"/> used as text separator.</param>
        /// <param name="values">Next values.</param>
        /// <param name="initial">Initial string to concatenate.</param>
        /// <returns>Concatenation with UTF-8 encoding.</returns>
        public static Byte[]? Join(String? separator, IEnumerable<String>? values, String? initial = default)
        {
            if (values != null)
            {
                using Utf8StringConcatenation helper = new(separator);
                Task.Run(() => helper.WriteAsync(initial, values)).Wait();
                return helper.ToArray();
            }
            return default;
        }

        /// <summary>
        /// Method for initial writing of a concatenation with separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8StringConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private async static Task InitalJoinAsync(Utf8StringConcatenation helper, String? value)
        {
            helper._write = Join;
            await helper._writer.WriteAsync(value).ConfigureAwait(false);
        }

        /// <summary>
        /// Method for next writing of a concatenation without separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8StringConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private static async Task ConcatAsync(Utf8StringConcatenation helper, String? value)
            => await helper._writer.WriteAsync(value).ConfigureAwait(false);

        /// <summary>
        /// Method for next writing of a concatenation with separator.
        /// </summary>
        /// <param name="helper"><see cref="Utf8StringConcatenation"/></param>
        /// <param name="value">Text to write.</param>
        private static async Task Join(Utf8StringConcatenation helper, String? value)
        {
            await helper._writer.WriteAsync(helper._separator).ConfigureAwait(false);
            await helper._writer.WriteAsync(value).ConfigureAwait(false);
        }

        void IDisposable.Dispose()
        {
            if (!this._disposed)
                this._writer.Dispose();
            base.Dispose();
        }
    }
}
