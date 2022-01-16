using System;
using System.Diagnostics.CodeAnalysis;
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
        protected MemoryStream _mem;
        /// <summary>
        /// Text separator.
        /// </summary>
        protected T? _separator;

        /// <summary>
        /// Delegate used for write text;
        /// </summary>
        protected TWrite _write;

        /// <summary>
        /// Disposed flag.
        /// </summary>
        private Boolean _disposed;

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
            this._write = this.GetInitialWriteMethod(separator, initialJoin, concat);
        }

        /// <summary>
        /// Destructor.
        /// </summary>
        [ExcludeFromCodeCoverage]
        ~Utf8Concatenation() => this.Dispose(false);

        /// <summary>
        /// Gets the initial value for write method delegate.
        /// </summary>
        /// <param name="separator"><typeparamref name="T"/> value.</param>
        /// <param name="initialJoin"><typeparamref name="TWrite"/> delegate for initial concatenation with separator.</param>
        /// <param name="concat"><typeparamref name="TWrite"/> delegate for concatenation without separator.</param>
        private TWrite GetInitialWriteMethod(T? separator, TWrite initialJoin, TWrite concat)
        {
            if (!IsEmpty(separator))
                return initialJoin;
            return concat;
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
        /// Performs the dispose of current instance.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
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

        /// <summary>
        /// Indicates whether the given <see cref="Byte"/> is UTF-8 null character. 
        /// </summary>
        /// <param name="utf8Char">UTF-8 character.</param>
        /// <returns>
        /// <see langword="true"/> if <see cref="Byte"/> instance is a null character; otherwise, 
        /// <see langword="false"/>.
        /// </returns>
        private static Boolean IsNullUtf8Char(in Byte utf8Char)
            => utf8Char == default;

        /// <summary>
        /// Indicates whether the given <see cref="Byte"/> sequence is UTF-8 BOM character. 
        /// </summary>
        /// <param name="utf8Char1">First UTF-8 character.</param>
        /// <param name="utf8Char2">Second UTF-8 character.</param>
        /// <param name="utf8Char3">Third UTF-8 character.</param>
        /// <returns>
        /// <see langword="true"/> if <see cref="Byte"/> sequence is a UTF-8 BOM character; otherwise, 
        /// <see langword="false"/>.
        /// </returns>
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

        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">Indicates whether the caller method is <see cref="IDisposable.Dispose()"/>.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (!this._disposed)
            {
                this.DisposeManaged(disposing);
                this._disposed = true;
            }
        }

#nullable disable
        /// <summary>
        /// Performs the dispose of managed resources of current instance.
        /// </summary>
        /// <param name="disposing">Indicates whether the caller method is <see cref="IDisposable.Dispose()"/>.</param>
        private void DisposeManaged(Boolean disposing)
        {
            if (disposing)
            {
                try
                {
                    this._mem.Dispose();
                }
                finally
                {
                    this._mem = default;
                }
                this._separator = null;
                this._write = null;
            }
        }
#nullable restore
    }
}
