using System.Globalization;

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
    /// Determines whether the text in <paramref name="utf8Chars"/> and the text in <paramref name="utf16Chars"/> have the same
    /// value. A parameter specifies the culture, case, and sort rules used in the comparison.
    /// </summary>
    /// <param name="utf8Chars">UTF-8 text characteres.</param>
    /// <param name="utf16Chars">UTF-16 text characteres.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies how the texts will be compared.</param>
    /// <returns><see langword="true"/> if both texts are equals; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Boolean TextEquals(ReadOnlySpan<Byte> utf8Chars, ReadOnlySpan<Char> utf16Chars, StringComparison comparisonType = StringComparison.Ordinal)
    {
        Boolean result = true;

        while (!utf8Chars.IsEmpty && !utf16Chars.IsEmpty && result)
        {
            Int32 bytesConsumed = 0;
            Int32 charsConsumed = 0;

            result =
                Rune.DecodeFromUtf8(utf8Chars, out Rune utf8Rune, out bytesConsumed) == OperationStatus.Done &&
                Rune.DecodeFromUtf16(utf16Chars, out Rune utf16Rune, out charsConsumed) == OperationStatus.Done &&
                RuneEquals(utf8Rune, utf16Rune, comparisonType);

            utf8Chars = utf8Chars.Slice(bytesConsumed);
            utf16Chars = utf16Chars.Slice(charsConsumed);
        }

        return result && utf8Chars.IsEmpty && utf16Chars.IsEmpty;
    }


    /// <summary>
    /// Determines whether the text in <paramref name="utf8Chars1"/> and the text in <paramref name="utf8Chars2"/> have the same
    /// value. A parameter specifies the culture, case, and sort rules used in the comparison.
    /// </summary>
    /// <param name="utf8Chars1">UTF-8 text characteres.</param>
    /// <param name="utf8Chars2">UTF-16 text characteres.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies how the texts will be compared.</param>
    /// <returns><see langword="true"/> if both texts are equals; otherwise, <see langword="false"/>.</returns>
    private static Boolean TextEquals(ReadOnlySpan<Byte> utf8Chars1, ReadOnlySpan<Byte> utf8Chars2, StringComparison comparisonType = StringComparison.Ordinal)
    {
        Boolean result = true;

        while (!utf8Chars1.IsEmpty && !utf8Chars2.IsEmpty && result)
        {
            Int32 bytesConsumed1 = 0;
            Int32 charsConsumed2 = 0;

            result =
                Rune.DecodeFromUtf8(utf8Chars1, out Rune utf8Rune, out bytesConsumed1) == OperationStatus.Done &&
                Rune.DecodeFromUtf8(utf8Chars2, out Rune utf16Rune, out charsConsumed2) == OperationStatus.Done &&
                RuneEquals(utf8Rune, utf16Rune, comparisonType);

            utf8Chars1 = utf8Chars1.Slice(bytesConsumed1);
            utf8Chars2 = utf8Chars2.Slice(charsConsumed2);
        }

        return result && utf8Chars1.IsEmpty && utf8Chars2.IsEmpty;
    }

    /// <summary>
    /// Determines whether the character in <paramref name="value1"/> and the character in <paramref name="value2"/> have the same
    /// value. A parameter specifies the culture, case, and sort rules used in the comparison.
    /// </summary>
    /// <param name="value1"><see cref="Rune"/> instance.</param>
    /// <param name="value2"><see cref="Rune"/> instance.</param>
    /// <param name="comparisonType">One of the enumeration values that specifies how the characters will be compared.</param>
    /// <returns><see langword="true"/> if both characters are equals; otherwise, <see langword="false"/>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Boolean RuneEquals(Rune value1, Rune value2, StringComparison comparisonType)
        => comparisonType == StringComparison.Ordinal ? value1 == value2 :
        Char.ConvertFromUtf32(value1.Value).Equals(Char.ConvertFromUtf32(value2.Value), comparisonType);

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