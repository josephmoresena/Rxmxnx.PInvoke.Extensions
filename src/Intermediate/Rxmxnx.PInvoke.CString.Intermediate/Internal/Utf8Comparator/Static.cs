#if !NETCOREAPP
using Rune = System.UInt32;
#endif

namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// The abstract Utf8Comparator class provides a means for efficiently and customizable
/// comparing two UTF-8 texts.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal abstract unsafe partial class Utf8Comparator
{
	/// <summary>
	/// Retrieves the string representation of <paramref name="source"/>.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="byte"/> elements representing a UTF-8 encoded text.</param>
	/// <returns>A string that represents the <paramref name="source"/> text.</returns>
#if !PACKAGE && (!NETCOREAPP || NET7_0_OR_GREATER)
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static String GetStringFromUtf8(ReadOnlySpan<Byte> source) => Encoding.UTF8.GetString(source);
	/// <summary>
	/// Decodes all the bytes in <paramref name="source"/> into a <paramref name="destination"/>.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="byte"/> elements representing a UTF-8 encoded text.</param>
	/// <param name="destination">The character span receiving the decoded bytes.</param>
#if !PACKAGE && NETCOREAPP && !NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static void CopyCharsFromUtf8(ReadOnlySpan<Byte> source, Span<Char> destination)
		=> Encoding.UTF8.GetChars(source, destination);
	/// <summary>
	/// Calculates the number of characters produced by decoding the <paramref name="source"/>.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="byte"/> elements representing a UTF-8 encoded text.</param>
	/// <returns>The number of characters produced by decoding the UTF-8 encoded text.</returns>
#if !PACKAGE && NETCOREAPP && !NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static Int32 GetCharCountFromUtf8(ReadOnlySpan<Byte> source) => Encoding.UTF8.GetCharCount(source);
	/// <summary>
	/// Decodes the <see cref="Rune"/> at the beginning of the provided UTF-8 encoded source buffer.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="byte"/> elements representing a UTF-8 encoded text.</param>
	/// <returns>The decoded <see cref="Rune"/>, if any; otherwise, <see langword="null"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static DecodedRune? DecodeRuneFromUtf8(ref ReadOnlySpan<Byte> source)
	{
		DecodedRune? result = DecodedRune.Decode(source);
		if (result.HasValue)
			source = source[result.Value.CharsConsumed..];
		return result;
	}
	/// <summary>
	/// Performs an ordinal comparison between two UTF-16 character spans.
	/// </summary>
	/// <param name="spanA">The first UTF-16 span to compare.</param>
	/// <param name="spanB">The second UTF-16 span to compare.</param>
	/// <returns>
	/// A signed 32-bit integer that indicates the lexical relationship between <paramref name="spanA"/> and
	/// <paramref name="spanB"/>.
	/// </returns>
	/// <remarks>
	/// Comparison is performed using the numeric values of UTF-16 code units and is independent of culture or
	/// normalization.
	/// </remarks>
#if !PACKAGE && NETCOREAPP && !NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected static Int32 OrdinalCompare(ReadOnlySpan<Char> spanA, ReadOnlySpan<Char> spanB)
	{
		Int32 lengthA = spanA.Length;
		Int32 lengthB = spanB.Length;

		if (Utf8Comparator.OrdinalCompareFirst(ref spanA, ref spanB, out Int32 result))
			return result;

		Int32 minLength = Environment.Is64BitProcess ? 12 : 10;
		while (Math.Min(spanA.Length, spanB.Length) >= minLength)
		{
			switch (Environment.Is64BitProcess)
			{
				case true when Utf8Comparator.OrdinalCompare64Bit(ref spanA, ref spanB, out result):
				case false when Utf8Comparator.OrdinalCompare32Bit(ref spanA, ref spanB, out result):
					return result;
			}
		}
		while (Math.Min(spanA.Length, spanB.Length) > 0)
		{
			CharSpanChunk chunkA = Utf8Comparator.Read<CharSpanChunk>(ref spanA);
			CharSpanChunk chunkB = Utf8Comparator.Read<CharSpanChunk>(ref spanB);
			result = CharSpanChunk.Compare(chunkA, chunkB);
			if (result != 0) return result;
		}
		return lengthA - lengthB;
	}

	/// <summary>
	/// Compares the first two UTF-16 characters of each span.
	/// </summary>
	/// <param name="spanA">Reference to the first span.</param>
	/// <param name="spanB">Reference to the second span.</param>
	/// <param name="result">Receives the comparison result.</param>
	/// <returns> <see langword="true"/> if a difference was detected; otherwise <see langword="false"/>.</returns>
	/// <remarks>Advances both spans by the number of characters read.</remarks>
#if !PACKAGE && NETCOREAPP && !NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean OrdinalCompareFirst(ref ReadOnlySpan<Char> spanA, ref ReadOnlySpan<Char> spanB,
		out Int32 result)
	{
		Char charA = Utf8Comparator.Read<Char>(ref spanA);
		Char charB = Utf8Comparator.Read<Char>(ref spanB);
		result = charA - charB;
		if (result != 0) return true;

		charA = Utf8Comparator.Read<Char>(ref spanA);
		charB = Utf8Comparator.Read<Char>(ref spanB);
		result = charA - charB;
		return result != 0;
	}
	/// <summary>
	/// Reads a value of type <typeparamref name="T"/> from the given location.
	/// </summary>
	/// <typeparam name="T">The type of the value to read.</typeparam>
	/// <param name="source">The Unicode char span source.</param>
	/// <returns>A value of type <typeparamref name="T"/>  read from the given span.</returns>
