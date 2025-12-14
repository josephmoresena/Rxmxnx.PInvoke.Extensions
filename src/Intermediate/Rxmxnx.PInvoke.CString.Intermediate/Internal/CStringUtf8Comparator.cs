namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// A comparator class for comparing <see cref="CString"/> instances.
/// </summary>
internal sealed class CStringUtf8Comparator : Utf8Comparator<Byte>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringUtf8Comparator"/> class.
	/// </summary>
	/// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
	private CStringUtf8Comparator(StringComparison comparisonType) : base(comparisonType) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringUtf8Comparator"/> class.
	/// </summary>
	/// <param name="ignoreCase">
	/// <see langword="true"/> to ignore case during the comparison; otherwise, <see langword="false"/>.
	/// </param>
	/// <param name="culture">
	/// An object that supplies culture-specific comparison information.
	/// If <paramref name="culture"/> is <see langword="null"/>, the current culture is used.
	/// </param>
	private CStringUtf8Comparator(Boolean ignoreCase, CultureInfo? culture) : base(ignoreCase, culture) { }

	/// <inheritdoc/>
	protected override DecodedRune? DecodeRune(ref ReadOnlySpan<Byte> source)
		=> Utf8Comparator.DecodeRuneFromUtf8(ref source);
	/// <inheritdoc/>
	protected override ReadOnlySpan<Char> GetUnicodeSpan(ReadOnlySpan<Byte> source)
		=> Utf8Comparator.GetUnicodeSpanFromUtf8(source);
	/// <inheritdoc/>
	protected override String GetString(ReadOnlySpan<Byte> source) => Utf8Comparator.GetStringFromUtf8(source);
	/// <inheritdoc/>
	protected override Int32 CountChars(ReadOnlySpan<Byte> source) => Utf8Comparator.GetCharCountFromUtf8(source);
	/// <inheritdoc/>
	protected override void GetChars(ReadOnlySpan<Byte> source, Span<Char> destination)
		=> Utf8Comparator.CopyCharsFromUtf8(source, destination);

	/// <summary>
	/// Creates a new instance of the <see cref="CStringUtf8Comparator"/> class.
	/// </summary>
	/// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparison.</param>
	/// <returns>A new instance of the <see cref="CStringUtf8Comparator"/> class.</returns>
	public static CStringUtf8Comparator Create(StringComparison comparisonType = StringComparison.CurrentCulture)
		=> new(comparisonType);
	/// <summary>
	/// Creates a new instance of the <see cref="CStringUtf8Comparator"/> class.
	/// </summary>
	/// <param name="ignoreCase">
	/// <see langword="true"/> to ignore case during the comparison; otherwise, <see langword="false"/>.
	/// </param>
	/// <param name="culture">
	/// An object that supplies culture-specific comparison information.
	/// If <paramref name="culture"/> is <see langword="null"/>, the current culture is used.
	/// </param>
	/// <returns>A new instance of the <see cref="CStringUtf8Comparator"/> class.</returns>
	public static CStringUtf8Comparator Create(Boolean ignoreCase, CultureInfo? culture = default)
		=> new(ignoreCase, culture);
}