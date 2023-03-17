namespace Rxmxnx.PInvoke.Internal
{
    /// <summary>
    /// Class helper for UTF-8 text concatenation
    /// </summary>
    internal abstract partial class Utf8ConcatenationHelper<T> : IDisposable, IAsyncDisposable
    {
        /// <summary>
        /// Delegate. Indicates whether <paramref name="value"/> is empty.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="value"/> is empty; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        protected delegate Boolean IsEmptyDelegate([NotNullWhen(false)] T? value);

        /// <summary>
        /// Current instance stream.
        /// </summary>
        protected MemoryStream Stream => this._mem;
        /// <summary>
        /// Current instance writer.
        /// </summary>
        protected StreamWriter Writer => this._writer;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="separator">Separator for concatenation.</param>
        protected Utf8ConcatenationHelper(T? separator, IsEmptyDelegate isEmpty)
        {
            this._mem = new();
            this._writer = new(this._mem, Encoding.UTF8) { AutoFlush = true };
            this._separator = separator;
            this._isEmpty = isEmpty;
            this.InitializeDelegates();
        }

        /// <summary>
        /// Writes <paramref name="value"/> into the current instance.
        /// </summary>
        /// <param name="value">UTF-8 bytes to write.</param>
        public void Write(ReadOnlySpan<Byte> value) => this._binaryWrite(value);

        /// <summary>
        /// Writes <paramref name="value"/> into the current instance.
        /// </summary>
        /// <param name="value">Value to write to.</param>
        public void Write(T? value) => this._write(value);

        /// <summary>
        /// Asynchronously writes <paramref name="value"/> into the current instance.
        /// </summary>
        /// <param name="value">Value to write to.</param>
        /// <returns>A taks that represents the asynchronous write operation.</returns>
        private Task WriteAsync(T? value) => this._writeAsync(value);

        /// <inheritdoc/>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await this.DisposeAsync(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Writes <paramref name="value"/> in current instance.
        /// </summary>
        /// <param name="value">Value to write.</param>
        protected abstract void WriteValue(T value);

        /// <summary>
        /// Asynchronously writes <paramref name="value"/> in current instance.
        /// </summary>
        /// <param name="value">Value to write.</param>
        /// <returns>A taks that represents the asynchronous write operation.</returns>
        protected abstract Task WriteValueAsync(T value);

        /// <summary>
        /// Releases the resources of current instance.
        /// </summary>
        /// <param name="disposing">Indicates whether the caller method is <see cref="IDisposable.Dispose()"/>.</param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    this._writer.Dispose();
                    this._mem.Dispose();
                }
                this._disposedValue = true;
            }
        }

        /// <summary>
        /// Asynchronously releases the resources of current instance.
        /// </summary>
        /// <param name="disposing">Indicates whether the caller method is <see cref="IDisposable.Dispose()"/>.</param>
        /// <returns>A taks that represents the asynchronous dispose operation.</returns>
        protected virtual async ValueTask DisposeAsync(Boolean disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    await this._writer.DisposeAsync();
                    await this._mem.DisposeAsync();
                }
                this._disposedValue = true;
            }
        }

        /// <summary>
        /// Retrieves the binary data of UTF-8 text.
        /// </summary>
        /// <param name="nullTerminated">Indicates whether the UTF-8 text must be null-terminated.</param>
        /// <returns>Binary data of UTF-8 text</returns>
        protected Byte[]? ToArray(Boolean nullTerminated = false)
        {
            if (this._mem.Length > 0)
                return this.GetBinaryData(nullTerminated);
            return default;
        }
    }
}

