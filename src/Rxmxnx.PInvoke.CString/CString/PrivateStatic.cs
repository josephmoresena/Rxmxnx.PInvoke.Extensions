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
    /// Encodes the UTF-16 text using the UTF-8 charset and retrieves the <see cref="Byte"/> array with 
    /// UTF-8 text.
    /// </summary>
    /// <param name="str"><see cref="String"/> representation of UTF-16 text.</param>
    /// <returns><see cref="Byte"/> array with UTF-8 text.</returns>
    private static Byte[] GetUtf8Bytes(String str) => !String.IsNullOrEmpty(str) ? Encoding.UTF8.GetBytes(str) : Array.Empty<Byte>();

    /// <summary>
    /// Creates a UTF-16 text which containst the binary information of <paramref name="str"/>
    /// encoded to UTF-8 text.
    /// </summary>
    /// <param name="str"><see cref="String"/> representation of UTF-16 text.</param>
    /// <returns>UTF-16 text containing the UTF-8 text binary information.</returns>
    private static String GetUtf8String(String str)
    {
        //Encodes String to UTF8 bytes.
        Byte[] bytes = GetUtf8Bytes(str);
        //Calculates the final UTF8 String length.
        Int32 length = GetUtf8StringLength(bytes);
        //Creates final String.
        String result = String.Create(bytes.Length, bytes, CopyBytes);

        //Try to fetch internal String
        return String.IsInterned(result) ?? result;
    }

    /// <summary>
    /// Copies the <paramref name="source"/> binary information into
    /// <paramref name="destination"/> span.
    /// </summary>
    /// <param name="destination">Span of <see cref="Char"/> values.</param>
    /// <param name="source">Array of <see cref="Byte"/> values.</param>
    private static void CopyBytes(Span<Char> destination, Byte[] source)
    {
        //Converts binary span into source char span.
        ReadOnlySpan<Char> sourceChars = MemoryMarshal.Cast<Byte, Char>(source);
        //Gets the binary size of source char span.
        Int32 offset = sourceChars.Length * sizeof(Char);
        //Creates the remaining bytes from source.
        ReadOnlySpan<Byte> remSource = source.AsSpan()[offset..];
        //Gets the remaining binary destination into destination span.
        Span<Byte> remDestination = MemoryMarshal.Cast<Char, Byte>(destination)[sourceChars.Length..];

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
        ReadOnlySpan<Byte> currSpan = current;
        ReadOnlySpan<Byte> otherSpan = other;

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
    private static Boolean SequenceEquals<T>(ReadOnlySpan<T> current, ReadOnlySpan<T> other)
        where T : unmanaged
    {
        for (Int32 i = 0; i < current.Length; i++)
            if (!current[i].Equals(other[i]))
                return false;
        return true;
    }

    /// <summary>
    /// Indicates whether <paramref name="data"/> contains a null-terminated UTF-8 text.
    /// </summary>
    /// <param name="data">A read-only byte span containing UTF-8 text.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="data"/> contains a null-terminated UTF-8 text; 
    /// otherwise, <see langword="false"/>.
    /// </returns>
    private static Boolean IsNullTerminatedSpan(ReadOnlySpan<Byte> data)
        => !data.IsEmpty && data[^1] == default;

    /// <summary>
    /// Retrieves the UTF8 String length which contains <paramref name="bytes"/>
    /// information.
    /// </summary>
    /// <param name="bytes">UTF8 bytes.</param>
    /// <returns>UTF8 String length contains <paramref name="bytes"/> information.</returns>
    private static Int32 GetUtf8StringLength(ReadOnlySpan<Byte> bytes)
    {
        Int32 result = (bytes.Length + 1) / sizeof(Char);
        if (bytes.Length % sizeof(Char) == 0)
            result++;
        return result;
    }
}