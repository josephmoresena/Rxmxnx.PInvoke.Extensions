namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Instance cache.
	/// </summary>
	private readonly IList<CString?> _cache;
	/// <summary>
	/// Collection of lengths for each text in the buffer. Used for interpreting the buffer content.
	/// </summary>
	private readonly Int32?[] _lengths;
	/// <summary>
	/// Non-empty items count.
	/// </summary>
	private readonly Int32 _nonEmptyCount;
	/// <summary>
	/// Internal buffer representing the combined null-terminated UTF-8 texts.
	/// </summary>
	private readonly String _value;

	/// <summary>
	/// Retrieves the internal buffer as a <see cref="ReadOnlySpan{Char}"/> instance and creates a
	/// <see cref="CString"/> array representing the sequence of texts.
	/// </summary>
	/// <param name="output">
	/// Output <see cref="CString"/> array that represents the sequence of texts.
	/// </param>
	/// <returns>A <see cref="ReadOnlySpan{Char}"/> representing the internal buffer.</returns>
	private unsafe ReadOnlySpan<Char> AsUnsafeSpan(out CString[] output)
	{
		ReadOnlySpan<Char> result = this._value;
		ref Char firstCharRef = ref MemoryMarshal.GetReference(result);
		IntPtr ptr = new(Unsafe.AsPointer(ref firstCharRef));
		output = this.GetValues(ptr).ToArray();
		return this._value;
	}
	/// <summary>
	/// Retrieves a sequence of <see cref="CString"/> based on the buffer and lengths of the texts.
	/// </summary>
	/// <param name="ptr">Pointer to the start of the buffer.</param>
	/// <returns>An <see cref="IEnumerable{CString}"/> representing the sequence of texts.</returns>
	private IEnumerable<CString> GetValues(IntPtr ptr)
	{
		Int32 offset = 0;
		// ReSharper disable once ForCanBeConvertedToForeach
		for (Int32 index = 0; index < this._lengths.Length; index++)
		{
			Int32? length = this._lengths[index];
			switch (length)
			{
				case > 0:
					yield return CString.CreateUnsafe(ptr + offset, length.Value + 1);
					offset += length.Value + 1;
					break;
				default:
				{
					if (length.HasValue)
						yield return CString.Empty;
					else
						yield return CString.Zero;
					break;
				}
			}
		}
	}
	/// <summary>
	/// Creates a <see cref="FixedCStringSequence"/> instance from the current instance and a pointer to the buffer.
	/// </summary>
	/// <param name="ptr">Pointer to the UTF-8 sequence buffer.</param>
	/// <returns>A <see cref="FixedCStringSequence"/> instance.</returns>
	private unsafe FixedCStringSequence GetFixedSequence(Char* ptr)
	{
		_ = this.AsUnsafeSpan(out CString[] output);
		return new(output, CString.CreateUnsafe(new(ptr), this._value.Length * sizeof(Char), true));
	}
	/// <summary>
	/// Calculates the offset and length for the indicated sub-range.
	/// </summary>
	/// <param name="index">Starting index of the sub-range.</param>
	/// <param name="count">Number of items in the sub-range.</param>
	/// <param name="binaryOffset">Output: Binary offset for the given sub-range.</param>
	/// <param name="binaryLength">Output: Binary length for the given sub-range.</param>
	private void CalculateSubRange(Int32 index, Int32 count, out Int32 binaryOffset, out Int32 binaryLength)
	{
		binaryOffset = 0;
		binaryLength = -1;
		for (Int32 i = 0; i < index + count; i++)
		{
			Int32 spanLength = CStringSequence.GetSpanLength(this._lengths[i]);
			if (i < index)
				binaryOffset += spanLength;
			else
				binaryLength += spanLength;
		}
	}
	/// <summary>
	/// Creates a byte array containing each non-empty UTF-8 text bytes in the current instance.
	/// </summary>
	/// <param name="arrayLength">Array length.</param>
	/// <returns>A byte array containing each non-empty UTF-8 text bytes in the current instance.</returns>
	private Byte[] CreateTextArray(Int32 arrayLength)
	{
		Byte[] result = CString.CreateByteArray(arrayLength);
		Utf8View view = new(this, false);
		Span<Byte> destination = result.AsSpan();
		foreach (ReadOnlySpan<Byte> value in view)
		{
			value.CopyTo(destination);
			destination = destination[value.Length..];
		}
		destination.Clear();
		return result;
	}
	/// <summary>
	/// Resolves the zero-based index of the segment that contains the specified absolute <paramref name="offset"/>
	/// within the current instance.
	/// </summary>
	/// <param name="offset">Absolute offset within the concatenated sequence of items.</param>
	/// <returns>
	/// The zero-based index of the segment that contains <paramref name="offset"/>, or -1 if the offset falls outside
	/// all segments.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private Int32 ResolveIndexFromOffset(Int32 offset)
	{
		ReadOnlySpan<Int32?> lengths = this._lengths.AsSpan();
		for (Int32 i = 0; i < lengths.Length; i++)
		{
			Int32 length = lengths[i].GetValueOrDefault();
			if (length == 0) continue;
			if (offset < 0) break;
			if (offset < length) return i;
			offset -= length + 1;
		}
		return -1;
	}
	/// <summary>
	/// Searches for the first segment whose stored length matches exactly <paramref name="length"/>.
	/// </summary>
	/// <param name="length">Length value to match. May be <see langword="null"/>.</param>
	/// <returns>
	/// The zero-based index of the first segment whose length equals <paramref name="length"/>, or -1 if no match is
	/// found.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private Int32 GetIndexOfExactLength(Int32? length)
	{
#if !NET10_0_OR_GREATER
		ReadOnlySpan<Int32?> lengths = this._lengths.AsSpan();
		for (Int32 i = 0; i < lengths.Length; i++)
		{
			if (lengths[i] != length) continue;
			return i;
		}
		return -1;
#else
		return this._lengths.AsSpan().IndexOf(length);
#endif
	}
}