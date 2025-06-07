#if !NET6_0_OR_GREATER
using MemoryMarshalCompat = Rxmxnx.PInvoke.Internal.FrameworkCompat.MemoryMarshalCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class CStringSequence
{
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
	private static String CreateBuffer(ReadOnlySpan<CString?> values)
	{
		Int32 totalBytes = CStringSequence.GetTotalBytes(values);
		Int32 totalChars = totalBytes / sizeof(Char) + totalBytes % sizeof(Char);
#pragma warning disable CS8500
		fixed (CString?* valuesPtr = values)
		{
			CStringSpanState state = new() { Ptr = valuesPtr, Length = values.Length, };
			return String.Create(totalChars, state, CStringSequence.CopyText);
		}
#pragma warning restore CS8500
	}
	/// <summary>
	/// Creates buffer using <paramref name="span"/> and <paramref name="lengths"/>.
	/// </summary>
	/// <param name="span">Span of UTF-8 text pointers.</param>
	/// <param name="lengths">UTF-8 text lengths.</param>
	/// <returns>Created buffer.</returns>
	private static String CreateBuffer(ReadOnlySpan<ReadOnlyValPtr<Byte>> span, Int32?[] lengths)
	{
		fixed (void* ptrSpan = &MemoryMarshal.GetReference(span))
			return CStringSequence.CreateBuffer(ptrSpan, lengths);
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
	/// Copies the content of <see cref="CString"/> items from the provided <paramref name="state"/>
	/// to the given <paramref name="charSpan"/>.
	/// </summary>
	/// <param name="charSpan">
	/// The writable <see cref="Char"/> span that is the destination of the copy operation.
	/// </param>
	/// <param name="state">
	/// The collection of <see cref="CString"/> items whose contents are to be copied.
	/// </param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void CopyText(Span<Char> charSpan, CStringSpanState state)
	{
		Int32 position = 0;
		ref CString? refCStr = ref *state.Ptr;
		ReadOnlySpan<CString?> values = MemoryMarshal.CreateReadOnlySpan(ref refCStr, state.Length);
		Span<Byte> byteSpan = MemoryMarshal.AsBytes(charSpan);
		foreach (CString? value in values)
		{
			if (value is null || value.Length == 0) continue;
			ReadOnlySpan<Byte> valueSpan = value;
			valueSpan.CopyTo(byteSpan[position..]);
			position += valueSpan.Length;
			byteSpan[position] = default;
			position++;
		}
	}
	/// <summary>
	/// Copies the content of <see cref="CopyTextHelper"/> to the given <paramref name="charSpan"/>.
	/// </summary>
	/// <param name="charSpan">
	/// The writable <see cref="Char"/> span that is the destination of the copy operation.
	/// </param>
	/// <param name="helper">Helper value with pointer to UTF-8 source text.</param>
	private static void CopyText(Span<Char> charSpan, CopyTextHelper helper)
	{
		Span<Byte> byteSpan = MemoryMarshal.AsBytes(charSpan);
		ReadOnlySpan<Byte> sourceSpan = new(helper.Pointer, helper.Length);
		for (Int32 i = 0; i < sourceSpan.Length && i < byteSpan.Length; i++)
		{
			byteSpan[i] = sourceSpan[i];
			if (sourceSpan[i] == default && sourceSpan[i - 1] != default)
				helper.NullChars.Add(i);
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
		foreach (CString? value in seq)
		{
			if (value is null || value.Length <= 0) continue;
			ReadOnlySpan<Byte> bytes = value.AsSpan();
			bytes.CopyTo(destination.AsSpan()[offset..]);
			offset += bytes.Length;
		}
	}
	/// <summary>
	/// Creates a UTF-8 text sequence using the given <paramref name="helper"/>,
	/// with each UTF-8 text being initialized using the specified callback.
	/// </summary>
	/// <typeparam name="TState">The type of the element to pass to the action of the state.</typeparam>
	/// <param name="buffer">The UTF-16 buffer where the sequence is created.</param>
	/// <param name="helper">The state object used for creation.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void CreateCStringSequence<TState>(Span<Char> buffer, SequenceCreationHelper<TState> helper)
#if NET9_0_OR_GREATER
	where TState : allows ref struct
#endif
		=> CStringSequence.CreateCStringSequence(MemoryMarshal.AsBytes(buffer), helper);
	/// <summary>
	/// Creates a UTF-8 text sequence using the given <paramref name="helper"/>,
	/// with each UTF-8 text being initialized using the specified callback.
	/// </summary>
	/// <typeparam name="TState">The type of the element to pass to the action of the state.</typeparam>
	/// <param name="buffer">The byte buffer where the sequence is created.</param>
	/// <param name="helper">The state object used for creation.</param>
	private static void CreateCStringSequence<TState>(Span<Byte> buffer, SequenceCreationHelper<TState> helper)
#if NET9_0_OR_GREATER
	where TState : allows ref struct
#endif
	{
		Int32 offset = 0;
		for (Int32 i = 0; i < helper.Lengths.Length; i++)
		{
			Int32? length = helper.Lengths[i];
			if (length is not > 0)
				continue;
			helper.InvokeAction(buffer.Slice(offset, length.Value), i);
			offset += length.Value + 1;
		}
	}
	/// <summary>
	/// Creates a transitive <see cref="CString"/> instance from <paramref name="str"/>.
	/// </summary>
	/// <param name="str">The <see cref="String"/> instance to be converted.</param>
	/// <returns>
	/// A <see cref="CString"/> instance equivalent to <paramref name="str"/>, or <see langword="null"/> if
	/// <paramref name="str"/> is <see langword="null"/>.
	/// </returns>
	[return: NotNullIfNotNull(nameof(str))]
	private static CString? CreateTransitive(String? str)
#if NET6_0_OR_GREATER
		=> str is not null ? CString.Create(new CStringStringState(str)) : default;
#else
	{
		if (str is null) return default;
		CStringStringState state = new(str);
		return CString.Create(state, CStringStringState.GetSpan, false, state.Utf8Length);
	}
#endif
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
	/// Retrieves the length array for a given collection of UTF-8 texts.
	/// </summary>
	/// <param name="list">A collection of UTF-8 texts.</param>
	/// <returns>An array representing the length of each UTF-8 text in the collection.</returns>
	private static Int32?[] GetLengthArray(ReadOnlySpan<CString?> list)
	{
		Int32?[] result = new Int32?[list.Length];
		for (Int32 i = 0; i < result.Length; i++)
			result[i] = CStringSequence.GetLength(list[i]);
		return result;
	}
	/// <summary>
	/// Retrieves the length array for a given collection of Null-terminated UTF-8 text pointers.
	/// </summary>
	/// <param name="list">A collection of Null-terminated UTF-8 text pointers.</param>
	/// <returns>An array representing the length of each UTF-8 text in the collection.</returns>
	private static Int32?[] GetLengthArray(ReadOnlySpan<ReadOnlyValPtr<Byte>> list)
	{
		Int32?[] result = new Int32?[list.Length];
		for (Int32 i = 0; i < list.Length; i++)
		{
			if (!list[i].IsZero)
#if NET6_0_OR_GREATER
				result[i] = MemoryMarshal.CreateReadOnlySpanFromNullTerminated(list[i]).Length;
#else
				result[i] = MemoryMarshalCompat.CreateReadOnlySpanFromNullTerminated(list[i]).Length;
#endif
		}
		return result;
	}
	/// <summary>
	/// Creates cache for <paramref name="lengths"/>.
	/// </summary>
	/// <param name="lengths">The lengths of the UTF-8 text sequence.</param>
	/// <param name="totalNonEmpty">Output. Count of non-empty UTF-8 texts.</param>
	/// <returns>Instance cache.</returns>
	private static IList<CString?> CreateCache(IReadOnlyList<Int32?> lengths, out Int32 totalNonEmpty)
	{
		List<Int32> emptyIndices =
			CStringSequence.GetEmptyIndexList(lengths, out totalNonEmpty, out Int32 lastNonEmpty, out Int32 skipLast);

		// All elements are empty, the cache is an empty array.
		if (emptyIndices.Count == lengths.Count) return Array.Empty<CString>();

		// There is no empty elements or there are only at the end of the list
		if (emptyIndices.Count == 0 || (emptyIndices.Count - skipLast == 1 && lastNonEmpty + 1 == emptyIndices[0]))
			return lengths.Count switch
			{
				<= 32 => new CString?[totalNonEmpty],
				<= 256 => FixedCache.CreateFixedCache(totalNonEmpty, ImmutableHashSet<Int32>.Empty),
				_ => new DynamicCache(),
			};

		// Otherwise
		return lengths.Count switch
		{
			<= 256 => FixedCache.CreateFixedCache(totalNonEmpty, emptyIndices.SkipLast(skipLast).ToImmutableHashSet()),
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
		Int32 length = bytesLength / sizeof(Char) + bytesLength % sizeof(Char);
		return length;
	}
	/// <summary>
	/// Retrieves usable UTF-8 buffer from <paramref name="bufferSpan"/>.
	/// </summary>
	/// <param name="bufferSpan">A buffer of a UTF-8 sequence.</param>
	/// <param name="isParsable">Indicates whether resulting buffer is parsable.</param>
	/// <returns>A UTF-8 buffer.</returns>
	private static ReadOnlySpan<Byte> GetSourceBuffer(ReadOnlySpan<Byte> bufferSpan, ref Boolean isParsable)
	{
		Int32 bufferLength = bufferSpan.Length;
		if (bufferSpan.Length == 0)
		{
			isParsable = false;
			return bufferSpan;
		}
		while (bufferSpan.Length > 0 && bufferSpan[0] == default)
			bufferSpan = bufferSpan[1..]; // Any UTF-8 null-character at beginning is ignored.
		isParsable &= bufferSpan.Length == bufferLength;
		while (!isParsable && bufferSpan.Length > 3 && bufferSpan[^3] == default)
		{
			// If not parsable buffer, unnecessary UTF-8 null-characters at the end will be removed.
			if (bufferSpan[^2] != default || bufferSpan[^1] != default)
				break;
			bufferSpan = bufferSpan[..^1];
		}
		isParsable = bufferSpan.Length == bufferLength && bufferSpan[^1] == default;
		return bufferSpan;
	}
	/// <summary>
	/// Creates a new <see cref="CStringSequence"/> instance from <paramref name="buffer"/>.
	/// </summary>
	/// <param name="buffer">A buffer of a UTF-8 sequence.</param>
	/// <returns>A new <see cref="CStringSequence"/> instance.</returns>
	private static CStringSequence CreateFrom(ReadOnlySpan<Byte> buffer)
	{
		if (buffer.Length == 0) return CStringSequence.Empty;
		Int32 totalBytes = buffer.Length + (buffer[^1] == default ? 0 : 1);
		Int32 totalChars = totalBytes / sizeof(Char);
		ReadOnlySpan<Int32> nulls;
		String sequenceBuffer;
		fixed (Byte* ptr = &MemoryMarshal.GetReference(buffer))
		{
			CopyTextHelper state = new() { Pointer = ptr, Length = buffer.Length, NullChars = [], };
			sequenceBuffer = String.Create(totalChars, state, CStringSequence.CopyText);
#if NET5_0_OR_GREATER
			nulls = CollectionsMarshal.AsSpan(state.NullChars);
#else
			Span<Int32> nullsTmp = stackalloc Int32[state.NullChars.Count];
			for (Int32 i = 0; i < state.NullChars.Count; i++)
				nullsTmp[i] = state.NullChars[i];
#pragma warning disable CS9080
			nulls = nullsTmp;
#pragma warning restore CS9080
#endif
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
		for (Int32 i = 0; i < span.Length; i++)
		{
			if (span[i] == default && span[i - 1] != default)
				nulls.Add(i);
		}
		if (span[^1] != default) nulls.Add(span.Length);
#if NET5_0_OR_GREATER
		Int32?[] lengths = CStringSequence.GetLengths(CollectionsMarshal.AsSpan(nulls));
#else
		Span<Int32> nullsTmp = stackalloc Int32[nulls.Count];
		for (Int32 i = 0; i < nulls.Count; i++)
			nullsTmp[i] = nulls[i];
		Int32?[] lengths = CStringSequence.GetLengths(nullsTmp);
#endif
		return new(buffer, lengths);
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
	/// <summary>
	/// Retrieves the number of required bytes in the sequence buffer.
	/// </summary>
	/// <param name="values">The collection of text.</param>
	/// <returns>Number of required bytes</returns>
	private static Int32 GetTotalBytes(ReadOnlySpan<CString?> values)
	{
		Int32 totalBytes = 0;
		foreach (CString? value in values)
		{
			if (value is not null && value.Length > 0)
				totalBytes += value.Length + 1;
		}
		return totalBytes;
	}
}