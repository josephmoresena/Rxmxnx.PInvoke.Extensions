namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Comparator class for <see cref="CString"/> instances.
/// </summary>
internal sealed class CStringUtf8Comparator : Utf8Comparator<Byte>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    private CStringUtf8Comparator(StringComparison comparisonType)
        : base(comparisonType)
    {
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ignoreCase">
    /// <see langword="true"/> to ignore case during the comparision; otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="culture">
    /// An object that supplies culture-specific comparision information.
    /// If <paramref name="culture"/> is <see langword="null"/>, the current culture is used.
    /// </param>
    private CStringUtf8Comparator(Boolean ignoreCase, CultureInfo? culture)
        : base(ignoreCase, culture)
    {
    }

    /// <inheritdoc/>
    protected override DecodedRune? DecodeRune(ref ReadOnlySpan<Byte> source) => DecodeRuneFromUtf8(ref source);

    /// <inheritdoc/>
    protected override ReadOnlySpan<Char> GetUnicodeSpan(ReadOnlySpan<Byte> source)
        => GetUnicodeSpanFromUtf8(source);

    /// <summary>
    /// Creates a new <see cref="CStringUtf8Comparator"/> instance.
    /// </summary>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    /// <returns>A new <see cref="CStringUtf8Comparator"/> instance.</returns>
    public static CStringUtf8Comparator Create(StringComparison comparisonType = StringComparison.CurrentCulture)
        => new(comparisonType);

    /// <summary>
    /// Creates a new <see cref="StringUtf8Comparator"/> instance.
    /// </summary>
    /// <param name="ignoreCase">
    /// <see langword="true"/> to ignore case during the comparision; otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="culture">
    /// An object that supplies culture-specific comparision information.
    /// If <paramref name="culture"/> is <see langword="null"/>, the current culture is used.
    /// </param>
    /// <returns>A new <see cref="CStringUtf8Comparator"/> instance.</returns>
    public static CStringUtf8Comparator Create(Boolean ignoreCase, CultureInfo? culture = default)
        => new(ignoreCase, culture);
}

