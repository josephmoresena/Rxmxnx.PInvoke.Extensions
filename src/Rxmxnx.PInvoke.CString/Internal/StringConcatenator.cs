namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for <see cref="CString"/> concatenation.
/// </summary>
internal sealed class StringConcatenator : BinaryConcatenator<String>, IDisposable
{
    /// <summary>
    /// Indicates whether the empty values must be ignored in the concatenation.
    /// </summary>
    private readonly Boolean _ignoreEmpty = false;
    /// <summary>
    /// <see cref="StreamWriter"/> used as UTF-8 writer.
    /// </summary>
    private readonly StreamWriter _writer;

    /// <summary>
    /// Indicates whether current instance has been disposed.
    /// </summary>
    private Boolean _disposedValue = false;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    public StringConcatenator(CancellationToken cancellationToken = default) : this(default, cancellationToken) { }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="separator"><see cref="String"/> separator instance.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
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

