namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for <see cref="CString"/> concatenation.
/// </summary>
internal sealed class CStringConcatenator : BinaryConcatenator<CString>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="separator"><see cref="CString"/> separator instance.</param>
    public CStringConcatenator(CString? separator) :
        base(separator, CString.IsNullOrEmpty)
    {
    }

    /// <inheritdoc/>
    protected override void WriteValue(CString value)
        => value.Write(base.Stream, false);

    /// <inheritdoc/>
    protected override Task WriteValueAsync(CString value)
        => value.WriteAsync(base.Stream, false);

    /// <summary>
    /// Creates a <see cref="CString"/> instance from concatenation.
    /// </summary>
    /// <returns>
    /// A <see cref="CString"/> instance that represents the UTF-8 concatenation.
    /// </returns>
    public CString ToCString() => base.ToArray(true) ?? CString.Empty;
}

