namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Size of <see cref="Char"/> value in bytes.
	/// </summary>
	private const Int32 sizeOfChar = sizeof(Char);
	
	/// <summary>
	/// Represents an empty sequence.
	/// </summary>
	private static readonly CStringSequence empty = new(String.Empty, Array.Empty<Int32?>());

	/// <summary>
	/// Determines the length of the given <see cref="CString"/> instance for the sequence.
	/// </summary>
	/// <param name="cstr">The <see cref="CString"/> instance whose length is to be retrieved.</param>
	/// <returns>The length of the <paramref name="cstr"/> for the sequence.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32? GetLength(CString? cstr)
	{
		Int32? result = default;

		if (!CString.IsNullOrEmpty(cstr) || cstr?.IsReference == false)
			result = cstr.Length;

		return result;
	}
	/// <summary>
	/// Constructs a sequence buffer from the provided text collection.
	/// </summary>
	/// <param name="values">The collection of text.</param>
	/// <returns>
	/// A <see cref="String"/> instance that contains the binary information of the UTF-8 text sequence.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static String CreateBuffer(IReadOnlyList<CString?> values)
	{
		Int32 totalBytes = values.Where(value => value is not null && value.Length > 0).Sum(value => value!.Length + 1);
		Int32 totalChars = totalBytes / CStringSequence.sizeOfChar + totalBytes % CStringSequence.sizeOfChar;
		return String.Create(totalChars, values, CStringSequence.CopyText);
	}
	/// <summary>
	/// Copies the content of <see cref="CString"/> items from the provided <paramref name="values"/>
	/// to the given <paramref name="charSpan"/>.
	/// </summary>
	/// <param name="charSpan">
	/// The writable <see cref="Char"/> span that is the destination of the copy operation.
	/// </param>
	/// <param name="values">
	/// The collection of <see cref="CString"/> items whose contents are to be copied.
	/// </param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void CopyText(Span<Char> charSpan, IReadOnlyList<CString?> values)
	{
		Int32 position = 0;
		Span<Byte> byteSpan = MemoryMarshal.AsBytes(charSpan);
		foreach (CString value in values.Where(value => value is not null && value.Length > 0).Cast<CString>())
		{
			ReadOnlySpan<Byte> valueSpan = value.AsSpan();
			valueSpan.CopyTo(byteSpan[position..]);
			position += valueSpan.Length;
			byteSpan[position] = default;
			position++;
		}
	}
	/// <summary>
	/// Copies the content of the specified <paramref name="sequence"/> to the given
	/// <paramref name="charSpan"/>.
	/// </summary>
	/// <param name="charSpan">
	/// The writable <see cref="Char"/> span that is the destination of the copy operation.
	/// </param>
	/// <param name="sequence">
	/// The <see cref="CStringSequence"/> instance whose content is to be copied.
	/// </param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void CopySequence(Span<Char> charSpan, CStringSequence sequence)
	{
		ReadOnlySpan<Char> chars = sequence._value;
		chars.CopyTo(charSpan);
	}

	/// <summary>
	/// Performs a binary copy of all non-empty CString values in <paramref name="seq"/> to
	/// the <paramref name="destination"/> byte array.
	/// </summary>
	/// <param name="seq">The source <see cref="FixedCStringSequence"/> instance.</param>
	/// <param name="destination">The destination byte array.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void BinaryCopyTo(FixedCStringSequence seq, Byte[] destination)
	{
		Int32 offset = 0;
		foreach (CString value in seq.Values)
		{
			if (value.Length > 0)
			{
				ReadOnlySpan<Byte> bytes = value.AsSpan();
				bytes.CopyTo(destination.AsSpan()[offset..]);
				offset += bytes.Length;
			}
		}
	}
	/// <summary>
	/// Creates a UTF-8 text sequence using the given <paramref name="state"/>,
	/// with each UTF-8 text being initialized using the specified callback.
	/// </summary>
	/// <typeparam name="TState">The type of the element to pass to the action of the state.</typeparam>
	/// <param name="buffer">The UTF-16 buffer where the sequence is created.</param>
	/// <param name="state">The state object used for creation.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void CreateCStringSequence<TState>(Span<Char> buffer, SequenceCreationState<TState> state)
		=> CStringSequence.CreateCStringSequence(MemoryMarshal.AsBytes(buffer), state);
	/// <summary>
	/// Creates a UTF-8 text sequence using the given <paramref name="state"/>,
	/// with each UTF-8 text being initialized using the specified callback.
	/// </summary>
	/// <typeparam name="TState">The type of the element to pass to the action of the state.</typeparam>
	/// <param name="buffer">The byte buffer where the sequence is created.</param>
	/// <param name="state">The state object used for creation.</param>
	private static void CreateCStringSequence<TState>(Span<Byte> buffer, SequenceCreationState<TState> state)
	{
		Int32 offset = 0;
		for (Int32 i = 0; i < state.Lengths.Length; i++)
		{
			Int32? length = state.Lengths[i];
			if (length is not > 0)
				continue;
			state.InvokeAction(buffer.Slice(offset, length.Value), i);
			offset += length.Value + 1;
		}
	}
	/// <summary>
	/// Converts the given <paramref name="str"/> to a <see cref="CString"/> instance.
	/// </summary>
	/// <param name="str">The <see cref="String"/> instance to be converted.</param>
	/// <returns>
	/// A <see cref="CString"/> instance equivalent to <paramref name="str"/>, or <see langword="null"/> if
	/// <paramref name="str"/> is <see langword="null"/>.
	/// </returns>
	[return: NotNullIfNotNull(nameof(str))]
	private static CString? GetCString(String? str)
		=> str is not null ? CString.Create(Encoding.UTF8.GetBytes(str)) : default;
	/// <summary>
	/// Gets the length of the byte span representing a <see cref="CString"/> of the given <paramref name="length"/>.
	/// </summary>
	/// <param name="length">The length of the <see cref="CString"/>.</param>
	/// <returns>
	/// The length of the byte span representing a <see cref="CString"/> of <paramref name="length"/> length,
	/// or 0 if <paramref name="length"/> is <see langword="null"/> or non-positive.
	/// </returns>
	private static Int32 GetSpanLength(Int32? length) => length is > 0 ? length.Value + 1 : 0;
	/// <summary>
	/// Creates an array of <see cref="CString"/> values from given enumeration.
	/// </summary>
	/// <param name="values">Enumeration of <see cref="CString"/> instances.</param>
	/// <param name="array">Output. Resulting array as output parameter.</param>
	/// <returns>Resulting array.</returns>
	private static CString?[] FromArray(IEnumerable<CString?> values, out CString?[] array)
	{
		array = values.ToArray();
		return array;
	}
	/// <summary>
	/// Retrieves the length array for a given collection of UTF-8 texts.
	/// </summary>
	/// <param name="list">A collection of UTF-8 texts.</param>
	/// <returns>An array representing the length of each UTF-8 text in the collection.</returns>
	private static Int32?[] GetLengthArray(IReadOnlyList<CString?> list)
	{
		Int32?[] result = new Int32?[list.Count];
		for (Int32 i = 0; i < result.Length; i++)
			result[i] = CStringSequence.GetLength(list[i]);
		return result;
	}
}