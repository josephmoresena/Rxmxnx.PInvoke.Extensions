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

    /// <summary>
    /// Indicates whether <paramref name="data"/> contains a null-terminated UTF-8 text.
    /// </summary>
    /// <param name="data">A read-only byte span containing UTF-8 text.</param>
    /// <param name="textLength">Output. The length of the UTF-8 text in <paramref name="data"/>.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="data"/> contains a null-terminated UTF-8 text; 
    /// otherwise, <see langword="false"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Boolean IsNullTerminatedSpan(ReadOnlySpan<Byte> data, out Int32 textLength)
    {
        textLength = data.Length;
        while (textLength > 0 && data[textLength - 1] == default)
            textLength--;
        return textLength < data.Length;
    }

    /// <summary>
    /// Creates a null-terminated UTF-8 text that only contains the character <paramref name="c"/>
    /// <paramref name="count"/> times.
    /// </summary>
    /// <param name="c">A UTF-8 char.</param>
    /// <param name="count">The number of the times <paramref name="c"/> occours.</param>
    /// <returns>
    /// A null-terminated UTF-8 text that only contains the character <paramref name="c"/>
    /// <paramref name="count"/> times.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Byte[] CreateRepeatedChar(Byte c, Int32 count)
    {
        Byte[] result = new Byte[count + 1];
        for (Int32 i = 0; i < count; i++)
            result[i] = c;
        return result;
    }

    /// <summary>
    /// Creates a null-terminated UTF-8 text that only contains the sequence <paramref name="seq"/>
    /// <paramref name="count"/> times.
    /// </summary>
    /// <param name="seq">A UTF-8 sequence.</param>
    /// <param name="count">The number of the times <paramref name="seq"/> occours.</param>
    /// <returns>
    /// A null-terminated UTF-8 text that only contains the sequence <paramref name="seq"/>
    /// <paramref name="count"/> times.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Byte[] CreateRepeatedSequence(ReadOnlySpan<Byte> seq, Int32 count)
    {
        Byte[] result = new Byte[seq.Length * count + 1];
        for (Int32 i = 0; i < count; i++)
            seq.CopyTo(result.AsSpan()[(seq.Length * i)..]);
        return result;
    }
}