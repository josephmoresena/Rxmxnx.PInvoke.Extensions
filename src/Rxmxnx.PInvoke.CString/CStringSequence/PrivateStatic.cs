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
    /// Creates a new UTF-8 text sequence with a specific <paramref name="lengths"/> and initializes each
    /// UTF-8 texts into it after creation by using the specified callback.
    /// </summary>
    /// <typeparam name="TState">The type of the element to pass to <paramref name="action"/>.</typeparam>
    /// <param name="lengths">The lengths of the UTF-8 text sequence to create.</param>
    /// <param name="state">The element to pass to <paramref name="action"/>.</param>
    /// <param name="action">A callback to initialize each <see cref="CString"/>.</param>
    /// <returns>The create UTF-8 text sequence.</returns>
    public static CStringSequence Create<TState>(TState state, CStringSequenceCreationAction<TState> action, params Int32?[] lengths)
    {
        Int32 bytesLength = lengths.Where(l => l.GetValueOrDefault() > 0).Sum(l => l.GetValueOrDefault() + 1);
        Int32 length = bytesLength / SizeOfChar + (bytesLength % SizeOfChar);
        String buffer = String.Create(length, new SequenceCreationState<TState>(state, action, lengths), CreateCStringSequence);
        return new(buffer, lengths);
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
    private static unsafe void CopyText(Span<Char> charSpan, IReadOnlyList<CString?> values)
    {
        Int32 position = 0;
        fixed (void* charsPtr = &MemoryMarshal.GetReference(charSpan))
        {
            Span<Byte> byteSpan = new(charsPtr, charSpan.Length * SizeOfChar);
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
}

