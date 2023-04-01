namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Comparator class for <see cref="CString"/> instances.
/// </summary>
internal sealed class CStringUtf8Comparator : Utf8ComparatorBase<Byte>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    public CStringUtf8Comparator(StringComparison comparisonType = StringComparison.CurrentCulture) : base(comparisonType)
    {
    }

    /// <inheritdoc/>
    protected override Rune? DecodeRune(ReadOnlySpan<Byte> source, out Int32 charsConsumed)
        => DecodeRuneFromUtf8(source, out charsConsumed);

    /// <inheritdoc/>
    protected override ReadOnlySpan<Char> GetUnicodeSpan(ReadOnlySpan<Byte> source)
        => GetUnicodeSpanFromUtf8(source);
}

