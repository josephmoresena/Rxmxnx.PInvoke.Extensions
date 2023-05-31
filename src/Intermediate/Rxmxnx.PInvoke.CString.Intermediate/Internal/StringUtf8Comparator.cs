﻿namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Comparator class for <see cref="CString"/> and <see cref="String"/> instances.
/// </summary>
internal sealed class StringUtf8Comparator : Utf8Comparator<Char>
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    private StringUtf8Comparator(StringComparison comparisonType)
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
    private StringUtf8Comparator(Boolean ignoreCase, CultureInfo? culture)
        : base(ignoreCase, culture)
    {
    }

    /// <inheritdoc/>
    protected override DecodedRune? DecodeRune(ref ReadOnlySpan<Char> source)
    {
        DecodedRune? result = DecodedRune.Decode(source);
        if (result is not null)
            source = source[result.CharsConsumed..];
        return result;
    }

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
    /// <param name="culture">
    /// An object that supplies culture-specific comparision information.
    /// If <paramref name="culture"/> is <see langword="null"/>, the current culture is used.
    /// </param>
    /// <returns>A new <see cref="StringUtf8Comparator"/> instance.</returns>
    public static StringUtf8Comparator Create(Boolean ignoreCase, CultureInfo? culture = default)
        => new(ignoreCase, culture);
}