#if !PACKAGE && NETCOREAPP && !NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static T Read<T>(ref ReadOnlySpan<Char> source) where T : unmanaged
	{
		if (source.IsEmpty) return default;

		ref Char refChar = ref MemoryMarshal.GetReference(source);
		if (sizeof(T) == sizeof(Char))
		{
			source = source.Length > 0 ? source[1..] : default;
			return Unsafe.As<Char, T>(ref refChar);
		}

		Int32 sizeOnBytes = Math.Min(sizeof(T), source.Length * sizeof(Char));
		if (sizeof(T) == sizeOnBytes)
		{
			Int32 sizeInChars = sizeof(T) / sizeof(Char);
			source = source.Length > 0 ? source[sizeInChars..] : default;
			return Unsafe.As<Char, T>(ref refChar);
		}

		T result = default;
		ref Byte refByte = ref Unsafe.As<Char, Byte>(ref refChar);
		ReadOnlySpan<Byte> bytes = MemoryMarshal.CreateReadOnlySpan(ref refByte, sizeOnBytes);
		Span<Byte> resultBytes = MemoryMarshal.CreateSpan(ref Unsafe.As<T, Byte>(ref result), bytes.Length);
		bytes.CopyTo(resultBytes);
		source = default;
		return result;
	}
	/// <summary>
	/// Compares the first 24 UTF-16 characters of each span.
	/// </summary>
	/// <param name="spanA">Reference to the first span.</param>
	/// <param name="spanB">Reference to the second span.</param>
	/// <param name="result">Receives the comparison result.</param>
	/// <returns> <see langword="true"/> if a difference was detected; otherwise <see langword="false"/>.</returns>
	/// <remarks>Advances both spans by the number of characters read.</remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private static Boolean OrdinalCompare64Bit(ref ReadOnlySpan<Char> spanA, ref ReadOnlySpan<Char> spanB,
		out Int32 result)
	{
		DoubleCharSpanChunk doubleA = Utf8Comparator.Read<DoubleCharSpanChunk>(ref spanA);
		DoubleCharSpanChunk doubleB = Utf8Comparator.Read<DoubleCharSpanChunk>(ref spanB);
		result = DoubleCharSpanChunk.Compare(doubleA, doubleB);
		if (result != 0) return true;
		doubleA = Utf8Comparator.Read<DoubleCharSpanChunk>(ref spanA);
		doubleB = Utf8Comparator.Read<DoubleCharSpanChunk>(ref spanB);
		result = DoubleCharSpanChunk.Compare(doubleA, doubleB);
		if (result != 0) return true;
		doubleA = Utf8Comparator.Read<DoubleCharSpanChunk>(ref spanA);
		doubleB = Utf8Comparator.Read<DoubleCharSpanChunk>(ref spanB);
		result = DoubleCharSpanChunk.Compare(doubleA, doubleB);
		return result != 0;
	}
	/// <summary>
	/// Compares the first 20 UTF-16 characters of each span.
	/// </summary>
	/// <param name="spanA">Reference to the first span.</param>
	/// <param name="spanB">Reference to the second span.</param>
	/// <param name="result">Receives the comparison result.</param>
	/// <returns> <see langword="true"/> if a difference was detected; otherwise <see langword="false"/>.</returns>
	/// <remarks>Advances both spans by the number of characters read.</remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private static Boolean OrdinalCompare32Bit(ref ReadOnlySpan<Char> spanA, ref ReadOnlySpan<Char> spanB,
		out Int32 result)
	{
		CharSpanChunk chunkA = Utf8Comparator.Read<CharSpanChunk>(ref spanA);
		CharSpanChunk chunkB = Utf8Comparator.Read<CharSpanChunk>(ref spanB);

		result = CharSpanChunk.Compare(chunkA, chunkB);
		if (result != 0) return true;
		chunkA = Utf8Comparator.Read<CharSpanChunk>(ref spanA);
		chunkB = Utf8Comparator.Read<CharSpanChunk>(ref spanB);
		result = CharSpanChunk.Compare(chunkA, chunkB);
		if (result != 0) return true;
		chunkA = Utf8Comparator.Read<CharSpanChunk>(ref spanA);
		chunkB = Utf8Comparator.Read<CharSpanChunk>(ref spanB);
		result = CharSpanChunk.Compare(chunkA, chunkB);
		if (result != 0) return true;
		chunkA = Utf8Comparator.Read<CharSpanChunk>(ref spanA);
		chunkB = Utf8Comparator.Read<CharSpanChunk>(ref spanB);
		result = CharSpanChunk.Compare(chunkA, chunkB);
		if (result != 0) return true;
		chunkA = Utf8Comparator.Read<CharSpanChunk>(ref spanA);
		chunkB = Utf8Comparator.Read<CharSpanChunk>(ref spanB);
		result = CharSpanChunk.Compare(chunkA, chunkB);
		return result != 0;
	}
}