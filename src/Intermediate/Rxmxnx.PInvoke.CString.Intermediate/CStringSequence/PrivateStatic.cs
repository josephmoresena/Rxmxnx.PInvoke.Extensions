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
		Int32 bufferLength = CStringSequence.GetBufferLength(lengths.AsSpan());
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
			bytes[offset + utf8Text.Length] = default; // Null-termination.
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
#pragma warning disable CS8619
		ReadOnlySpan<CString?> values = MemoryMarshal.CreateReadOnlySpan(ref refCStr, state.Length);
#pragma warning restore CS8619
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
		List<Int32> nulls = helper.NullChars;
		Int32 textLength = Math.Min(sourceSpan.Length, byteSpan.Length);
		Boolean addNullAtEnd = sourceSpan[textLength - 1] != default;
		Int32 offset = 0;
		while (!sourceSpan.IsEmpty && !byteSpan.IsEmpty)
		{
			Int32 distToNull = sourceSpan.IndexOf((Byte)0);
			if (distToNull < 0) break;
			if (distToNull > 0)
			{
				offset += distToNull;
				nulls.Add(offset);

				Int32 newOffset = distToNull + 1;
				sourceSpan[..newOffset].CopyTo(byteSpan);
				sourceSpan = sourceSpan[newOffset..];
				byteSpan = byteSpan[newOffset..];
				offset++;
				continue;
			}
			Int32 zeros = CStringSequence.GetZeros(sourceSpan);
			offset += distToNull + zeros;
			byteSpan[..zeros].Clear();
			sourceSpan = sourceSpan[zeros..];
			byteSpan = byteSpan[zeros..];
		}
		sourceSpan.CopyTo(byteSpan);
		if (addNullAtEnd)
			helper.NullChars.Add(textLength);
		if (!byteSpan.IsEmpty)
			byteSpan[sourceSpan.Length..].Clear();
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
#if NET7_0_OR_GREATER
		=> str is not null ? CString.Create(new CStringStringState(str)) : default;
