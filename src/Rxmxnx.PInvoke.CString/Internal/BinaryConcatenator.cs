namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Class helper for UTF-8 text concatenation
/// </summary>
internal abstract partial class BinaryConcatenator<T> : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// Current instance stream.
    /// </summary>
    protected MemoryStream Stream => this._mem;
    /// <summary>
    /// The token for monitor to cancellation requests
    /// </summary>
    protected CancellationToken CancellationToken => this._cancellationToken;

    /// <summary>
    /// Instance separator.
    /// </summary>
    public T? Separator => this._separator;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="separator">Separator for concatenation.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    protected BinaryConcatenator(T? separator, CancellationToken cancellationToken)
    {
        this._mem = new();
        this._separator = separator;
        this._cancellationToken = cancellationToken;
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
    public Task WriteAsync(T? value) => this._writeAsync(value);

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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract void WriteValue(T value);

    /// <summary>
    /// Asynchronously writes <paramref name="value"/> in current instance.
    /// </summary>
    /// <param name="value">Value to write.</param>
    /// <returns>A taks that represents the asynchronous write operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract Task WriteValueAsync(T value);

    /// <summary>
    /// Indicates whether <paramref name="value"/> is empty.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is empty; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected abstract Boolean IsEmpty([NotNullWhen(false)] T? value);

    /// <summary>
    /// Indicates whether <paramref name="value"/> is empty.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="value"/> is empty; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    protected virtual Boolean IsEmpty(ReadOnlySpan<Byte> value) => value.IsEmpty;

    /// <summary>
    /// Releases the resources of current instance.
    /// </summary>
    /// <param name="disposing">Indicates whether the caller method is <see cref="IDisposable.Dispose()"/>.</param>
    protected virtual void Dispose(Boolean disposing)
    {
        if (!this._disposedValue)
        {
            if (disposing)
                this._mem.Dispose();
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
                await this._mem.DisposeAsync();
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
        Byte[]? result = default;
        if (this._mem.Length > 0)
        {
            ReadOnlySpan<Byte> span = PrepareUtf8Text(this._mem.GetBuffer());
            if (!span.IsEmpty)
            {
                Int32 resultLength = span.Length + (nullTerminated ? 1 : 0);
                result = new Byte[resultLength];
                span.CopyTo(result);
            }
        }
        return result;
    }
}