namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Comparator class for <see cref="CString"/> and <see cref="String"/> instances.
/// </summary>
internal sealed class StringUtf8Comparator : Utf8ComparatorBase<Char>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    public StringUtf8Comparator(StringComparison comparisonType = StringComparison.CurrentCulture) : base(comparisonType)
    {
    }

    /// <inheritdoc/>
    protected override Rune? DecodeRune(ReadOnlySpan<Char> source, out Int32 charsConsumed)
        => Rune.DecodeFromUtf16(source, out Rune result, out charsConsumed) == OperationStatus.Done ? result : default(Rune?);

    /// <inheritdoc/>
    protected override ReadOnlySpan<Char> GetUnicodeSpan(ReadOnlySpan<Char> source) => source;
}