#else
	{
		if (str is null) return default;
		CStringStringState state = new(str);
		return CString.Create(state, CStringStringState.GetSpan, state.Utf8Length);
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
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
	private static IList<CString?> CreateCache(ReadOnlySpan<Int32?> lengths, out Int32 totalNonEmpty)
	{
		List<Int32> emptyIndices =
			CStringSequence.GetEmptyIndexList(lengths, out totalNonEmpty, out Int32 lastNonEmpty, out Int32 skipLast);

		// All elements are empty, the cache is an empty array.
		if (emptyIndices.Count == lengths.Length) return Array.Empty<CString>();

		// There is no empty elements or there are only at the end of the list
		if (emptyIndices.Count == 0 || (emptyIndices.Count - skipLast == 1 && lastNonEmpty + 1 == emptyIndices[0]))
			return lengths.Length switch
			{
				<= 32 => new CString?[totalNonEmpty],
				<= 256 => FixedCache.CreateFixedCache(totalNonEmpty),
				_ => DynamicCache.CreateDynamicCache(totalNonEmpty),
			};

		// Otherwise
		return lengths.Length switch
		{
			<= 256 => FixedCache.CreateFixedCache(totalNonEmpty, emptyIndices.SkipLast(skipLast).ToImmutableHashSet()),
			_ => DynamicCache.CreateDynamicCache(totalNonEmpty),
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
	private static List<Int32> GetEmptyIndexList(ReadOnlySpan<Int32?> lengths, out Int32 totalNonEmpty,
		out Int32 lastNonEmpty, out Int32 skipLast)
	{
		List<Int32> result = new(lengths.Length);
		lastNonEmpty = -1;
		// Fills gaps list and determines latest non-empty element.
		for (Int32 i = 0; i < lengths.Length; i++)
		{
			if (lengths[i].GetValueOrDefault() != 0)
				lastNonEmpty = i;
			else
				result.Add(i);
		}
		// Determines total non-empty elements.
		totalNonEmpty = lengths.Length - result.Count;
		// Determines how many items can be skipped to the end of the list.
		skipLast = 0;
		for (Int32 i = result.Count - 1; i > 0; i--)
		{
			if (result[i] - 1 != result[i - 1] || result[^1] != lengths.Length - 1) break;
			skipLast++;
		}
		return result;
	}
	/// <summary>
	/// Calculates buffer's length.
	/// </summary>
	/// <param name="lengths">The lengths of the UTF-8 text sequence to create.</param>
	/// <returns><see cref="String"/> buffer length.</returns>
	private static Int32 GetBufferLength(ReadOnlySpan<Int32?> lengths)
	{
		Int32 bytesLength = CStringSequence.GetTotalBytes(lengths);
		Int32 length = bytesLength / sizeof(Char) + bytesLength % sizeof(Char);
		return length;
	}
	/// <summary>
	/// Creates a new <see cref="CStringSequence"/> instance from <paramref name="buffer"/>.
	/// </summary>
	/// <param name="buffer">A buffer of a UTF-8 sequence.</param>
	/// <returns>A new <see cref="CStringSequence"/> instance.</returns>
	private static CStringSequence CreateFrom(ReadOnlySpan<Byte> buffer)
	{
		CStringSequence.Trim(ref buffer);

		if (buffer.Length == 0) return CStringSequence.Empty;
		Int32 totalBytes = buffer.Length + (buffer[^1] == default ? 0 : 1);
		Int32 totalChars = totalBytes / sizeof(Char) + totalBytes % sizeof(Char);
		String sequenceBuffer;
		Int32?[] lengths;
		fixed (Byte* ptr = &MemoryMarshal.GetReference(buffer))
		{
			CopyTextHelper state = new() { Pointer = ptr, Length = buffer.Length, NullChars = [], };
			sequenceBuffer = String.Create(totalChars, state, CStringSequence.CopyText);
			lengths = CStringSequence.GetLengths(state.NullChars);
		}
		return new(sequenceBuffer, lengths);
	}
	/// <summary>
	/// Creates a new <see cref="CStringSequence"/> instance from <paramref name="buffer"/>.
	/// </summary>
	/// <param name="buffer">A buffer of a UTF-8 sequence.</param>
	/// <returns>A new <see cref="CStringSequence"/> instance.</returns>
	private static CStringSequence CreateFrom(String buffer)
	{
		ReadOnlySpan<Byte> source = MemoryMarshal.AsBytes(buffer.AsSpan());
		Int32?[] lengths = CStringSequence.GetLengths(CStringSequence.GetNulls(source));
		return new(buffer, lengths);
	}
	/// <summary>
	/// Retrieves the number of consecutive UTF-8 null-characters in <paramref name="buffer"/>.
	/// </summary>
	/// <param name="buffer">A buffer of a UTF-8 sequence.</param>
	/// <returns>The number of consecutive UTF-8 null-characters in <paramref name="buffer"/>.</returns>
	private static Int32 GetZeros(ReadOnlySpan<Byte> buffer)
	{
		Int32 zeros = 0;
		while (zeros < buffer.Length && buffer[zeros] == 0)
			zeros++;
		return zeros;
	}
	/// <summary>
	/// Trims leading and trailing UTF-8 null-character from the given <see cref="ReadOnlySpan{Byte}"/>.
	/// </summary>
	/// <param name="bufferSpan">
	/// A reference to a <see cref="ReadOnlySpan{Byte}"/> that will be updated to exclude leading and trailing
	/// UTF-8 null-characters.
	/// </param>
	private static void Trim(ref ReadOnlySpan<Byte> bufferSpan)
	{
		Int32 zeros = CStringSequence.GetZeros(bufferSpan);
		Int32 length = bufferSpan.Length - zeros;
		while (length > 0 && bufferSpan[zeros + length - 1] == default)
			length--;
		bufferSpan = bufferSpan.Slice(zeros, length);
	}
	/// <summary>
	/// Retrieves the zero-based indices of isolated UTF-8 null-character within the given span.
	/// </summary>
	/// <param name="span">A <see cref="ReadOnlySpan{Byte}"/> to scan for null bytes.</param>
	/// <returns>
	/// A <see cref="List{Int32}"/> containing the zero-based positions of null bytes that are not part of a
	/// consecutive sequence of zeros.
	/// </returns>
	private static List<Int32> GetNulls(ReadOnlySpan<Byte> span)
	{
		List<Int32> nulls = [];
		Int32 offset = 0;
		while (!span.IsEmpty)
		{
			Int32 distToNull = span.IndexOf((Byte)0);
			if (distToNull < 0) break;
			offset += distToNull;
			if (distToNull > 0)
			{
				nulls.Add(offset);
				span = span[(distToNull + 1)..];
				offset++;
				continue;
			}
			Int32 zeros = CStringSequence.GetZeros(span);
			offset += zeros;
			span = span[zeros..];
		}
		return nulls;
	}
	/// <summary>
	/// Retrieves the sequence lengths array from <paramref name="nulls"/>.
	/// </summary>
	/// <param name="nulls">Collection of the indices UTF-8 null-character in buffer.</param>
	/// <returns>Sequence lengths array.</returns>
	private static Int32?[] GetLengths(List<Int32> nulls)
	{
#if NET5_0_OR_GREATER
		return CStringSequence.GetLengths(CollectionsMarshal.AsSpan(nulls));
#else
		Span<Int32> nullsTmp = stackalloc Int32[nulls.Count];
		for (Int32 i = 0; i < nulls.Count; i++)
			nullsTmp[i] = nulls[i];
		return CStringSequence.GetLengths(nullsTmp);
#endif
	}
	/// <summary>
	/// Retrieves the sequence lengths array from <paramref name="nulls"/>.
	/// </summary>
	/// <param name="nulls">Collection of the indices UTF-8 null-character in buffer.</param>
	/// <returns>Sequence lengths array.</returns>
	private static Int32?[] GetLengths(ReadOnlySpan<Int32> nulls)
	{
		if (nulls.IsEmpty) return [];

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
	/// <returns>Number of required bytes.</returns>
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
	/// <summary>
	/// Retrieves the number of required bytes in the sequence buffer.
	/// </summary>
	/// <param name="lengths">The lengths of the UTF-8 text sequence.</param>
	/// <returns>Number of required bytes.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 GetTotalBytes(ReadOnlySpan<Int32?> lengths)
	{
		Int32 result = 0;
		foreach (Int32? length in lengths)
			result += CStringSequence.GetSpanLength(length);
		return result;
	}
}