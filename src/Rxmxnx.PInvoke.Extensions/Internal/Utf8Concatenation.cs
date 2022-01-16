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
                return this.GetBinaryData();
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

        /// <summary>
        /// Retrieves a copy of binary data into the buffer.
        /// </summary>
        /// <returns>Copy of binary data into the buffer.</returns>
        private Byte[]? GetBinaryData()
        {
            ReadOnlySpan<Byte> span = PrepareUtf8Text(this._mem.GetBuffer());
            if (!span.IsEmpty)
                return span.ToArray();
            return default;
        }

        /// <summary>
        /// Prepares a UTF-8 text for concatenation process.
        /// </summary>
        /// <param name="span"><see cref="ReadOnlySpan{Byte}"/> to UTF-8 text.</param>
        /// <returns><see cref="ReadOnlySpan{Byte}"/> to UTF-8 binary data.</returns>
        protected static ReadOnlySpan<Byte> PrepareUtf8Text(ReadOnlySpan<Byte> span)
        {
            if (!span.IsEmpty)
            {
                Int32 iPosition = GetInitialPosition(span);
                Int32 fLength = GetFinalLength(span, iPosition);
                return span.Slice(iPosition, fLength);
            }
            return span;
        }

        /// <summary>
        /// Gets the initial position of the UTF-8 text.
        /// </summary>
        /// <param name="span"><see cref="ReadOnlySpan{Byte}"/> to UTF-8 text.</param>
        /// <returns>Initial position of the UTF-8 text.</returns>
        private static Int32 GetInitialPosition(ReadOnlySpan<Byte> span)
        {
            Int32 iPosition = 0;
            while (iPosition < span.Length && IsNullUtf8Char(span[iPosition]))
                iPosition++;
            while (iPosition + 2 < span.Length && IsBOMChar(span[iPosition], span[iPosition + 1], span[iPosition + 2]))
                iPosition += 3;
            return iPosition;
        }

        private static Boolean IsNullUtf8Char(in Byte utf8Char)
            => utf8Char == default;

        private static Boolean IsBOMChar(in Byte utf8Char1, in Byte utf8Char2, in Byte utf8Char3)
            => utf8Char1 == 239 && utf8Char2 == 187 && utf8Char3 == 191;

        /// <summary>
        /// Gets the final length of the UTF-8 text.
        /// </summary>
        /// <param name="span"><see cref="ReadOnlySpan{Byte}"/> to UTF-8 text.</param>
        /// <param name="iPosition">Initial position of the UTF-8 text.</param>
        /// <returns>Final length of the UTF-8 text.</returns>
        private static Int32 GetFinalLength(ReadOnlySpan<Byte> span, Int32 iPosition)
        {
            Int32 fPosition = span.Length - 1;
            while (fPosition >= iPosition && IsNullUtf8Char(span[fPosition]))
                fPosition--;
            return fPosition - iPosition + 1;
        }
    }
}
