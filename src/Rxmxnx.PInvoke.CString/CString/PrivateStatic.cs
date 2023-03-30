using System.Collections.Generic;
using System.Xml.Linq;

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
            Int32 bytesConsumed = 0;
            Int32 charsConsumed = 0;

            result =
                Rune.DecodeFromUtf8(textA, out Rune utf8Rune, out bytesConsumed) == OperationStatus.Done &&
                Rune.DecodeFromUtf16(textB, out Rune utf16Rune, out charsConsumed) == OperationStatus.Done &&
                RuneEquals(utf8Rune, utf16Rune, comparisonType);

            textA = textA.Slice(bytesConsumed);
            textB = textB.Slice(charsConsumed);
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
            Int32 bytesConsumed1 = 0;
            Int32 bytesConsumed2 = 0;

            result =
                Rune.DecodeFromUtf8(textA, out Rune utf8Rune, out bytesConsumed1) == OperationStatus.Done &&
                Rune.DecodeFromUtf8(textB, out Rune utf16Rune, out bytesConsumed2) == OperationStatus.Done &&
                RuneEquals(utf8Rune, utf16Rune, comparisonType);

            textA = textA.Slice(bytesConsumed1);
            textB = textB.Slice(bytesConsumed2);
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
    private static Int32 TextCompare(ReadOnlySpan<Byte> textA, ReadOnlySpan<Char> textB, StringComparison comparisonType = StringComparison.CurrentCulture)
    {
        Int32 result = 0;
        while (!textA.IsEmpty && !textB.IsEmpty && result == 0)
        {
            Boolean utf8Decoded = Rune.DecodeFromUtf8(textA, out Rune utf8Rune, out Int32 bytesConsumed) == OperationStatus.Done;
            Boolean utf16Decoded = Rune.DecodeFromUtf16(textB, out Rune utf16Rune, out Int32 charsConsumed) == OperationStatus.Done;

            textA = textA.Slice(bytesConsumed);
            textB = textB.Slice(charsConsumed);

            if (utf8Decoded && utf16Decoded)
                result = RuneCompare(utf8Rune, utf16Rune, comparisonType);
            else
                result = utf8Decoded ? 1 : 0;
        }

        return result != 0 || textA.IsEmpty && textB.IsEmpty ? result : !textA.IsEmpty ? 1 : 0;
    }

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
    {
        Int32 result = 0;
        while (!textA.IsEmpty && !textB.IsEmpty && result == 0)
        {
            Boolean utf8Decoded1 = Rune.DecodeFromUtf8(textA, out Rune utf8Rune1, out Int32 bytesConsumed1) == OperationStatus.Done;
            Boolean utf8Decoded2 = Rune.DecodeFromUtf8(textB, out Rune utf8Rune2, out Int32 bytesConsumed2) == OperationStatus.Done;

            textA = textA.Slice(bytesConsumed1);
            textB = textB.Slice(bytesConsumed2);

            if (utf8Decoded1 && utf8Decoded2)
                result = RuneCompare(utf8Rune1, utf8Rune2, comparisonType);
            else
                result = utf8Decoded1 ? 1 : 0;
        }

        return result != 0 || textA.IsEmpty && textB.IsEmpty ? result : !textA.IsEmpty ? 1 : 0;
    }

    /// <summary>
    /// Compares two specified <see cref="Rune"/> objects using the specified rules, and returns an integer that indicates their
    /// relative position in the sort order.
    /// </summary>
    /// <param name="runA">The first <see cref="Rune"/> to compare.</param>
    /// <param name="runB">The second <see cref="Rune"/> instance.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies the rules to use in the comparision.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the lexical relationship between the two comparands.
    /// <list type="table">
    /// <listheader>
    /// <term>Value</term><description>Condition</description>
    /// </listheader>
    /// <item>
    /// <term>Less than zero</term>
    /// <description><paramref name="runA"/> precedes <paramref name="runB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Zero</term>
    /// <description><paramref name="runA"/> is in the same position as <paramref name="runB"/> in the sort order.</description>
    /// </item>
    /// <item>
    /// <term>Greater than zero</term>
    /// <description><paramref name="runA"/> follows <paramref name="runB"/> in the sort order.</description>
    /// </item>
    /// </list>
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Int32 RuneCompare(Rune runA, Rune runB, StringComparison comparisonType)
        => comparisonType == StringComparison.Ordinal ? runA.CompareTo(runB)  :
        String.Compare(Char.ConvertFromUtf32(runA.Value), Char.ConvertFromUtf32(runB.Value), comparisonType);

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