#if !NETCOREAPP
using Rune = System.UInt32;
#endif

namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// The abstract Utf8Comparator class provides a means for efficiently and customizable
/// comparing two UTF-8 texts.
/// </summary>
/// <typeparam name="TChar">The type of characters in the text being compared.</typeparam>
internal abstract partial class Utf8Comparator<TChar> where TChar : unmanaged
{
	/// <summary>
	/// Constructor for setting up the comparator with a specific comparison type.
	/// </summary>
	/// <param name="comparisonType">
	/// One of the enumeration values that specifies the rules to use in the comparison.
	/// </param>
	protected Utf8Comparator(StringComparison comparisonType)
	{
		switch (comparisonType)
		{
			case StringComparison.CurrentCulture:
				this._culture = CultureInfo.CurrentCulture;
				this._options = CompareOptions.None;
				this._optionsIgnoreCase = CompareOptions.IgnoreCase;
				this._ignoreCase = false;
				this._ordinal = false;
				break;
			case StringComparison.CurrentCultureIgnoreCase:
				this._culture = CultureInfo.CurrentCulture;
				this._options = CompareOptions.IgnoreCase;
				this._optionsIgnoreCase = CompareOptions.IgnoreCase;
				this._ignoreCase = true;
				this._ordinal = false;
				break;
			case StringComparison.InvariantCulture:
				this._culture = CultureInfo.InvariantCulture;
				this._options = CompareOptions.None;
				this._optionsIgnoreCase = CompareOptions.IgnoreCase;
				this._ignoreCase = false;
				this._ordinal = false;
				break;
			case StringComparison.InvariantCultureIgnoreCase:
				this._culture = CultureInfo.InvariantCulture;
				this._options = CompareOptions.IgnoreCase;
				this._optionsIgnoreCase = CompareOptions.IgnoreCase;
				this._ignoreCase = true;
				this._ordinal = false;
				break;
			case StringComparison.OrdinalIgnoreCase:
				this._culture = CultureInfo.InvariantCulture;
				this._options = CompareOptions.OrdinalIgnoreCase;
				this._optionsIgnoreCase = CompareOptions.OrdinalIgnoreCase;
				this._ignoreCase = true;
				this._ordinal = true;
				break;
			default:
				this._culture = CultureInfo.InvariantCulture;
				this._options = CompareOptions.Ordinal;
				this._optionsIgnoreCase = CompareOptions.Ordinal;
				this._ignoreCase = false;
				this._ordinal = true;
				break;
		}
	}
	/// <summary>
	/// Constructor for setting up the comparator with a specific case-ignore specification and culture.
	/// </summary>
	/// <param name="ignoreCase">true to ignore case during the comparison; otherwise, false.</param>
	/// <param name="culture">An object that supplies culture-specific comparison information.</param>
	protected Utf8Comparator(Boolean ignoreCase, CultureInfo? culture)
	{
		this._culture = culture ?? CultureInfo.CurrentCulture;
		this._ignoreCase = ignoreCase;
		this._options = !ignoreCase ? CompareOptions.None : CompareOptions.IgnoreCase;
		this._optionsIgnoreCase = CompareOptions.IgnoreCase;
	}

