namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Base class for UTF-8 text comparator.
/// </summary>
/// <typeparam name="TChar">Type of the characters in the compared text.</typeparam>
internal abstract partial class Utf8Comparator<TChar> where TChar : unmanaged
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
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
    /// Constructor.
    /// </summary>
    /// <param name="ignoreCase">
    /// <see langword="true"/> to ignore case during the comparision; otherwise, <see langword="false"/>.
    /// </param>
    /// <param name="culture">
    /// An object that supplies culture-specific comparision information.
    /// If <paramref name="culture"/> is <see langword="null"/>, the current culture is used.
    /// </param>
    protected Utf8Comparator(Boolean ignoreCase, CultureInfo? culture)
    {
        this._culture = culture ?? CultureInfo.CurrentCulture;
        this._ignoreCase = ignoreCase;
        this._options = !ignoreCase ? CompareOptions.None : CompareOptions.IgnoreCase;
        this._optionsIgnoreCase = CompareOptions.IgnoreCase;
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
        if (this.UnsupportedCulture())
            return this.Compare(new(this._ignoreCase), textA, textB);

        //Creates a comparison state instance.
        ComparisonState state = new(this._ignoreCase);
        //Perform the initial comparison.
        Int32 result = this.Compare(state, ref textA, ref textB);

        //As long as the comparison status indicates that a new comparison is needed, we must perform
        //a new comparison.
        while (state.Continue)
        {
            Int32 newResult = this.Compare(state, ref textA, ref textB);
            //Only if both texts are different can the new comparison affect the result.
            if (newResult != 0)
                result = newResult;
        }

        //If the result is zero, we must determine if the length of both texts is equal.
        return result != 0 || textA.IsEmpty && textB.IsEmpty ? result : !textA.IsEmpty ? 1 : 0;
    }

    /// <summary>
    /// Determines whether the text in <paramref name="textA"/> and the text in <paramref name="textB"/> have the same
    /// value. A parameter specifies the culture, case, and sort rules used in the comparison.
    /// </summary>
    /// <param name="textA">The first text to equalize.</param>
    /// <param name="textB">The second text instance.</param>
    public Boolean TextEquals(ReadOnlySpan<Byte> textA, ReadOnlySpan<TChar> textB)
    {
        //Creates a comparison state instance.
        ComparisonState state = new(this._ignoreCase, true);
        Boolean result = this.Compare(state, ref textA, ref textB) == 0;
        //If the result is true, we must determine if the length of both texts is equal.
        return result && textA.IsEmpty && textB.IsEmpty;
    }

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
    /// <returns>Decoded <see cref="Rune"/>.</returns>
    protected abstract DecodedRune? DecodeRune(ref ReadOnlySpan<TChar> source);

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
    /// <returns>Decoded <see cref="Rune"/>.</returns>
    protected static DecodedRune? DecodeRuneFromUtf8(ref ReadOnlySpan<Byte> source)
    {
        DecodedRune? result = DecodedRune.Decode(source);
        if (result is not null)
            source = source[result.CharsConsumed..];
        return result;
    }
}

