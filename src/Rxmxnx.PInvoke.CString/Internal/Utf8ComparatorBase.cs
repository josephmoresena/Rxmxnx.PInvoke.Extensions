namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Base class for UTF-8 text comparator.
/// </summary>
/// <typeparam name="TChar">Type of the characters in the compared text.</typeparam>
internal abstract class Utf8ComparatorBase<TChar> where TChar : unmanaged
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
    /// Constructor.
    /// </summary>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    protected Utf8ComparatorBase(StringComparison comparisonType)
    {
        switch (comparisonType)
        {
            case StringComparison.CurrentCulture:
                this._culture = CultureInfo.CurrentCulture;
                this._options = CompareOptions.None;
                this._optionsIgnoreCase = CompareOptions.IgnoreCase;
                this._ignoreCase = false;
                break;
            case StringComparison.CurrentCultureIgnoreCase:
                this._culture = CultureInfo.CurrentCulture;
                this._options = CompareOptions.IgnoreCase;
                this._optionsIgnoreCase = CompareOptions.IgnoreCase;
                this._ignoreCase = true;
                break;
            case StringComparison.InvariantCulture:
                this._culture = CultureInfo.InvariantCulture;
                this._options = CompareOptions.None;
                this._optionsIgnoreCase = CompareOptions.IgnoreCase;
                this._ignoreCase = false;
                break;
            case StringComparison.InvariantCultureIgnoreCase:
                this._culture = CultureInfo.InvariantCulture;
                this._options = CompareOptions.IgnoreCase;
                this._optionsIgnoreCase = CompareOptions.IgnoreCase;
                this._ignoreCase = true;
                break;
            case StringComparison.OrdinalIgnoreCase:
                this._culture = CultureInfo.InvariantCulture;
                this._options = CompareOptions.OrdinalIgnoreCase;
                this._optionsIgnoreCase = CompareOptions.OrdinalIgnoreCase;
                this._ignoreCase = true;
                break;
            default:
                this._culture = CultureInfo.InvariantCulture;
                this._options = CompareOptions.Ordinal;
                this._optionsIgnoreCase = CompareOptions.Ordinal;
                this._ignoreCase = false;
                break;
        }
    }

    /// <summary>
    /// Compares two specified texts using the specified rules, and returns an integer that indicates their relative position in
    /// the sort order.
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
    /// <term>Zero</term>
    /// <description><paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="textA"/> follows <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    public Int32 Compare(ReadOnlySpan<Byte> textA, ReadOnlySpan<TChar> textB)
    {
        Boolean insensitiveCaseComparison = this._ignoreCase;
        Int32 result = Compare(ref textA, ref textB, ref insensitiveCaseComparison);

        if (result != 0 && !this._ignoreCase && insensitiveCaseComparison)
        {
            Int32 resultIgnoreCase = Compare(ref textA, ref textB, ref insensitiveCaseComparison);
            if (resultIgnoreCase != 0)
                result = resultIgnoreCase;
        }

        return result != 0 || textA.IsEmpty && textB.IsEmpty ? result : !textA.IsEmpty ? 1 : 0;
    }

    /// <summary>
    /// Compares two specified texts using the specified rules, and returns an integer that indicates their relative position in
    /// the sort order.
    /// </summary>
    /// <param name="textA">The first text to compare.</param>
    /// <param name="textB">The second text instance.</param>
    /// <param name="caseInsensitive">Indicates whether the comparison should be case-insensitive.</param>
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
    private Int32 Compare(ref ReadOnlySpan<Byte> textA, ref ReadOnlySpan<TChar> textB, ref Boolean caseInsensitive)
    {
        Int32 result = 0;
        while (!textA.IsEmpty && !textB.IsEmpty && result == 0)
        {
            Rune? runeA = DecodeRuneFromUtf8(textA, out Int32 consumedA);
            Rune? runeB = this.DecodeRune(textB, out Int32 consumedB);

            if (!runeA.HasValue || !runeB.HasValue)
                return runeA.HasValue ? 1 : runeB.HasValue ? -1 : 0;

            if (runeA != runeB)
            {
                Int32? tmpResult = Compare(runeA.Value, runeB.Value, ref textA, ref textB, ref caseInsensitive);
                if (!tmpResult.HasValue)
                    return 0;
                result = tmpResult.Value;
            }

            textA = textA[consumedA..];
            textB = textB[consumedB..];
        }
        return result;
    }

    /// <summary>
    /// Compares two specified texts using the specified rules, and returns an integer that indicates their relative position in
    /// the sort order.
    /// </summary>
    /// <param name="runeA">First rune in <paramref name="textA"/>.</param>
    /// <param name="runeB">First rune in <paramref name="textB"/>.</param>
    /// <param name="textA">The first text to compare.</param>
    /// <param name="textB">The second text instance.</param>
    /// <param name="caseInsensitive">
    /// <para>Input. Indicates whether the comparison should be case-insensitive.</para>
    /// <para>
    /// Output. <see langword="true"/> if <paramref name="runeA"/> is in the same position as <paramref name="runeB"/> not in the
    /// sort order but in the case-insensitive one; otherwise, untouched.
    /// </para>
    /// </param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Null</term>
    /// <description><paramref name="textA"/> is in the same position as <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Less than zero</term>
    /// <description>
    /// <paramref name="runeA"/> precedes <paramref name="runeB"/> or <paramref name="textA"/> precedes 
    /// <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Zero</term>
    /// <description>
    /// <paramref name="runeA"/> is in the same position as <paramref name="runeB"/> in the sort order.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="runeA"/> follows <paramref name="runeB"/> or <paramref name="textA"/> 
    /// follows <paramref name="textB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    private Int32? Compare(Rune runeA, Rune runeB, ref ReadOnlySpan<Byte> textA, ref ReadOnlySpan<TChar> textB, ref Boolean caseInsensitive)
    {
        Boolean unicodeA = IsUnicode(runeA);
        Boolean unicodeB = IsUnicode(runeB);

        if (unicodeA && unicodeB && !IgnoreCaseEqual(runeA, runeB, caseInsensitive))
        {
            ReadOnlySpan<Char> spanA = GetUnicodeSpanFromUtf8(textA);
            ReadOnlySpan<Char> spanB = this.GetUnicodeSpan(textB);

            textA = ReadOnlySpan<Byte>.Empty;
            textB = ReadOnlySpan<TChar>.Empty;
            return Compare(spanA, spanB, caseInsensitive);
        }

        return Compare(runeA, runeB, ref caseInsensitive);
    }

    /// <summary>
    /// Compares two specified <see cref="Rune"/> objects using the specified rules, and returns an integer that indicates their
    /// relative position in the sort order.
    /// </summary>
    /// <param name="runeA">The first <see cref="Rune"/> to compare.</param>
    /// <param name="runeB">The second <see cref="Rune"/> instance.</param>
    /// <param name="caseInsensitive">
    /// <para>Input. Indicates whether the comparison should be case-insensitive.</para>
    /// <para>
    /// Output. <see langword="true"/> if <paramref name="runeA"/> is in the same position as <paramref name="runeB"/> not in the
    /// sort order but in the case-insensitive one; otherwise, untouched.
    /// </para>
    /// </param>
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
    private Int32 Compare(Rune runeA, Rune runeB, ref Boolean caseInsensitive)
    {
        String strA = Char.ConvertFromUtf32(runeA.Value);
        String strB = Char.ConvertFromUtf32(runeB.Value);
        Int32 result = this._culture.CompareInfo.Compare(strA, strB, this.GetOptions(caseInsensitive));

        if (result != 0 && !caseInsensitive && this._options != this._optionsIgnoreCase)
        {
            String strANormalizedBase = strA.Normalize(NormalizationForm.FormD)[..1];
            String strBNormalizedBase = strB.Normalize(NormalizationForm.FormD)[..1];

            caseInsensitive = this._culture.CompareInfo.Compare(strANormalizedBase, strBNormalizedBase, this._optionsIgnoreCase) == 0;
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
    private Boolean IgnoreCaseEqual(Rune runeA, Rune runeB, Boolean caseInsensitive)
    {
        if (this._options == this._optionsIgnoreCase)
            return false;

        Rune runeAComparable = Rune.IsLower(runeA) ? Rune.ToUpper(runeA, this._culture) : Rune.ToLower(runeA, this._culture);
        return runeAComparable == runeB;
    }

    /// <summary>
    /// Compares two specified <see cref="ReadOnlySpan{Char}"/> using the specified rules, and returns an integer that indicates their relative position in
    /// the sort order.
    /// </summary>
    /// <param name="spanA">The first text to compare.</param>
    /// <param name="spanB">The second text instance.</param>
    /// <param name="caseInsensitive">Indicates whether current comparision should be case-insensitive.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description><paramref name="spanA"/> precedes <paramref name="spanB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Null</term>
    /// <description><paramref name="spanA"/> is in the same position as <paramref name="spanB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="spanA"/> follows <paramref name="spanB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Int32? Compare(ReadOnlySpan<Char> spanA, ReadOnlySpan<Char> spanB, Boolean caseInsensitive)
    {
        Int32 result = this._culture.CompareInfo.Compare(spanA, spanB, this.GetOptions(caseInsensitive));
        return result != 0 ? result : default(Int32?);
    }

    /// <summary>
    /// Retrieves the <see cref="CompareOptions"/> for current comparison.
    /// </summary>
    /// <param name="caseInsensitive">Indicates whether current comparision should be case-insensitive.</param>
    /// <returns><see cref="CompareOptions"/> for current comparison.</returns>
    private CompareOptions GetOptions(Boolean caseInsensitive) => !caseInsensitive ? this._options : this._optionsIgnoreCase;

    /// <summary>
    /// Retrieves <see cref="ReadOnlySpan{Char}"/> representation of <paramref name="source"/>>
    /// </summary>
    /// <param name="source">A read-only span of <typeparamref name="TChar"/> values that represents a text.</param>
    /// <returns>A <see cref="ReadOnlySpan{Char}"/> representation of <paramref name="source"/>.</returns>
    protected abstract ReadOnlySpan<Char> GetUnicodeSpan(ReadOnlySpan<TChar> source);

    /// <summary>
    /// Decodes the <see cref="Rune"/> at the beginning of the provided unicode source buffer.
    /// </summary>
    /// <param name="source">A read-only span of <see cref="Byte"/> that represents a text.</param>
    /// <param name="charsConsumed">
    /// Output. The number of units read to create the resulting <see cref="Rune"/>.
    /// </param>
    /// <returns>Decoded <see cref="Rune"/>.</returns>
    protected abstract Rune? DecodeRune(ReadOnlySpan<TChar> source, out Int32 charsConsumed);

    /// <summary>
    /// Retrieves the <see cref="ReadOnlySpan{Char}"/> representation of <paramref name="source"/>>
    /// </summary>
    /// <param name="source">A read-only span of <see cref="Byte"/> that represents a text.</param>
    /// <returns>A <see cref="ReadOnlySpan{Char}"/> representation of <paramref name="source"/>.</returns>
    protected static ReadOnlySpan<Char> GetUnicodeSpanFromUtf8(ReadOnlySpan<Byte> source) => Encoding.UTF8.GetString(source);

    /// <summary>
    /// Decodes the <see cref="Rune"/> at the beginning of the provided unicode source buffer.
    /// </summary>
    /// <param name="source">A read-only span of <see cref="Byte"/> that represents a text.</param>
    /// <param name="charsConsumed">
    /// Output. The number of units read to create the resulting <see cref="Rune"/>.
    /// </param>
    /// <returns>Decoded <see cref="Rune"/>.</returns>
    protected static Rune? DecodeRuneFromUtf8(ReadOnlySpan<Byte> source, out Int32 charsConsumed)
        => Rune.DecodeFromUtf8(source, out Rune result, out charsConsumed) == OperationStatus.Done ? result : default(Rune?);

    /// <summary>
    /// Indicates whether <paramref name="rune"/> is unicode.
    /// </summary>
    /// <param name="rune"><see cref="Rune"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="rune"/> is unicode; otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Boolean IsUnicode(Rune rune) => !rune.IsAscii && rune.IsBmp;
}

