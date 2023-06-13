namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for concatenating <see cref="String"/> instances.
/// </summary>
internal sealed class StringConcatenator : BinaryConcatenator<String>
{
    /// <summary>
    /// Indicates whether the empty values should be ignored during the
    /// concatenation process.
    /// </summary>
    private readonly Boolean _ignoreEmpty = false;
    /// <summary>
    /// The <see cref="StreamWriter"/> used for writing UTF-8 encoded data.
    /// </summary>
    private readonly StreamWriter _writer;

    /// <summary>
    /// Indicates whether the current instance has been disposed.
    /// </summary>
    private Boolean _disposedValue = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="StringConcatenator"/> class
    /// without a specified separator.
    /// </summary>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    public StringConcatenator(CancellationToken cancellationToken = default) : this(default, cancellationToken) { }
    /// <summary>
    /// Initializes a new instance of the <see cref="StringConcatenator"/> class
    /// with a specified separator string.
    /// </summary>
    /// <param name="separator">The string to use as a separator in the concatenation.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests.
    /// The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    public StringConcatenator(String? separator, CancellationToken cancellationToken = default) :
        base(separator, cancellationToken)
    {
        this._ignoreEmpty = !String.IsNullOrEmpty(separator);
        this._writer = new(base.Stream, Encoding.UTF8, leaveOpen: true) { AutoFlush = true, };
    }

    /// <inheritdoc/>
    protected override Boolean IsEmpty(String? value) => String.IsNullOrEmpty(value) && !this._ignoreEmpty;
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    protected override Boolean IsEmpty(ReadOnlySpan<Byte> value) => base.IsEmpty(value) && !this._ignoreEmpty;
    /// <inheritdoc/>
    protected override void Dispose(Boolean disposing)
    {
        if (!this._disposedValue)
        {
            if (disposing)
                this._writer.Dispose();
            this._disposedValue = true;
        }
        base.Dispose(disposing);
    }
    /// <inheritdoc/>
    protected override async ValueTask DisposeAsync(Boolean disposing)
    {
        if (!this._disposedValue)
        {
            if (disposing)
                await this._writer.DisposeAsync();
            this._disposedValue = true;
        }
        await base.DisposeAsync(disposing);
    }
    /// <inheritdoc/>
    protected override void WriteValue([AllowNull] String value) => this._writer.Write(value);
    /// <inheritdoc/>
    protected override Task WriteValueAsync([AllowNull] String value) => this._writer.WriteAsync(value);
}