	/// <summary>
	/// Compares two specified texts using the specified rules and returns an integer that indicates their relative position in
	/// the sort order.
	/// </summary>
	/// <param name="textA">The first text to compare.</param>
	/// <param name="textB">The second text instance.</param>
	/// <returns>
	/// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
	/// <list type="table">
	///     <listheader>
	///         <term>Value</term><description>Condition</description>
	///     </listheader>
	///     <item>
	///         <term>Less than zero</term>
	///         <description><paramref name="textA"/> precedes <paramref name="textB"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Zero</term>
	///         <description><paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.</description>
	///     </item>
	///     <item>
	///         <term>Greater than zero</term>
	///         <description><paramref name="textA"/> follows <paramref name="textB"/> in the sort order.</description>
	///     </item>
	/// </list>
	/// </returns>
	public Int32 Compare(ReadOnlySpan<Byte> textA, ReadOnlySpan<TChar> textB)
		=> this._ordinal ? this.OrdinalCompare(textA, textB) : this.Compare(textA, textB, this._ignoreCase);
	/// <summary>
	/// Determines whether the text in <paramref name="textA"/> and the text in <paramref name="textB"/> are equivalent,
	/// using specified culture, case, and sorting rules during the comparison.
	/// </summary>
	/// <param name="textA">The first text to compare.</param>
	/// <param name="textB">The second text to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="textA"/> is the same as the value of <paramref name="textB"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	public Boolean TextEquals(ReadOnlySpan<Byte> textA, ReadOnlySpan<TChar> textB)
	{
		while (!textA.IsEmpty && !textB.IsEmpty)
		{
			//Preserve the original text in comparison.
			ReadOnlySpan<Byte> textA0 = textA;
			ReadOnlySpan<TChar> textB0 = textB;

			DecodedRune? runeA = Utf8Comparator<TChar>.DecodeRuneFromUtf8(ref textA);
			DecodedRune? runeB = this.DecodeRune(ref textB);

			//If the runes are not comparable to each other a full text comparison will be needed.
			if (!runeA.HasValue || !runeB.HasValue) return this.Compare(textA0, textB0, this._ignoreCase) == 0;
			//If the value of both runes is the same, no further comparison is necessary.
			if (runeA == runeB) continue;
			String strA = Char.ConvertFromUtf32(runeA.Value.Value);
			String strB = Char.ConvertFromUtf32(runeB.Value.Value);
#if NET5_0_OR_GREATER
			if (this._culture.CompareInfo.Compare(strA, strB, this.GetOptions(this._ignoreCase)) != 0)
#else
			if (this._culture.CompareInfo.Compare(strA, strB, this.GetOptions(this._ignoreCase)) != 0)
#endif
				return false;
		}

		return textA.IsEmpty && textB.IsEmpty;
	}

	/// <summary>
	/// Retrieves a <see cref="ReadOnlySpan{Char}"/> representation of the <paramref name="source"/>.
	/// </summary>
	/// <param name="source">A read-only span of <typeparamref name="TChar"/> elements that represent a text.</param>
	/// <returns>A <see cref="ReadOnlySpan{Char}"/> that represents the <paramref name="source"/> text.</returns>
	protected abstract ReadOnlySpan<Char> GetUnicodeSpan(ReadOnlySpan<TChar> source);
	/// <summary>
	/// Decodes the <see cref="Rune"/> at the beginning of the provided <typeparamref name="TChar"/> source buffer.
	/// </summary>
	/// <param name="source">A read-only span of <typeparamref name="TChar"/> elements that represents a text.</param>
	/// <returns>The decoded <see cref="Rune"/>, if any; otherwise, <see langword="null"/>.</returns>
	protected abstract DecodedRune? DecodeRune(ref ReadOnlySpan<TChar> source);

	/// <summary>
	/// Retrieves the <see cref="ReadOnlySpan{Char}"/> representation of the <paramref name="source"/>.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="Byte"/> elements representing a UTF-8 encoded text.</param>
	/// <returns>A <see cref="ReadOnlySpan{Char}"/> that represents the <paramref name="source"/> text.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static ReadOnlySpan<Char> GetUnicodeSpanFromUtf8(ReadOnlySpan<Byte> source)
		=> Encoding.UTF8.GetString(source);
	/// <summary>
	/// Decodes the <see cref="Rune"/> at the beginning of the provided UTF-8 encoded source buffer.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="Byte"/> elements representing a UTF-8 encoded text.</param>
	/// <returns>The decoded <see cref="Rune"/>, if any; otherwise, <see langword="null"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static DecodedRune? DecodeRuneFromUtf8(ref ReadOnlySpan<Byte> source)
	{
		DecodedRune? result = DecodedRune.Decode(source);
		if (result.HasValue)
			source = source[result.Value.CharsConsumed..];
		return result;
	}
}