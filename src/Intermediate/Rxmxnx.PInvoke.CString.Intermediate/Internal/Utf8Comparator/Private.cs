namespace Rxmxnx.PInvoke.Internal;

internal partial class Utf8Comparator<TChar>
{
	/// <summary>
	/// The culture used for the current comparison.
	/// </summary>
	private readonly CultureInfo _culture;
	/// <summary>
	/// A value indicating whether the current comparison is case-insensitive.
	/// </summary>
	private readonly Boolean _ignoreCase;
	/// <summary>
	/// The options used for the current comparison.
	/// </summary>
	private readonly CompareOptions _options;
	/// <summary>
	/// The options used for a case-insensitive comparison.
	/// </summary>
	private readonly CompareOptions _optionsIgnoreCase;
	/// <summary>
	/// A value indicating whether the current comparison is ordinal.
	/// </summary>
	private readonly Boolean _ordinal;

	/// <summary>
	/// Compares two specified <see cref="ReadOnlySpan{Char}"/> instances using the specified rules, and returns an integer
	/// that indicates their relative position in
	/// the sort order.
	/// </summary>
	/// <param name="textA">The first text to compare.</param>
	/// <param name="textB">The second text to compare.</param>
	/// <param name="ignoreCase">
	/// <see langword="true"/> to ignore case during the comparison; otherwise, <see langword="false"/>.
	/// </param>
	/// <param name="stringB">The second string instance.</param>
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private Int32 Compare(ReadOnlySpan<Byte> textA, ReadOnlySpan<TChar> textB, Boolean ignoreCase,
		String? stringB = default)
		=> this.Compare(this._culture.CompareInfo, this.GetOptions(ignoreCase), textA, textB, stringB);
	/// <summary>
	/// Compares two specified <see cref="ReadOnlySpan{Char}"/> instances using the specified rules, and returns an integer
	/// that indicates their relative position in
	/// the sort order.
	/// </summary>
	/// <param name="compareInfo">A <see cref="CompareInfo"/> instance.</param>
	/// <param name="options">A <see cref="CompareOptions"/> value.</param>
	/// <param name="textA">The first text to compare.</param>
	/// <param name="textB">The second text to compare.</param>
	/// <param name="stringB">The second string instance.</param>
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
	private Int32 Compare(CompareInfo compareInfo, CompareOptions options, ReadOnlySpan<Byte> textA,
		ReadOnlySpan<TChar> textB, String? stringB)
	{
#if NET5_0_OR_GREATER
		Int32 stackConsumed = 0;
		Char[]? arrayA = default;
		Char[]? arrayB = default;

		try
		{
			Int32 charCountA = Utf8Comparator.GetCharCountFromUtf8(textA);
			Span<Char> spanA = StackAllocationHelper.ConsumeStackBytes(charCountA, ref stackConsumed) ?
				stackalloc Char[charCountA] :
				StackAllocationHelper.RentArray(charCountA, out arrayA, false);

			Utf8Comparator.CopyCharsFromUtf8(textA, spanA);
			if (stringB is not null)
				return compareInfo.Compare(spanA, stringB, options);
			if (typeof(TChar) == typeof(Char))
				return compareInfo.Compare(spanA, MemoryMarshal.Cast<TChar, Char>(textB), options);

			Int32 charCountB = this.CountChars(textB);
			Span<Char> spanB = StackAllocationHelper.ConsumeStackBytes(charCountA, ref stackConsumed) ?
				stackalloc Char[charCountB] :
				StackAllocationHelper.RentArray(charCountB, out arrayB, false);

			this.GetChars(textB, spanB);
			return compareInfo.Compare(spanA, spanB, options);
		}
		finally
		{
			StackAllocationHelper.ReturnArray(arrayA);
			StackAllocationHelper.ReturnArray(arrayB);
			StackAllocationHelper.ReleaseStackBytes(stackConsumed);
		}
#else
		String stringA = Utf8Comparator.GetStringFromUtf8(textA);
		return compareInfo.Compare(stringA, stringB ?? this.GetString(textB), options);
#endif
	}
	/// <summary>
	/// Compares two specified <see cref="ReadOnlySpan{Char}"/> instances by evaluating the numeric values of the UTF-16 units.
	/// </summary>
	/// <param name="textA">The first text to compare.</param>
	/// <param name="textB">The second text to compare.</param>
	/// <param name="stringB">The second string instance.</param>
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
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
	private Int32 OrdinalCompare(ReadOnlySpan<Byte> textA, ReadOnlySpan<TChar> textB, String? stringB)
	{
		if (this._ignoreCase)
			return this.Compare(this._culture.CompareInfo, CompareOptions.OrdinalIgnoreCase, textA, textB, stringB);

		Int32 stackConsumed = 0;
		Char[]? arrayA = default;
		Char[]? arrayB = default;
		try
		{
			Int32 charCountA = Utf8Comparator.GetCharCountFromUtf8(textA);
			Span<Char> spanA = StackAllocationHelper.ConsumeStackBytes(charCountA, ref stackConsumed) ?
				stackalloc Char[charCountA] :
				StackAllocationHelper.RentArray(charCountA, out arrayA, false);

			Utf8Comparator.CopyCharsFromUtf8(textA, spanA);
			if (stringB is not null)
				return Utf8Comparator.OrdinalCompare(spanA, stringB.AsSpan());
			if (typeof(TChar) == typeof(Char))
				return Utf8Comparator.OrdinalCompare(spanA, MemoryMarshal.Cast<TChar, Char>(textB));

			Int32 charCountB = this.CountChars(textB);
			Span<Char> spanB = StackAllocationHelper.ConsumeStackBytes(charCountA, ref stackConsumed) ?
				stackalloc Char[charCountB] :
				StackAllocationHelper.RentArray(charCountB, out arrayB, false);

			this.GetChars(textB, spanB);
			return Utf8Comparator.OrdinalCompare(spanA, spanB);
		}
		finally
		{
			StackAllocationHelper.ReturnArray(arrayA);
			StackAllocationHelper.ReturnArray(arrayB);
			StackAllocationHelper.ReleaseStackBytes(stackConsumed);
		}
	}
	/// <summary>
	/// Retrieves the <see cref="CompareOptions"/> for the current comparison.
	/// </summary>
	/// <param name="caseInsensitive">Indicates whether the comparison should be case-insensitive.</param>
	/// <returns><see cref="CompareOptions"/> for the current comparison.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private CompareOptions GetOptions(Boolean caseInsensitive)
		=> !caseInsensitive ? this._options : this._optionsIgnoreCase;
	/// <summary>
	/// Determines whether the text in <paramref name="runeA"/> and the text in <paramref name="runeB"/> are equivalent,
	/// using specified culture, case, and sorting rules during the comparison.
	/// </summary>
	/// <param name="runeA">The first text to compare.</param>
	/// <param name="runeB">The second text to compare.</param>
	/// <returns>
	/// <see langword="true"/> if the value of <paramref name="runeA"/> is the same as the value of <paramref name="runeB"/>;
	/// otherwise, <see langword="false"/>.
	/// </returns>
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
	private Boolean RuneEqual(DecodedRune runeA, DecodedRune runeB)
	{
		CompareOptions compareOptions = this.GetOptions(this._ignoreCase);
		Boolean result = runeA == runeB;

		// If the value of both runes are the same, no further comparison is necessary.
		if (result || compareOptions is CompareOptions.Ordinal)
			return result;

		// If not ordinal equality, perform a text comparison.
#if NET5_0_OR_GREATER
		Span<Char> strA = stackalloc Char[2];
		Span<Char> strB = stackalloc Char[2];

		((Rune)runeA.Value).EncodeToUtf16(strA);
		((Rune)runeB.Value).EncodeToUtf16(strA);
#else
		String strA = Char.ConvertFromUtf32(runeA.Value);
		String strB = Char.ConvertFromUtf32(runeB.Value);
#endif
		return this._culture.CompareInfo.Compare(strA, strB, compareOptions) == 0;
	}
	/// <summary>
	/// Retrieves a substring of <paramref name="stringB"/> according to <paramref name="textB"/>.
	/// </summary>
	/// <param name="textB">The second text to compare.</param>
	/// <param name="stringB">The second string instance.</param>
	/// <returns>A  substring of <paramref name="stringB"/>.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private String? GetSubStringB(ReadOnlySpan<TChar> textB, String? stringB)
		=> !Utf8Comparator<TChar>.IgnoreStringInput ? stringB?[^this.CountChars(textB)..] : default;
}