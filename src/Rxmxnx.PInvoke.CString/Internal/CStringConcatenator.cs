namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for <see cref="CString"/> concatenation.
/// </summary>
internal sealed class CStringConcatenator : BinaryConcatenator<CString>
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
    public CStringConcatenator(CancellationToken cancellationToken = default) : this(default, cancellationToken) { }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="separator"><see cref="CString"/> separator instance.</param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
    /// </param>
    public CStringConcatenator(CString? separator, CancellationToken cancellationToken = default) :
        base(separator, cancellationToken)
    {
        this._ignoreEmpty = !CString.IsNullOrEmpty(separator);
    }

    /// <inheritdoc/>
    protected override void WriteValue(CString value)
        => value.Write(base.Stream, false);

    /// <inheritdoc/>
    protected override Task WriteValueAsync(CString value)
        => value.WriteAsync(base.Stream, false, base.CancellationToken);

    /// <inheritdoc/>
    protected override Boolean IsEmpty([NotNullWhen(false)] CString? value)
        => value is null || value.Length == 0 && !this._ignoreEmpty;

    /// <inheritdoc/>
    protected override Boolean IsEmpty(ReadOnlySpan<Byte> value) => base.IsEmpty(value) && !this._ignoreEmpty;

    /// <summary>
    /// Creates a <see cref="CString"/> instance from concatenation.
    /// </summary>
    /// <returns>
    /// A <see cref="CString"/> instance that represents the UTF-8 concatenation.
    /// </returns>
    public CString ToCString() => base.ToArray(true) ?? CString.Empty;
}

