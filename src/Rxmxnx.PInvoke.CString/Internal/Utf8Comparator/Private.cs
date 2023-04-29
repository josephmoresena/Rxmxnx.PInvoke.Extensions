namespace Rxmxnx.PInvoke.Internal;

internal partial class Utf8Comparator<TChar>
{
    /// <summary>
    /// Culture for current comparison.
    /// </summary>
    private readonly CultureInfo _culture;
    /// <summary>
    /// Options for current comparision.
    /// </summary>
    private readonly CompareOptions _options;
    /// <summary>
    /// Options for current case-insensitive;
    /// </summary>
    private readonly CompareOptions _optionsIgnoreCase;
    /// <summary>
    /// Indicates whether comparison is case-insensitive.
    /// </summary>
    private readonly Boolean _ignoreCase;
    /// <summary>
    /// Indicates whether comparison is ordinal.
    /// </summary>
    private readonly Boolean _ordinal;

    /// <summary>
    /// Compares two specified <see cref="ReadOnlySpan{Char}"/> using the specified rules, and returns an integer that indicates their relative position in
    /// the sort order.
    /// </summary>
    /// <param name="textA">The first text to compare.</param>
    /// <param name="textB">The second text instance.</param>
    /// <param name="ignoreCase">
    /// <see langword="true"/> to ignore case during the comparision; otherwise, <see langword="false"/>.
    /// </param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description><paramref name="textA"/> precedes <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Null</term>
    /// <description><paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="textA"/> follows <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Int32 Compare(ReadOnlySpan<Byte> textA, ReadOnlySpan<TChar> textB, Boolean ignoreCase)
    {
        ReadOnlySpan<Char> spanA = GetUnicodeSpanFromUtf8(textA);
        ReadOnlySpan<Char> spanB = this.GetUnicodeSpan(textB);
        return this._culture.CompareInfo.Compare(spanA, spanB, this.GetOptions(ignoreCase));
    }

    /// <summary>
    /// Compares two specified <see cref="ReadOnlySpan{Char}"/> valuating the numeric values of the UTF-16 units.
    /// </summary>
    /// <param name="textA">The first text to compare.</param>
    /// <param name="textB">The second text instance.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description><paramref name="textA"/> precedes <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Null</term>
    /// <description><paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="textA"/> follows <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    private Int32 OrdinalCompare(ReadOnlySpan<Byte> textA, ReadOnlySpan<TChar> textB)
    {
        String strA = new(GetUnicodeSpanFromUtf8(textA));
        String strB = new(this.GetUnicodeSpan(textB));

        if (!this._ignoreCase)
            return String.CompareOrdinal(strA, strB);
        else
            return String.Compare(strA, strB, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Retrieves the <see cref="CompareOptions"/> for current comparison.
    /// </summary>
    /// <param name="caseInsensitive">Indicates whether current comparision should be case-insensitive.</param>
    /// <returns><see cref="CompareOptions"/> for current comparison.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CompareOptions GetOptions(Boolean caseInsensitive) => !caseInsensitive ? this._options : this._optionsIgnoreCase;
}
