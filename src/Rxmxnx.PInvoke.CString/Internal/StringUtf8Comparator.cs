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
    private StringUtf8Comparator(StringComparison comparisonType)
        : base(comparisonType)
    {
    }

    /// <inheritdoc/>
    protected override DecodedRune? DecodeRune(ReadOnlySpan<Char> source) => DecodedRune.Decode(source);

    /// <inheritdoc/>
    protected override ReadOnlySpan<Char> GetUnicodeSpan(ReadOnlySpan<Char> source) => source;

    /// <summary>
    /// Creates a new <see cref="StringUtf8Comparator"/> instance.
    /// </summary>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    /// <returns>A new <see cref="CStringUtf8Comparator"/> instance.</returns>
    public static StringUtf8Comparator Create(StringComparison comparisonType = StringComparison.CurrentCulture)
        => new(comparisonType);

    /// <summary>
    /// Creates a new <see cref="StringUtf8Comparator"/> instance.
    /// </summary>
    /// <param name="ignoreCase">
    /// <see langword="true"/> to ignore case during the comparision; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>A new <see cref="StringUtf8Comparator"/> instance.</returns>
    public static StringUtf8Comparator Create(Boolean ignoreCase)
        => new(ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture);
}

