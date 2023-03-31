namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Retrives a <see cref="EqualsDelegate"/> delegate for native comparision.
    /// </summary>
    /// <returns><see cref="EqualsDelegate"/> delegate.</returns>
    [ExcludeFromCodeCoverage]
    private static EqualsDelegate GetEquals() => Environment.Is64BitProcess ? Equals<Int64> : Equals<Int32>;

    /// <summary>
    /// Copies the <paramref name="source"/> binary information into
    /// <paramref name="destination"/> span.
    /// </summary>
    /// <param name="destination">Span of <see cref="Char"/> values.</param>
    /// <param name="source">Array of <see cref="Byte"/> values.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyBytes(Span<Char> destination, Byte[] source)
    {
        //Converts binary span into source char span.
        ReadOnlySpan<Char> sourceChars = MemoryMarshal.Cast<Byte, Char>(source);
        //Gets the binary size of source char span.
        Int32 offset = sourceChars.Length * sizeof(Char);
        //Creates the remaining bytes from source.
        ReadOnlySpan<Byte> remSource = source.AsSpan()[offset..];
        //Gets the remaining binary destination into destination span.
        Span<Byte> remDestination = MemoryMarshal.AsBytes(destination[sourceChars.Length..]);

        //Copies the source char span into destination span.
        sourceChars.CopyTo(destination);
        //Copies the remaining binary span into UTF8 destination span.
        remSource.CopyTo(remDestination);
    }

    /// <summary>
    /// Indicates whether <paramref name="current"/> <see cref="ReadOnlySpan{Byte}"/>
    /// is equal to 
    /// <paramref name="other"/> <see cref="ReadOnlySpan{Byte}"/>.
    /// instance.
    /// </summary>
    /// <typeparam name="TInteger"><see cref="ValueType"/> for reduce comparation.</typeparam>
    /// <param name="current">A <see cref="CString"/> to compare with <paramref name="other"/>.</param>
    /// <param name="other">A <see cref="CString"/> to compare with this <paramref name="current"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="current"/> <see cref="CString"/> is equal to 
    /// <paramref name="other"/> parameter; otherwise, <see langword="false"/>.
    /// </returns>
    private static unsafe Boolean Equals<TInteger>(ReadOnlySpan<Byte> current, ReadOnlySpan<Byte> other)
        where TInteger : unmanaged
    {
        if (current.Length == other.Length)
        {
            ReadOnlySpan<TInteger> currentIntegers = MemoryMarshal.Cast<Byte, TInteger>(current);
            ReadOnlySpan<TInteger> otherIntegers = MemoryMarshal.Cast<Byte, TInteger>(other);

            if (SequenceEquals(currentIntegers, otherIntegers))
            {
                Int32 binaryOffset = currentIntegers.Length * sizeof(TInteger);
                return SequenceEquals(current[binaryOffset..], other[binaryOffset..]);
            }
        }

        return false;
    }

    /// <summary>
    /// Indicates whether <paramref name="current"/> <see cref="ReadOnlySpan{T}"/>
    /// is equal to 
    /// <paramref name="other"/> <see cref="ReadOnlySpan{T}"/>.
    /// instance.
    /// </summary>
    /// <typeparam name="T"><see cref="ValueType"/> of span.</typeparam>
    /// <param name="current">A <see cref="ReadOnlySpan{T}"/> to compare with <paramref name="other"/>.</param>
    /// <param name="other">A <see cref="ReadOnlySpan{T}"/> to compare with this <paramref name="current"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="current"/> <see cref="ReadOnlySpan{T}"/> is equal to 
    /// <paramref name="other"/> parameter; otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Boolean SequenceEquals<T>(ReadOnlySpan<T> current, ReadOnlySpan<T> other)
        where T : unmanaged
    {
        for (Int32 i = 0; i < current.Length; i++)
            if (!current[i].Equals(other[i]))
                return false;
        return true;
    }

    /// <summary>
    /// Determines whether the text in <paramref name="textA"/> and the text in <paramref name="textB"/> have the same
    /// value. A parameter specifies the culture, case, and sort rules used in the comparison.
    /// </summary>
    /// <param name="textA">UTF-8 text characteres.</param>
    /// <param name="textB">UTF-16 text characteres.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies how the texts will be compared.</param>
    /// <returns><see langword="true"/> if both texts are equals; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Boolean TextEquals(ReadOnlySpan<Byte> textA, ReadOnlySpan<Char> textB, StringComparison comparisonType = StringComparison.CurrentCulture)
    {
        Boolean result = true;

        while (!textA.IsEmpty && !textB.IsEmpty && result)
        {
            Int32 charsConsumed = 0;

            result =
                Rune.DecodeFromUtf8(textA, out Rune utf8Rune, out Int32 bytesConsumed) == OperationStatus.Done &&
                Rune.DecodeFromUtf16(textB, out Rune utf16Rune, out charsConsumed) == OperationStatus.Done &&
                RuneEquals(utf8Rune, utf16Rune, comparisonType);

            textA = textA[bytesConsumed..];
            textB = textB[charsConsumed..];
        }

        return result && textA.IsEmpty && textB.IsEmpty;
    }

    /// <summary>
    /// Determines whether the text in <paramref name="textA"/> and the text in <paramref name="textB"/> have the same
    /// value. A parameter specifies the culture, case, and sort rules used in the comparison.
    /// </summary>
    /// <param name="textA">UTF-8 text characteres.</param>
    /// <param name="textB">UTF-16 text characteres.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies how the texts will be compared.</param>
    /// <returns><see langword="true"/> if both texts are equals; otherwise, <see langword="false"/>.</returns>
    private static Boolean TextEquals(ReadOnlySpan<Byte> textA, ReadOnlySpan<Byte> textB, StringComparison comparisonType = StringComparison.CurrentCulture)
    {
        Boolean result = true;

        while (!textA.IsEmpty && !textB.IsEmpty && result)
        {
            Int32 bytesConsumed2 = 0;

            result =
                Rune.DecodeFromUtf8(textA, out Rune utf8Rune, out Int32 bytesConsumed1) == OperationStatus.Done &&
                Rune.DecodeFromUtf8(textB, out Rune utf16Rune, out bytesConsumed2) == OperationStatus.Done &&
                RuneEquals(utf8Rune, utf16Rune, comparisonType);

            textA = textA[bytesConsumed1..];
            textB = textB[bytesConsumed2..];
        }

        return result && textA.IsEmpty && textB.IsEmpty;
    }

    /// <summary>
    /// Determines whether the character in <paramref name="runA"/> and the character in <paramref name="runB"/> have the same
    /// value. A parameter specifies the culture, case, and sort rules used in the comparison.
    /// </summary>
    /// <param name="runA"><see cref="Rune"/> instance.</param>
    /// <param name="runB"><see cref="Rune"/> instance.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies how the characters will be compared.</param>
    /// <returns><see langword="true"/> if both characters are equals; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Boolean RuneEquals(Rune runA, Rune runB, StringComparison comparisonType)
        => comparisonType == StringComparison.Ordinal ? runA == runB :
        Char.ConvertFromUtf32(runA.Value).Equals(Char.ConvertFromUtf32(runB.Value), comparisonType);

    /// <summary>
    /// Compares two specified texts using the specified rules, and returns an integer that indicates their relative position in
    /// the sort order.
    /// </summary>
    /// <param name="textA">The first text to compare.</param>
    /// <param name="textB">The second text instance.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
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
    private static Int32 TextCompare(ReadOnlySpan<Byte> textA, ReadOnlySpan<Byte> textB, StringComparison comparisonType = StringComparison.CurrentCulture)
        => Compare(textA, textB, Rune.DecodeFromUtf8, Encoding.UTF8.GetString, comparisonType);

    /// <summary>
    /// Compares two specified texts using the specified rules, and returns an integer that indicates their relative position in
    /// the sort order.
    /// </summary>
    /// <param name="textA">The first text to compare.</param>
    /// <param name="textB">The second text instance.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
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
    private static Int32 TextCompare(ReadOnlySpan<Byte> textA, ReadOnlySpan<Char> textB, StringComparison comparisonType = StringComparison.CurrentCulture)
        => Compare(textA, textB, Rune.DecodeFromUtf16, CreateString, comparisonType);

    /// <summary>
    /// Compares two specified texts using the specified rules, and returns an integer that indicates their relative position in
    /// the sort order.
    /// </summary>
    /// <param name="textA">The first text to compare.</param>
    /// <param name="textB">The second text instance.</param>
    /// <param name="decodeRune">Delegate for getting <see cref="Rune"/> instances from <paramref name="textB"/>.</param>
    /// <param name="getString">Delegate for getting <see cref="String"/> representation of <paramref name="textB"/>.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
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
    private static Int32 Compare<T>(ReadOnlySpan<Byte> textA, ReadOnlySpan<T> textB, DecodeRuneFrom<T> decodeRune, GetStringDelegate<T> getString, StringComparison comparisonType)
        where T : unmanaged
    {
        Int32 result = 0;
        StringComparison? ignoreCaseComparison = default;

        while (!textA.IsEmpty && !textB.IsEmpty && result == 0)
        {
            Boolean decodedA = Rune.DecodeFromUtf8(textA, out Rune runA, out Int32 consumedA) == OperationStatus.Done;
            Boolean decodedB = decodeRune(textB, out Rune runB, out Int32 consumedB) == OperationStatus.Done;

            if (!decodedA || !decodedB)
                return decodedA ? 1 : decodedB ? -1 : 0;

            if (runA != runB)
            {
                Int32? tmpResult = Compare(runA, runB, textA, textB, comparisonType, getString, ref ignoreCaseComparison);
                if (!tmpResult.HasValue)
                    return 0;
                result = tmpResult.Value;
            }

            textA = textA[consumedA..];
            textB = textB[consumedB..];
        }

        if (ignoreCaseComparison.HasValue)
        {
            Int32 resultIgnoreCase = Compare(textA, textB, decodeRune, getString, ignoreCaseComparison.Value);
            if (resultIgnoreCase != 0)
                result = resultIgnoreCase;
        }

        return result != 0 || textA.IsEmpty && textB.IsEmpty ? result : !textA.IsEmpty ? 1 : 0;
    }

    /// <summary>
    /// Compares two specified texts using the specified rules, and returns an integer that indicates their relative position in
    /// the sort order.
    /// </summary>
    /// <typeparam name="T">Type of chars in <paramref name="textB"/>.</typeparam>
    /// <param name="runeA">First rune in <paramref name="textA"/>.</param>
    /// <param name="runeB">First rune in <paramref name="textB"/>.</param>
    /// <param name="textA">The first text to compare.</param>
    /// <param name="textB">The second text instance.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    /// <param name="getString">Delegate for getting <see cref="String"/> representation of <paramref name="textB"/>.</param>
    /// <param name="ignoreCaseComparison">
    /// Output. The case insensitive comparison in which <paramref name="runeA"/> and <paramref name="runeB"/> 
    /// are in the same position.
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
    private static Int32? Compare<T>(Rune runeA, Rune runeB, ReadOnlySpan<Byte> textA, ReadOnlySpan<T> textB, StringComparison comparisonType, GetStringDelegate<T> getString, ref StringComparison? ignoreCaseComparison)
        where T : unmanaged
    {
        Boolean unicodeA = IsUnicode(runeA);
        Boolean unicodeB = IsUnicode(runeB);

        if (unicodeA && unicodeB && !IgnoreCaseEqual(runeA, runeB, comparisonType))
        {
            String strA = Encoding.UTF8.GetString(textA);
            String strb = getString(textB);

            return Compare(strA, strb, comparisonType);
        }

        return Compare(runeA, runeB, comparisonType, ref ignoreCaseComparison);
    }

    /// <summary>
    /// Compares two specified <see cref="String"/> objects using the specified rules, and returns an integer that indicates their relative position in
    /// the sort order.
    /// </summary>
    /// <param name="strA">The first text to compare.</param>
    /// <param name="strB">The second text instance.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description><paramref name="strA"/> precedes <paramref name="strB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Null</term>
    /// <description><paramref name="strA"/> is in the same position as <paramref name="strB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="strA"/> follows <paramref name="strB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Int32? Compare(String strA, String strB, StringComparison comparisonType)
    {
        Int32 result = String.Compare(strA, strB, comparisonType);
        return result != 0 ? result : default(Int32?);
    }

    /// <summary>
    /// Compares two specified <see cref="Rune"/> objects using the specified rules, and returns an integer that indicates their
    /// relative position in the sort order.
    /// </summary>
    /// <param name="runeA">The first <see cref="Rune"/> to compare.</param>
    /// <param name="runeB">The second <see cref="Rune"/> instance.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    /// <param name="ignoreCaseComparison">
    /// Output. The case insensitive comparison in which <paramref name="runeA"/> and <paramref name="runeB"/> 
    /// are in the same position.
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
    private static Int32 Compare(Rune runeA, Rune runeB, StringComparison comparisonType, ref StringComparison? ignoreCaseComparison)
    {
        String strA = Char.ConvertFromUtf32(runeA.Value);
        String strB = Char.ConvertFromUtf32(runeB.Value);
        Int32 result = String.Compare(strA, strB, comparisonType);

        if (result != 0 && GetIgnoreCaseComparison(comparisonType) is StringComparison ignoreCaseComparisonTmp)
        {
            String strANormalizedBase = strA.Normalize(NormalizationForm.FormD)[..1];
            String strBNormalizedBase = strB.Normalize(NormalizationForm.FormD)[..1];

            if (String.Compare(strANormalizedBase, strBNormalizedBase, ignoreCaseComparisonTmp) == 0)
                ignoreCaseComparison = ignoreCaseComparisonTmp;
        }

        return result;
    }

    /// <summary>
    /// Retrieves the case insensitive <see cref="StringComparison"/> value that corresponds to <paramref name="comparisonType"/>.
    /// </summary>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    /// <returns>The case insensitive <see cref="StringComparison"/> value that corresponds to <paramref name="comparisonType"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static StringComparison? GetIgnoreCaseComparison(StringComparison comparisonType)
        => comparisonType switch
        {
            StringComparison.CurrentCulture => StringComparison.CurrentCultureIgnoreCase,
            StringComparison.CurrentCultureIgnoreCase => StringComparison.CurrentCultureIgnoreCase,
            StringComparison.InvariantCulture => StringComparison.InvariantCultureIgnoreCase,
            StringComparison.InvariantCultureIgnoreCase => StringComparison.InvariantCultureIgnoreCase,
            _ => default(StringComparison?)
        };

    /// <summary>
    /// Indicates whether <paramref name="runeA"/> and <paramref name="runeB"/> are equal ignoring case.
    /// </summary>
    /// <param name="runeA"><see cref="Rune"/> instance.</param>
    /// <param name="runeB"><see cref="Rune"/> instance.</param>
    /// <param name="comparisonType">
    /// One of the enumeration values that specifies the rules to use in the comparision.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="runeA"/> and <paramref name="runeB"/> are equal ignoring 
    /// case; otherwise, <see langword="false"/>.
    /// </returns>
    private static Boolean IgnoreCaseEqual(Rune runeA, Rune runeB, StringComparison comparisonType)
    {
        CultureInfo? culture = GetIgnoreCaseComparison(comparisonType) switch
        {
            StringComparison.InvariantCultureIgnoreCase => CultureInfo.InvariantCulture,
            StringComparison.CurrentCultureIgnoreCase => CultureInfo.CurrentCulture,
            _ => default,
        };

        if (culture is null)
            return false;

        Rune runeAComparable = Rune.IsLower(runeA) ? Rune.ToUpper(runeA, culture) : Rune.ToLower(runeA, culture);
        return runeAComparable == runeB;
    }

    /// <summary>
    /// Indicates whether <paramref name="rune"/> is unicode.
    /// </summary>
    /// <param name="rune"><see cref="Rune"/> instance.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="rune"/> is unicode; otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Boolean IsUnicode(Rune rune) => !rune.IsAscii && rune.IsBmp;

    /// <summary>
    /// Creates a <see cref="String"/> from the unicode characters indicated in the specified read-only span. 
    /// </summary>
    /// <param name="chars">A read-only span of unicode charateres.</param>
    /// <returns>The <see cref="String"/> representation of <paramref name="chars"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static String CreateString(ReadOnlySpan<Char> chars) => new(chars);

    /// <summary>
    /// Indicates whether <paramref name="data"/> contains a null-terminated UTF-8 text.
    /// </summary>
    /// <param name="data">A read-only byte span containing UTF-8 text.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="data"/> contains a null-terminated UTF-8 text; 
    /// otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Boolean IsNullTerminatedSpan(ReadOnlySpan<Byte> data)
        => !data.IsEmpty && data[^1] == default;
}