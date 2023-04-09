namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for <see cref="CString"/> concatenation.
/// </summary>
internal sealed class CStringConcatenator : BinaryConcatenator<CString>
{
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
        base(separator, CString.IsNullOrEmpty, cancellationToken)
    {
    }

    /// <inheritdoc/>
    protected override void WriteValue(CString value)
        => value.Write(base.Stream, false);

    /// <inheritdoc/>
    protected override Task WriteValueAsync(CString value)
        => value.WriteAsync(base.Stream, false, base.CancellationToken);

    /// <summary>
    /// Creates a <see cref="CString"/> instance from concatenation.
    /// </summary>
    /// <returns>
    /// A <see cref="CString"/> instance that represents the UTF-8 concatenation.
    /// </returns>
    public CString ToCString() => base.ToArray(true) ?? CString.Empty;
}

