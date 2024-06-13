namespace Rxmxnx.PInvoke;

[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
public unsafe partial class CStringSequence
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
		for (Int32 index = 0; index < values.Count; index++)
		{
			CString? value = values[index];
			if (value is null || value.Length == 0) continue;
			ReadOnlySpan<Byte> valueSpan = value;
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
		for (Int32 index = 0; index < seq.Values.Count; index++)
		{
			CString value = seq.Values[index];
			if (value.Length <= 0) continue;
			ReadOnlySpan<Byte> bytes = value.AsSpan();
			bytes.CopyTo(destination.AsSpan()[offset..]);
			offset += bytes.Length;
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
	/// <summary>
	/// Creates cache for <paramref name="lengths"/>.
	/// </summary>
	/// <param name="lengths">The lengths of the UTF-8 text sequence.</param>
	/// <returns>Instance cache.</returns>
	private static IList<CString?> CreateCache(IReadOnlyList<Int32?> lengths)
	{
		List<Int32> emptyIndices =
			CStringSequence.GetEmptyIndexList(lengths, out Int32 count, out Int32 lastNonEmpty, out Int32 skipLast);

		// All elements are empty, the cache is an empty array.
		if (emptyIndices.Count == lengths.Count) return Array.Empty<CString>();

		// There is no empty elements or there are only at the end of the list
		if (emptyIndices.Count == 0 || (emptyIndices.Count - skipLast == 1 && lastNonEmpty + 1 == emptyIndices[0]))
			return lengths.Count switch
			{
				<= 32 => new CString?[count],
				<= 256 => FixedCache.CreateFixedCache(count, ImmutableHashSet<Int32>.Empty),
				_ => new DynamicCache(),
			};

		// Otherwise
		return lengths.Count switch
		{
			<= 256 => FixedCache.CreateFixedCache(count, emptyIndices.SkipLast(skipLast).ToImmutableHashSet()),
			_ => new DynamicCache(),
		};
	}
	/// <summary>
	/// Retrieves the gaps list from <paramref name="lengths"/>.
	/// </summary>
	/// <param name="lengths">Length of each UTF-8 text in the sequence.</param>
	/// <param name="totalNonEmpty">Output. Count of non-empty UTF-8 texts.</param>
	/// <param name="lastNonEmpty">Output. Index of last non-empty UTF-8 text.</param>
	/// <param name="skipLast">Output. Count of useless elements at the end of resulting list.</param>
	/// <returns>A list containing the indices of all empty UTF-8 in the sequence.</returns>
	private static List<Int32> GetEmptyIndexList(IReadOnlyList<Int32?> lengths, out Int32 totalNonEmpty,
		out Int32 lastNonEmpty, out Int32 skipLast)
	{
		List<Int32> result = new(lengths.Count);
		lastNonEmpty = -1;
		// Fills gaps list and determines latest non-empty element.
		for (Int32 i = 0; i < lengths.Count; i++)
		{
			if (lengths[i].GetValueOrDefault() != 0)
				lastNonEmpty = i;
			else
				result.Add(i);
		}
		// Determines total non-empty elements.
		totalNonEmpty = lengths.Count - result.Count;
		// Determines how many items can be skipped to the end of the list.
		skipLast = 0;
		for (Int32 i = result.Count - 1; i > 0; i--)
		{
			if (result[i] - 1 != result[i - 1] || result[^1] != lengths.Count - 1) break;
			skipLast++;
		}
		return result;
	}
	/// <summary>
	/// Calculates buffer's length.
	/// </summary>
	/// <param name="lengths">The lengths of the UTF-8 text sequence to create.</param>
	/// <returns><see cref="String"/> buffer length.</returns>
	private static Int32 GetBufferLength(IEnumerable<Int32?> lengths)
	{
		Int32 bytesLength = lengths.Sum(CStringSequence.GetSpanLength);
		Int32 length = bytesLength / CStringSequence.sizeOfChar + bytesLength % CStringSequence.sizeOfChar;
		return length;
	}
	/// <summary>
	/// Retrieves usable UTF-8 buffer from <paramref name="sourceChars"/>.
	/// </summary>
	/// <param name="sourceChars">A buffer of a UTF-8 sequence.</param>
	/// <param name="isParsable">Indicates whether resulting buffer is parsable.</param>
	/// <returns>A UTF-8 buffer.</returns>
	private static ReadOnlySpan<Byte> GetSourceBuffer(ReadOnlySpan<Char> sourceChars, out Boolean isParsable)
	{
		ReadOnlySpan<Byte> bufferSpan = MemoryMarshal.AsBytes(sourceChars);
		do
		{
			if (bufferSpan.Length == 0)
			{
				isParsable = true;
				return bufferSpan;
			}
			bufferSpan = bufferSpan[1..];
		} while (bufferSpan[0] == default);
		isParsable = bufferSpan.Length != sourceChars.Length * CStringSequence.sizeOfChar || bufferSpan[^1] == default;
		return bufferSpan;
	}
	/// <summary>
	/// Creates a new <see cref="CStringSequence"/> instance from <paramref name="buffer"/>.
	/// </summary>
	/// <param name="buffer">A buffer of a UTF-8 sequence.</param>
	/// <returns>A new <see cref="CStringSequence"/> instance.</returns>
	private static CStringSequence CreateFrom(ReadOnlySpan<Byte> buffer)
	{
		if (buffer.Length == 0) return CStringSequence.empty;
		Int32 sequenceBufferLength = buffer.Length + (buffer[^1] == default ? 0 : 1);
		ReadOnlySpan<Int32> nulls;
		String sequenceBuffer;
		fixed (Byte* ptr = &MemoryMarshal.GetReference(buffer))
		{
			SequenceCreationState state = new() { Pointer = ptr, Length = buffer.Length, NullChars = [], };
			sequenceBuffer = String.Create(sequenceBufferLength, state, CStringSequence.CreateBuffer);
			nulls = CollectionsMarshal.AsSpan(state.NullChars);
		}
		Int32?[] lengths = CStringSequence.GetLengths(nulls);
		return new(sequenceBuffer, lengths);
	}
	/// <summary>
	/// Creates a new <see cref="CStringSequence"/> instance from <paramref name="buffer"/>.
	/// </summary>
	/// <param name="buffer">A buffer of a UTF-8 sequence.</param>
	/// <returns>A new <see cref="CStringSequence"/> instance.</returns>
	private static CStringSequence CreateFrom(String buffer)
	{
		List<Int32> nulls = [];
		ReadOnlySpan<Byte> span = MemoryMarshal.AsBytes(buffer.AsSpan());
		for (Int32 i = 0; i < buffer.Length; i++)
		{
			if (span[i] == default)
				nulls.Add(i);
		}
		Int32?[] lengths = CStringSequence.GetLengths(CollectionsMarshal.AsSpan(nulls));
		return new(buffer, lengths);
	}
	/// <summary>
	/// Creates a UTF-8 text sequence using the given <paramref name="state"/>,
	/// with each UTF-8 text being initialized using the specified callback.
	/// </summary>
	/// <param name="buffer">The UTF-16 buffer where the sequence is created.</param>
	/// <param name="state">The state object used for creation.</param>
	private static void CreateBuffer(Span<Char> buffer, SequenceCreationState state)
	{
		Span<Byte> destination = MemoryMarshal.AsBytes(buffer);
		ReadOnlySpan<Byte> sourceSpan = new(state.Pointer, state.Length);
		for (Int32 i = 0; i < sourceSpan.Length; i++)
		{
			destination[i] = sourceSpan[i];
			if (destination[i] == default) state.NullChars.Add(i);
		}
	}
	/// <summary>
	/// Creates buffer using <paramref name="ptrSpan"/> and <paramref name="lengths"/>.
	/// </summary>
	/// <param name="ptrSpan">Pointer to pointer span.</param>
	/// <param name="lengths">UTF-8 text lengths.</param>
	/// <returns>Created buffer.</returns>
	private static String CreateBuffer(void* ptrSpan, Int32?[] lengths)
	{
		Int32 bufferLength = CStringSequence.GetBufferLength(lengths);
		SpanCreationInfo info = new() { Pointers = ptrSpan, Lengths = lengths, };
		return String.Create(bufferLength, info, CStringSequence.CreateBuffer);
	}
	/// <summary>
	/// Create buffer using <paramref name="info"/>.
	/// </summary>
	/// <param name="charSpan">A <see cref="Span{Char}"/> instance.</param>
	/// <param name="info">A <see cref="SpanCreationInfo"/> value.</param>
	private static void CreateBuffer(Span<Char> charSpan, SpanCreationInfo info)
	{
		Int32 offset = 0;
		ReadOnlySpan<IntPtr> pointers = new(info.Pointers, info.Lengths.Length);
		Span<Byte> bytes = MemoryMarshal.AsBytes(charSpan);
		for (Int32 i = 0; i < pointers.Length; i++)
		{
			Int32 textLength = info.Lengths[i].GetValueOrDefault();
			if (textLength < 1) continue;
			ReadOnlySpan<Byte> utf8Text = new(pointers[i].ToPointer(), textLength);
			utf8Text.CopyTo(bytes[offset..]);
			offset += utf8Text.Length + 1;
		}
	}
	/// <summary>
	/// Retrieves the sequence lengths array from <paramref name="nulls"/>.
	/// </summary>
	/// <param name="nulls">Collection of the indices UTF-8 null-character in buffer.</param>
	/// <returns>Sequence lengths array.</returns>
	private static Int32?[] GetLengths(ReadOnlySpan<Int32> nulls)
	{
		Int32?[] lengths = new Int32?[nulls.Length];
		Int32 offset = 0;
		for (Int32 i = 0; i < lengths.Length; i++)
		{
			Int32 length = nulls[i] - offset;
			lengths[i] = length;
			offset += length + 1;
		}
		return lengths;
	}
}