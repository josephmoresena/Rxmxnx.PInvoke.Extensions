#if !PACKAGE || !NETCOREAPP

namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// Rune compatibility utilities for internal use.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1764)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS907)]
#endif
internal static class RuneCompat
{
	private const UInt32 replacementChar = 0xFFFD;
	private const Char highSurrogateStart = '\ud800';
	private const Char lowSurrogateStart = '\udc00';
	private const Int32 highSurrogateRange = 0x3FF;
	/// <summary>
	/// Target framework for the current build.
	/// </summary>
	public static readonly String TargetFramework =
#if !NETCOREAPP
		".NETStandard 2.1";
#elif NETCOREAPP3_0
		".NETCoreApp 3.0.3";
#elif NETCOREAPP3_1
		".NETCoreApp 3.1.12";
#elif NET5_0
		".NETCoreApp 5.0.17";
#else
		RuntimeInformation.FrameworkDescription;
#endif

	/// <summary>
	/// Encodes <paramref name="rune"/> to a UTF-8 destination buffer.
	/// </summary>
	/// <param name="rune">A <see cref="UInt32"/> rune.</param>
	/// <param name="destination">The buffer to which to write this value as UTF-8.</param>
	/// <returns>The number of <see cref="byte"/>s written to <paramref name="destination"/>.</returns>
	public static Int32 EncodeToUtf8(UInt32 rune, Span<Byte> destination)
	{
		if (destination.Length < 1) return default;

		if (rune <= 0x7Fu)
		{
			destination[0] = (Byte)rune;
			return 1;
		}

		if (destination.Length < 2) return default;

		if (rune <= 0x7FFu)
		{
			destination[0] = (Byte)((rune + (0b110u << 11)) >> 6);
			destination[1] = (Byte)((rune & 0x3Fu) + 0x80u);
			return 2;
		}

		if (destination.Length < 3) return default;

		if (rune <= 0xFFFFu)
		{
			destination[0] = (Byte)((rune + (0b1110 << 16)) >> 12);
			destination[1] = (Byte)(((rune & (0x3Fu << 6)) >> 6) + 0x80u);
			destination[2] = (Byte)((rune & 0x3Fu) + 0x80u);
			return 3;
		}

		if (destination.Length < 4) return default;

		destination[0] = (Byte)((rune + (0b11110 << 21)) >> 18);
		destination[1] = (Byte)(((rune & (0x3Fu << 12)) >> 12) + 0x80u);
		destination[2] = (Byte)(((rune & (0x3Fu << 6)) >> 6) + 0x80u);
		destination[3] = (Byte)((rune & 0x3Fu) + 0x80u);
		return 4;
	}
	/// <summary>
	/// Decodes the <see cref="Rune"/> at the beginning of the provided UTF-8 source buffer.
	/// </summary>
	/// <param name="source">Source buffer.</param>
	/// <param name="result">Decoded rune.</param>
	/// <param name="bytesConsumed">Number of bytes consumed.</param>
	/// <returns>A <see cref="OperationStatus"/> value.</returns>
	public static OperationStatus DecodeFromUtf8(ReadOnlySpan<Byte> source, out UInt32 result, out Int32 bytesConsumed)
	{
		unchecked
		{
			Int32 index = 0;
			if ((UInt32)index >= (UInt32)source.Length) goto NeedsMoreData;

			UInt32 tempValue = source[index];
			if (tempValue > 0x7Fu) goto NotAscii;

			Finish:

			bytesConsumed = index + 1;
			result = tempValue;
			return OperationStatus.Done;

			NotAscii:
			if (!RuneCompat.IsInRangeInclusive(tempValue, 0xC2, 0xF4)) goto FirstByteInvalid;

			tempValue = (tempValue - 0xC2) << 6;

			index++;
			if ((UInt32)index >= (UInt32)source.Length) goto NeedsMoreData;
			Int32 thisByteSignExtended = (SByte)source[index];
			if (thisByteSignExtended >= -64) goto Invalid;

			tempValue += (UInt32)thisByteSignExtended;
			tempValue += 0x80; // remove the continuation byte marker
			tempValue += (0xC2 - 0xC0) << 6; // remove the leading byte marker

			if (tempValue < 0x0800)
			{
				Debug.Assert(RuneCompat.IsInRangeInclusive(tempValue, 0x0080, 0x07FF));
				goto Finish; // this is a valid 2-byte sequence
			}

			if (!RuneCompat.IsInRangeInclusive(tempValue, ((0xE0 - 0xC0) << 6) + (0xA0 - 0x80),
			                                   ((0xF4 - 0xC0) << 6) + (0x8F - 0x80)))
				// The first two bytes were not in the range [[E0 A0]..[F4 8F]].
				// This is an overlong 3-byte sequence or an out-of-range 4-byte sequence.
				goto Invalid;

			if (RuneCompat.IsInRangeInclusive(tempValue, ((0xED - 0xC0) << 6) + (0xA0 - 0x80),
			                                  ((0xED - 0xC0) << 6) + (0xBF - 0x80)))
				// This is a UTF-16 surrogate code point, which is invalid in UTF-8.
				goto Invalid;

			if (RuneCompat.IsInRangeInclusive(tempValue, ((0xF0 - 0xC0) << 6) + (0x80 - 0x80),
			                                  ((0xF0 - 0xC0) << 6) + (0x8F - 0x80)))
				// This is an overlong 4-byte sequence.
				goto Invalid;

			index++;
			if ((UInt32)index >= (UInt32)source.Length) goto NeedsMoreData;

			thisByteSignExtended = (SByte)source[index];
			if (thisByteSignExtended >= -64) goto Invalid; // this byte is not a UTF-8 continuation byte

			tempValue <<= 6;
			tempValue += (UInt32)thisByteSignExtended;
			tempValue += 0x80; // remove the continuation byte marker
			tempValue -= (0xE0 - 0xC0) << 12; // remove the leading byte marker

			if (tempValue <= 0xFFFF)
			{
				Debug.Assert(RuneCompat.IsInRangeInclusive(tempValue, 0x0800, 0xFFFF));
				goto Finish; // this is a valid 3-byte sequence
			}

			index++;
			if ((UInt32)index >= (UInt32)source.Length) goto NeedsMoreData;

			thisByteSignExtended = (SByte)source[index];
			if (thisByteSignExtended >= -64) goto Invalid; // this byte is not a UTF-8 continuation byte

			tempValue <<= 6;
			tempValue += (UInt32)thisByteSignExtended;
			tempValue += 0x80; // remove the continuation byte marker
			tempValue -= (0xF0 - 0xE0) << 18; // remove the leading byte marker

			goto Finish; // this is a valid 4-byte sequence

			FirstByteInvalid:

			index = 1; // Invalid subsequences are always at least length 1.

			Invalid:

			bytesConsumed = index;
			result = RuneCompat.replacementChar;
			return OperationStatus.InvalidData;

			NeedsMoreData:

			bytesConsumed = index;
			result = RuneCompat.replacementChar;
			return OperationStatus.NeedMoreData;
		}
	}
	/// <summary>
	/// Decodes the <see cref="Rune"/> at the beginning of the provided UTF-16 source buffer.
	/// </summary>
	/// <param name="source">Source buffer.</param>
	/// <param name="result">Decoded rune.</param>
	/// <param name="charsConsumed">Number of chars consumed.</param>
	/// <returns>A <see cref="OperationStatus"/> value.</returns>
	public static OperationStatus DecodeFromUtf16(ReadOnlySpan<Char> source, out UInt32 result, out Int32 charsConsumed)
	{
		if (!source.IsEmpty)
		{
			Char firstChar = source[0];
			if (RuneCompat.TryCreate(firstChar, out result))
			{
				charsConsumed = 1;
				return OperationStatus.Done;
			}

			if (1 < (UInt32)source.Length)
			{
				Char secondChar = source[1];
				if (RuneCompat.TryCreate(firstChar, secondChar, out result))
				{
					// Success! Formed a supplementary scalar value.
					charsConsumed = 2;
					return OperationStatus.Done;
				}
				goto InvalidData;
			}
			if (!Char.IsHighSurrogate(firstChar)) goto InvalidData;
		}

		charsConsumed = source.Length;
		result = RuneCompat.replacementChar;
		return OperationStatus.NeedMoreData;

		InvalidData:

		charsConsumed = 1; // maximal invalid subsequence for UTF-16 is always a single code unit in length
		result = RuneCompat.replacementChar;
		return OperationStatus.InvalidData;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsInRangeInclusive(UInt32 value, UInt32 lowerBound, UInt32 upperBound)
		=> value - lowerBound <= upperBound - lowerBound;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsSurrogateCodePoint(UInt32 value) => RuneCompat.IsInRangeInclusive(value, 0xD800U, 0xDFFFU);
	private static Boolean TryCreate(Char ch, out UInt32 result)
	{
		UInt32 extendedValue = ch;
		if (!RuneCompat.IsSurrogateCodePoint(extendedValue))
		{
			result = extendedValue;
			return true;
		}

		result = default;
		return false;
	}
	private static Boolean TryCreate(Char highSurrogate, Char lowSurrogate, out UInt32 result)
	{
		UInt32 highSurrogateOffset = (UInt32)highSurrogate - RuneCompat.highSurrogateStart;
		UInt32 lowSurrogateOffset = (UInt32)lowSurrogate - RuneCompat.lowSurrogateStart;

		if ((highSurrogateOffset | lowSurrogateOffset) <= RuneCompat.highSurrogateRange)
		{
			result = (highSurrogateOffset << 10) + ((UInt32)lowSurrogate - RuneCompat.lowSurrogateStart) +
				(0x40u << 10);
			return true;
		}

		result = default;
		return false;
	}
}
#endif