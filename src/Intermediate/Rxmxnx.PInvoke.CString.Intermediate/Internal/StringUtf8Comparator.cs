namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// A comparator class for comparing <see cref="CString"/> and <see cref="String"/> instances.
/// </summary>
internal sealed class StringUtf8Comparator : Utf8Comparator<Char>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="StringUtf8Comparator"/> class.
	/// </summary>
	/// <param name="comparisonType">
	/// One of the enumeration values that specifies the rules to use in the comparison.
	/// </param>
	private StringUtf8Comparator(StringComparison comparisonType) : base(comparisonType) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="StringUtf8Comparator"/> class.
	/// </summary>
	/// <param name="ignoreCase">
	/// <see langword="true"/> to ignore case during the comparison; otherwise, <see langword="false"/>.
	/// </param>
	/// <param name="culture">
	/// An object that supplies culture-specific comparison information.
	/// If <paramref name="culture"/> is <see langword="null"/>, the current culture is used.
	/// </param>
	private StringUtf8Comparator(Boolean ignoreCase, CultureInfo? culture) : base(ignoreCase, culture) { }

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
	/// Creates a new instance of the <see cref="StringUtf8Comparator"/> class.
	/// </summary>
	/// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
	/// <returns>A new instance of the <see cref="StringUtf8Comparator"/> class.</returns>
	public static StringUtf8Comparator Create(StringComparison comparisonType = StringComparison.CurrentCulture)
		=> new(comparisonType);
	/// <summary>
	/// Creates a new instance of the <see cref="StringUtf8Comparator"/> class.
	/// </summary>
	/// <param name="ignoreCase">
	/// <see langword="true"/> to ignore case during the comparison; otherwise, <see langword="false"/>.
	/// </param>
	/// <param name="culture">
	/// An object that supplies culture-specific comparison information.
	/// If <paramref name="culture"/> is <see langword="null"/>, the current culture is used.
	/// </param>
	/// <returns>A new instance of the <see cref="StringUtf8Comparator"/> class.</returns>
	public static StringUtf8Comparator Create(Boolean ignoreCase, CultureInfo? culture = default)
		=> new(ignoreCase, culture);
}