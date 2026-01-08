namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// A comparator class for comparing <see cref="CString"/> and <see cref="String"/> instances.
/// </summary>
internal sealed class StringUtf8Comparator : Utf8Comparator<Char>
{
	/// <summary>
	/// Equality comparator instance.
	/// </summary>
	public static readonly StringUtf8Comparator OrdinalComparator =
		StringUtf8Comparator.Create(StringComparison.Ordinal);

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
		if (result.HasValue)
			source = source[result.Value.CharsConsumed..];
		return result;
	}
	/// <inheritdoc/>
#if !PACKAGE && (!NETCOREAPP || NET7_0_OR_GREATER)
	[ExcludeFromCodeCoverage]
#endif
	protected override String GetString(ReadOnlySpan<Char> source) => new(source);
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	protected override Int32 CountChars(ReadOnlySpan<Char> source) => source.Length;
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	protected override void GetChars(ReadOnlySpan<Char> source, Span<Char> destination) => source.CopyTo(destination);

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