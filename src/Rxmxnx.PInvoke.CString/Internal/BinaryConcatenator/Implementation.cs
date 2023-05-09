namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for Binary concatenation.
/// </summary>
internal sealed class BinaryConcatenator : BinaryConcatenator<Byte?>
{
    /// <summary>
    /// Indicates whether the empty values must be ignored in the concatenation.
    /// </summary>
    private readonly Boolean _ignoreEmpty = false;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    public BinaryConcatenator(CancellationToken cancellationToken = default) : base(default, cancellationToken)
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="separator"><see cref="Byte"/> separator instance.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    public BinaryConcatenator(Byte separator, CancellationToken cancellationToken = default) : base(separator, cancellationToken)
    {
        this._ignoreEmpty = true;
    }

    /// <inheritdoc/>
    protected override void WriteValue(Byte? value)
        => this.Stream.WriteByte(value!.Value);

    /// <inheritdoc/>
    protected override Task WriteValueAsync(Byte? value)
        => Task.Run(() => this.Stream.WriteByte(value!.Value), base.CancellationToken);

    /// <inheritdoc/>
    protected override Boolean IsEmpty([NotNullWhen(false)] Byte? value) => !value.HasValue;

    /// <summary>
    /// Retrieves the binary data of UTF-8 text.
    /// </summary>
    /// <param name="nullTerminated">Indicates whether the UTF-8 text must be null-terminated.</param>
    /// <returns>Binary data of UTF-8 text</returns>
    public new Byte[]? ToArray(Boolean nullTerminated) => base.ToArray(nullTerminated);

    /// <inheritdoc/>
    protected override Boolean IsEmpty(ReadOnlySpan<Byte> value) => base.IsEmpty(value) && !this._ignoreEmpty;
}
