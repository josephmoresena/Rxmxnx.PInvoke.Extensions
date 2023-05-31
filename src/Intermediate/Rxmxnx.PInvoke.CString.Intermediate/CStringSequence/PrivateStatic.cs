namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
    /// <summary>
    /// Empty sequence.
    /// </summary>
    private static readonly CStringSequence empty = new(String.Empty, Array.Empty<Int32?>());

    /// <summary>
    /// Retrieves the length of <paramref name="cstr"/> for the sequence.
    /// </summary>
    /// <param name="cstr"><see cref="CString"/> instance.</param>
    /// <returns>Length of <paramref name="cstr"/> for the sequence.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Int32? GetLength(CString? cstr)
    {
        Int32? result = default;

        if (!CString.IsNullOrEmpty(cstr) || cstr?.IsReference == false)
            result = cstr.Length;

        return result;
    }

    /// <summary>
    /// Creates the sequence buffer.
    /// </summary>
    /// <param name="values">Text collection.</param>
    /// <returns>
    /// <see cref="String"/> instance that contains the binary information of the UTF-8 text sequence.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static String CreateBuffer(IReadOnlyList<CString?> values)
    {
        Int32 totalBytes = 0;
        for (Int32 i = 0; i < values.Count; i++)
            if (values[i] is CString value && value.Length > 0)
                totalBytes += value.Length + 1;
        Int32 totalChars = (totalBytes / SizeOfChar) + (totalBytes % SizeOfChar);
        return String.Create(totalChars, values, CopyText);
    }

    /// <summary>
    /// Copy the content of <see cref="CString"/> items in <paramref name="values"/> to
    /// <paramref name="charSpan"/> span.
    /// </summary>
    /// <param name="charSpan">A writable <see cref="Char"/> span.</param>
    /// <param name="values">A enumeration of <see cref="CString"/> items.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopyText(Span<Char> charSpan, IReadOnlyList<CString?> values)
    {
        Int32 position = 0;
        Span<Byte> byteSpan = MemoryMarshal.AsBytes(charSpan);
        for (Int32 i = 0; i < values.Count; i++)
            if (values[i] is CString value && value.Length > 0)
            {
                ReadOnlySpan<Byte> valueSpan = value.AsSpan();
                valueSpan.CopyTo(byteSpan[position..]);
                position += valueSpan.Length;
                byteSpan[position] = default;
                position++;
            }
    }

    /// <summary>
    /// Copy the content of <paramref name="sequence"/> to <paramref name="charSpan"/>.
    /// </summary>
    /// <param name="charSpan">A writable <see cref="Char"/> span.</param>
    /// <param name="sequence">A <see cref="CStringSequence"/> instance.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CopySequence(Span<Char> charSpan, CStringSequence sequence)
    {
        ReadOnlySpan<Char> chars = sequence._value;
        chars.CopyTo(charSpan);
    }

    /// <summary>
    /// Performs a binary copy of all non-empty <paramref name="seq"/> to 
    /// <paramref name="destination"/> span.
    /// </summary>
    /// <param name="seq">A <see cref="FixedCStringSequence"/> instance.</param>
    /// <param name="destination">The destination binary buffer.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void BinaryCopyTo(FixedCStringSequence seq, Byte[] destination)
    {
        Int32 offset = 0;
        foreach (CString value in seq.Values)
            if (value.Length > 0)
            {
                ReadOnlySpan<Byte> bytes = value.AsSpan();
                bytes.CopyTo(destination.AsSpan()[offset..]);
                offset += bytes.Length;
            }
    }

    /// <summary>
    /// Performs the creation of the UTF-8 text sequence with a specific <paramref name="state"/>.
    /// Each UTF-8 text is initialized using the specified callback.
    /// </summary>
    /// <typeparam name="TState">The type of the element to pass to state's action.</typeparam>
    /// <param name="buffer">UTF-16 buffer.</param>
    /// <param name="state">Creation state object.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void CreateCStringSequence<TState>(Span<Char> buffer, SequenceCreationState<TState> state)
        => CreateCStringSequence(MemoryMarshal.AsBytes(buffer), state);

    /// <summary>
    /// Performs the creation of the UTF-8 text sequence with a specific <paramref name="state"/>.
    /// Each UTF-8 text is initialized using the specified callback.
    /// </summary>
    /// <typeparam name="TState">The type of the element to pass to state's action.</typeparam>
    /// <param name="buffer">UTF-16 buffer.</param>
    /// <param name="state">Creation state object.</param>
    private static void CreateCStringSequence<TState>(Span<Byte> buffer, SequenceCreationState<TState> state)
    {
        Int32 offset = 0;
        for (Int32 i = 0; i < state.Lengths.Length; i++)
        {
            Int32? length = state.Lengths[i];
            if (length.HasValue && length.Value > 0)
            {
                state.InvokeAction(buffer.Slice(offset, length.Value), i);
                offset += length.Value + 1;
            }
        }
    }

    /// <summary>
    /// Retrieves a <see cref="CString"/> instance from <paramref name="str"/>.
    /// </summary>
    /// <param name="str"><see cref="String"/> instance.</param>
    /// <returns>A <see cref="CString"/> instance from <paramref name="str"/>.</returns>
    [return: NotNullIfNotNull(nameof(str))]
    private static CString? GetCString(String? str)
        => str is not null ? CString.Create(Encoding.UTF8.GetBytes(str)) : default;

    /// <summary>
    /// Retrieves the length of the <see cref="ReadOnlySpan{Byte}"/> of a <see cref="CString"/> of
    /// <paramref name="length"/> length.
    /// </summary>
    /// <param name="length"><see cref="CString"/> length.</param>
    /// <returns>
    /// Length of the <see cref="ReadOnlySpan{Byte}"/> of a <see cref="CString"/> of
    /// <paramref name="length"/> length.
    /// </returns>
    private static Int32 GetSpanLength(Int32? length)
        => length.HasValue && length.Value > 0 ? length.Value + 1 : 0;
}

