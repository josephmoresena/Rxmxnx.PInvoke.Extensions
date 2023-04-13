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
    /// Compares two specified texts using the specified rules, and returns an integer that indicates their relative position in
    /// the sort order.
    /// </summary>
    /// <param name="state">Comparison state.</param>
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
    /// <term>Zero</term>
    /// <description><paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="textA"/> follows <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    private Int32 Compare(ComparisonState state, ref ReadOnlySpan<Byte> textA, ref ReadOnlySpan<TChar> textB)
    {
        Int32 result = 0;

        //Initialize a new comparison.
        state.InitializeComparison();
        while (!textA.IsEmpty && !textB.IsEmpty && result == 0)
        {
            //Preserve the original text in comparison.
            ReadOnlySpan<Byte> textAO = textA;
            ReadOnlySpan<TChar> textBO = textB;

            DecodedRune? runeA = DecodeRuneFromUtf8(ref textA);
            DecodedRune? runeB = this.DecodeRune(ref textB);

            //If at least one rune could not be decoded, the comparison should end.
            //The result should be determined by the length of the texts.
            if (runeA is null || runeB is null)
                return runeA is not null ? 1 : runeB is not null ? -1 : 0;

            //If the value of both runes is the same, no further comparison is necessary.
            if (runeA != runeB)
                //If the runes are not comparable to each other a full text comparison will be needed.
                if (this.UnsupportedComparison(state, runeA, runeB))
                {
                    textA = ReadOnlySpan<Byte>.Empty;
                    textB = ReadOnlySpan<TChar>.Empty;
                    return this.Compare(state, textAO, textBO);
                }
                else
                    result = this.Compare(state, runeA, runeB);
        }
        return result;
    }

    /// <summary>
    /// Compares two specified <see cref="ReadOnlySpan{Char}"/> using the specified rules, and returns an integer that indicates their relative position in
    /// the sort order.
    /// </summary>
    /// <param name="state">Comparison state.</param>
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Int32 Compare(ComparisonState state, ReadOnlySpan<Byte> textA, ReadOnlySpan<TChar> textB)
    {
        ReadOnlySpan<Char> spanA = GetUnicodeSpanFromUtf8(textA);
        ReadOnlySpan<Char> spanB = this.GetUnicodeSpan(textB);
        return this._culture.CompareInfo.Compare(spanA, spanB, this.GetOptions(state.IgnoreCase));
    }

    /// <summary>
    /// Indicates whether <paramref name="runeA"/> and <paramref name="runeB"/> are uncomparable in the current comparision.
    /// </summary>
    /// <param name="state">Comparison state.</param>
    /// <param name="runeA">The first <see cref="Rune"/> to compare.</param>
    /// <param name="runeB">The second <see cref="Rune"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="runeA"/> and <paramref name="runeB"/> are uncomparable; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Boolean UnsupportedComparison(ComparisonState state, DecodedRune runeA, DecodedRune runeB)
        => !state.IsEqualization && !runeA.IsSingleUnicode && !runeB.IsSingleUnicode && !this.IgnoreCaseEqual(runeA, runeB, state.IgnoreCase);

    /// <summary>
    /// Indicates whether the current culture comparision is unsupported.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the current culture comparision is unsupported; otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Boolean UnsupportedCulture()
        => this._culture.Name.ToLowerInvariant().Split('-').FirstOrDefault() switch
        {
            "br" => true,
            "cs" => true,
            "cu" => true,
            "cy" => true,
            "dsb" => true,
            "hsb" => true,
            "ig" => true,
            "is" => true,
            "ku" => true,
            "om" => true,
            "pl" => true,
            "th" => true,
            "tk" => true,
            "sk" => true,
            "smn" => true,
            "se" => true,
            "sq" => true,
            "uz" => true,
            "wo" => true,
            _ => false,
        };

    /// <summary>
    /// Compares two specified <see cref="Rune"/> objects using the specified rules, and returns an integer that indicates their
    /// relative position in the sort order.
    /// </summary>
    /// <param name="state">Comparison state.</param>
    /// <param name="runeA">The first <see cref="Rune"/> to compare.</param>
    /// <param name="runeB">The second <see cref="Rune"/> instance.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description><paramref name="runeA"/> precedes <paramref name="runeB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Zero</term>
    /// <description><paramref name="runeA"/> is in the same position as <paramref name="runeB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="runeA"/> follows <paramref name="runeB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Int32 Compare(ComparisonState state, Rune runeA, Rune runeB)
    {
        String strA = Char.ConvertFromUtf32(runeA.Value);
        String strB = Char.ConvertFromUtf32(runeB.Value);
        Int32 result = this._culture.CompareInfo.Compare(strA, strB, this.GetOptions(state.IgnoreCase));

        //If both runes are different in the current comparison, it is necessary to determine if the base of both
        //runes is the same.
        if (result != 0 && !this._ordinal && !state.IsEqualization)
        {
            String strANormalizedBase = strA.Normalize(NormalizationForm.FormD)[..1];
            String strBNormalizedBase = strB.Normalize(NormalizationForm.FormD)[..1];

            //If the base of both runes is the same, a new comparison is needed to determine an absolute result.
            if (this._culture.CompareInfo.Compare(strANormalizedBase, strBNormalizedBase, this._optionsIgnoreCase) == 0)
                state.SetContinue();
        }

        return result;
    }

    /// <summary>
    /// Indicates whether <paramref name="runeA"/> and <paramref name="runeB"/> are equal ignoring case.
    /// </summary>
    /// <param name="runeA"><see cref="Rune"/> instance.</param>
    /// <param name="runeB"><see cref="Rune"/> instance.</param>
    /// <param name="caseInsensitive">Indicates whether the comparison should be case-insensitive.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="runeA"/> and <paramref name="runeB"/> are equal ignoring 
    /// case; otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Boolean IgnoreCaseEqual(Rune runeA, Rune runeB, Boolean caseInsensitive)
    {
        if (this.IsOrdinalOrIgnoreCase() && caseInsensitive)
            return false;

        Rune runeAComparable = Rune.IsLower(runeA) ? Rune.ToUpper(runeA, this._culture) : Rune.ToLower(runeA, this._culture);
        return runeAComparable == runeB;
    }

    /// <summary>
    /// Retrieves the <see cref="CompareOptions"/> for current comparison.
    /// </summary>
    /// <param name="caseInsensitive">Indicates whether current comparision should be case-insensitive.</param>
    /// <returns><see cref="CompareOptions"/> for current comparison.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private CompareOptions GetOptions(Boolean caseInsensitive) => !caseInsensitive ? this._options : this._optionsIgnoreCase;

    /// <summary>
    /// Indicates whether the comparision of current instance is ordinal or case-insensitive.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if the comparision of current instance is ordinal or case-insensitive; 
    /// otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Boolean IsOrdinalOrIgnoreCase() => this._options == this._optionsIgnoreCase;
}
