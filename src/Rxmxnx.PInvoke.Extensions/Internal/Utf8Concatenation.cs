using System;
using System.IO;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    /// <summary>
    /// This class helps to concatenate texts using UTF-8 encoding.
    /// </summary>
    /// <typeparam name="T">Type of text input</typeparam>
    /// <typeparam name="TWrite">Type of write delegate.</typeparam>
    internal abstract class Utf8Concatenation<T, TWrite> : IDisposable
        where T : class
        where TWrite : Delegate
    {
        /// <summary>
        /// <see cref="MemoryStream"/> used as buffer.
        /// </summary>
        protected readonly MemoryStream _mem;
        /// <summary>
        /// Text separator.
        /// </summary>
        protected readonly T? _separator;

        /// <summary>
        /// Delegate used for write text;
        /// </summary>
        protected TWrite _write;
        /// <summary>
        /// Disposed flag.
        /// </summary>
        protected Boolean _disposed;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="separator">Text separator.</param>
        /// <param name="initialJoin">Delegate for initial writing with separator.</param>
        /// <param name="concat">Delegate for writing without separator.</param>
        protected Utf8Concatenation(T? separator, TWrite initialJoin, TWrite concat)
        {
            this._mem = new();
            this._separator = separator;
            if (!IsEmpty(separator))
                this._write = initialJoin;
            else
                this._write = concat;
        }

        /// <summary>
        /// Indicates whether the specified <typeparamref name="T"/> is empty. 
        /// </summary>
        /// <param name="value"><typeparamref name="T"/> object.</param>
        /// <returns>
        /// <see langword="true"/> if the value parameter is empty; otherwise, <see langword="false"/>.
        /// </returns>
        protected abstract Boolean IsEmpty(T? value);

        /// <summary>
        /// Retrieves the binary data of UTF-8 text.
        /// </summary>
        /// <returns>Binary data of UTF-8 text</returns>
        public Byte[]? ToArray()
        {
            if (this._mem.Length > 0)
            {
                this._mem.WriteByte(default);
                return this._mem.ToArray();
            }
            return default;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting 
        /// unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!this._disposed)
            {
                this._disposed = true;
                this._mem.Dispose();
            }
        }
    }
}
