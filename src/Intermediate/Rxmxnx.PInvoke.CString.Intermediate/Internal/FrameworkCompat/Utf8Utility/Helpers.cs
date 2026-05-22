// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/*
MIT License

Copyright (c) .NET Foundation and contributors

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

// Adopted and adapted by Joseph Moreno in 2026 based on code from Microsoft.Blc.Memory 10.0.8
// (System.text.Unicode.Utf8Utility)

#if !NETCOREAPP
namespace System.Text.Unicode;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1199)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS907)]
#endif
internal static partial class Utf8Utility
{
	/// <summary>
	/// Given a machine-endian DWORD which four bytes of UTF-8 data, interprets the
	/// first three bytes as a three-byte UTF-8 subsequence and returns the UTF-16 representation.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 ExtractCharFromFirstThreeByteSequence(UInt32 value)
	{
		if (BitConverter.IsLittleEndian)
			return ((value & 0x003F0_000u) >> 16) | ((value & 0x0000_3F00u) >> 2) | ((value & 0x0000_000Fu) << 12);
		return ((value & 0x0F00_0000u) >> 12) | ((value & 0x003F_0000u) >> 10) | ((value & 0x0000_3F00u) >> 8);
	}

	/// <summary>
	/// Given a machine-endian DWORD which four bytes of UTF-8 data, interprets the
	/// first two bytes as a two-byte UTF-8 subsequence and returns the UTF-16 representation.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 ExtractCharFromFirstTwoByteSequence(UInt32 value)
	{
		if (!BitConverter.IsLittleEndian)
			return (Char)(((value & 0x1F00_0000u) >> 18) | ((value & 0x003F_0000u) >> 16));
		UInt32 leadingByte = (UInt32)(Byte)value << 6;
		return (Byte)(value >> 8) + leadingByte - (0xC0u << 6) - 0x80u;
	}

	/// <summary>
	/// Given a machine-endian DWORD which represents four bytes of UTF-8 data, interprets the input as a
	/// four-byte UTF-8 sequence and returns the machine-endian DWORD of the UTF-16 representation.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 ExtractCharsFromFourByteSequence(UInt32 value)
	{
		UInt32 retVal;
		if (BitConverter.IsLittleEndian)
		{
			retVal = (UInt32)(Byte)value << 8;
			retVal |= (value & 0x0000_3F00u) >> 6;
			retVal |= (value & 0x0030_0000u) >> 20;
			retVal |= (value & 0x3F00_0000u) >> 8;
			retVal |= (value & 0x000F_0000u) << 6;
			retVal -= 0x0000_0040u;
			retVal -= 0x0000_2000u;
			retVal += 0x0000_0800u;
			retVal += 0xDC00_0000u;
			return retVal;
		}

		retVal = value & 0xFF00_0000u;
		retVal |= (value & 0x003F_0000u) << 2;
		retVal |= (value & 0x0000_3000u) << 4;
		retVal |= (value & 0x0000_0F00u) >> 2;
		retVal |= value & 0x0000_003Fu;
		retVal -= 0x2000_0000u;
		retVal -= 0x0040_0000u;
		retVal += 0x0000_DC00u;
		retVal += 0x0800_0000u;
		return retVal;
	}

	/// <summary>
	/// Given a 32-bit integer that represents a valid packed UTF-16 surrogate pair, all in machine-endian order,
	/// returns the packed 4-byte UTF-8 representation of this scalar value, also in machine-endian order.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 ExtractFourUtf8BytesFromSurrogatePair(UInt32 value)
	{
		UInt32 tempA, tempB, tempC, tempD;
		if (BitConverter.IsLittleEndian)
		{
			value += 0x0000_0040u;
			tempA = BinaryPrimitives.ReverseEndianness(value & 0x003F_0700u);
			tempA = BitOperations.RotateLeft(tempA, 16);

			tempB = (value & 0x00FCu) << 6;
			tempC = (value >> 6) & 0x000F_0000u;
			tempC |= tempB;

			tempD = (value & 0x03u) << 20;
			tempD |= 0x8080_80F0u;

			return tempD | tempA | tempC;
		}

		value -= 0xD800_DC00u;
		value += 0x0040_0000u;

		tempA = value & 0x0700_0000u;
		tempB = (value >> 2) & 0x003F_0000u;
		tempB |= tempA;

		tempC = (value << 2) & 0x0000_0F00u;
		tempD = (value >> 4) & 0x0000_3000u;
		tempD |= tempC;

		return ((value & 0x3Fu) + 0xF080_8080u) | tempB | tempD;
	}

	/// <summary>
	/// Given a machine-endian DWORD which represents two adjacent UTF-8 two-byte sequences,
	/// returns the machine-endian DWORD representation of that same data as two adjacent
	/// UTF-16 byte sequences.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 ExtractTwoCharsPackedFromTwoAdjacentTwoByteSequences(UInt32 value)
	{
		if (BitConverter.IsLittleEndian)
			return ((value & 0x3F003F00u) >> 8) | ((value & 0x001F001Fu) << 6);
		return ((value & 0x1F001F00u) >> 2) | (value & 0x003F003Fu);
	}

	/// <summary>
	/// Given a machine-endian DWORD which represents two adjacent UTF-16 sequences,
	/// returns the machine-endian DWORD representation of that same data as two
	/// adjacent UTF-8 two-byte sequences.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 ExtractTwoUtf8TwoByteSequencesFromTwoPackedUtf16Chars(UInt32 value)
	{
		if (BitConverter.IsLittleEndian)
			return ((value >> 6) & 0x001F_001Fu) + ((value << 8) & 0x3F00_3F00u) + 0x80C0_80C0u;
		return ((value << 2) & 0x1F00_1F00u) + (value & 0x003F_003Fu) + 0xC080_C080u;
	}

	/// <summary>
	/// Given a machine-endian DWORD which represents two adjacent UTF-16 sequences,
	/// returns the machine-endian DWORD representation of the first UTF-16 char
	/// as a UTF-8 two-byte sequence packed into a WORD and zero-extended to DWORD.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 ExtractUtf8TwoByteSequenceFromFirstUtf16Char(UInt32 value)
	{
		UInt32 temp;
		if (BitConverter.IsLittleEndian)
		{
			temp = (value << 2) & 0x1F00u;
			value &= 0x3Fu;
			return BinaryPrimitives.ReverseEndianness((UInt16)(temp + value + 0xC080u));
		}
		temp = (value >> 16) & 0x3Fu;
		value = (value >> 14) & 0x1F00u;
		return value + temp + 0xC080u;
	}

	/// <summary>
	/// Given a 32-bit integer that represents two packed UTF-16 characters, all in machine-endian order,
	/// returns true iff the first UTF-16 character is ASCII.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsFirstCharAscii(UInt32 value)
		=> (BitConverter.IsLittleEndian && (value & 0xFF80u) == 0) ||
			(!BitConverter.IsLittleEndian && value < 0x0080_0000u);

	/// <summary>
	/// Given a 32-bit integer that represents two packed UTF-16 characters, all in machine-endian order,
	/// returns true iff the first UTF-16 character requires *at least* 3 bytes to encode in UTF-8.
	/// This also returns true if the first UTF-16 character is a surrogate character (well-formedness is not validated).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsFirstCharAtLeastThreeUtf8Bytes(UInt32 value)
		=> (BitConverter.IsLittleEndian && (value & 0xF800u) != 0) ||
			(!BitConverter.IsLittleEndian && value >= 0x0800_0000u);

	/// <summary>
	/// Given a 32-bit integer that represents two packed UTF-16 characters, all in machine-endian order,
	/// returns true iff the first UTF-16 character is a surrogate character (either high or low).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsFirstCharSurrogate(UInt32 value)
		=> (BitConverter.IsLittleEndian && ((value - 0xD800u) & 0xF800u) == 0) ||
			(!BitConverter.IsLittleEndian && value - 0xD800_0000u < 0x0800_0000u);

	/// <summary>
	/// Given a 32-bit integer that represents two packed UTF-16 characters, all in machine-endian order,
	/// returns true iff the first UTF-16 character would be encoded as exactly 2 bytes in UTF-8.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsFirstCharTwoUtf8Bytes(UInt32 value)
		=> (BitConverter.IsLittleEndian && ((value - 0x0080u) & 0xFFFFu) < 0x0780u) || (!BitConverter.IsLittleEndian &&
			UnicodeUtility.IsInRangeInclusive(value, 0x0080_0000u, 0x07FF_FFFFu));

	/// <summary>
	/// Returns <see langword="true"/> iff the low byte of <paramref name="value"/>
	/// is a UTF-8 continuation byte.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsLowByteUtf8ContinuationByte(UInt32 value) => (Byte)(value - 0x80u) <= 0x3Fu;

	/// <summary>
	/// Given a 32-bit integer that represents two packed UTF-16 characters, all in machine-endian order,
	/// returns true iff the second UTF-16 character is ASCII.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsSecondCharAscii(UInt32 value)
		=> (BitConverter.IsLittleEndian && value < 0x0080_0000u) ||
			(!BitConverter.IsLittleEndian && (value & 0xFF80u) == 0);

	/// <summary>
	/// Given a 32-bit integer that represents two packed UTF-16 characters, all in machine-endian order,
	/// returns true iff the second UTF-16 character requires *at least* 3 bytes to encode in UTF-8.
	/// This also returns true if the second UTF-16 character is a surrogate character (well-formedness is not validated).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsSecondCharAtLeastThreeUtf8Bytes(UInt32 value)
		=> (BitConverter.IsLittleEndian && (value & 0xF800_0000u) != 0) ||
			(!BitConverter.IsLittleEndian && (value & 0xF800u) != 0);

	/// <summary>
	/// Given a 32-bit integer that represents two packed UTF-16 characters, all in machine-endian order,
	/// returns true iff the second UTF-16 character is a surrogate character (either high or low).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsSecondCharSurrogate(UInt32 value)
		=> (BitConverter.IsLittleEndian && value - 0xD800_0000u < 0x0800_0000u) ||
			(!BitConverter.IsLittleEndian && ((value - 0xD800u) & 0xF800u) == 0);

	/// <summary>
	/// Given a 32-bit integer that represents two packed UTF-16 characters, all in machine-endian order,
	/// returns true iff the second UTF-16 character would be encoded as exactly 2 bytes in UTF-8.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsSecondCharTwoUtf8Bytes(UInt32 value)
		=> (BitConverter.IsLittleEndian && UnicodeUtility.IsInRangeInclusive(value, 0x0080_0000u, 0x07FF_FFFFu)) ||
			(!BitConverter.IsLittleEndian && ((value - 0x0080u) & 0xFFFFu) < 0x0780u);

	/// <summary>
	/// Returns <see langword="true"/> iff <paramref name="value"/> is a UTF-8 continuation byte;
	/// i.e., has binary representation 10xxxxxx, where x is any bit.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean IsUtf8ContinuationByte(in Byte value) => (SByte)value < -64;

	/// <summary>
	/// Given a 32-bit integer that represents two packed UTF-16 characters, all in machine-endian order,
	/// returns true iff the two characters represent a well-formed UTF-16 surrogate pair.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsWellFormedUtf16SurrogatePair(UInt32 value)
		=> (BitConverter.IsLittleEndian && ((value - 0xDC00_D800u) & 0xFC00_FC00u) == 0) ||
			(!BitConverter.IsLittleEndian && ((value - 0xD800_DC00u) & 0xFC00_FC00u) == 0);

	/// <summary>
	/// Converts a DWORD from machine-endian to little-endian.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 ToLittleEndian(UInt32 value)
		=> BitConverter.IsLittleEndian ? value : BinaryPrimitives.ReverseEndianness(value);

	/// <summary>
	/// Given a UTF-8 buffer which has been read into a DWORD in machine endianness,
	/// returns <see langword="true"/> iff the first two bytes of the buffer are
	/// an overlong representation of a sequence that should be represented as one byte.
	/// This method *does not* validate that the sequence matches the appropriate
	/// 2-byte sequence mask (see <see cref="UInt32BeginsWithUtf8TwoByteMask"/>).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean UInt32BeginsWithOverlongUtf8TwoByteSequence(UInt32 value)
		=> (BitConverter.IsLittleEndian && (Byte)value < 0xC2u) ||
			(!BitConverter.IsLittleEndian && value < 0xC200_0000u);

	/// <summary>
	/// Given a UTF-8 buffer which has been read into a DWORD in machine endianness,
	/// returns <see langword="true"/> iff the first four bytes of the buffer match
	/// the UTF-8 4-byte sequence mask [ 11110www 10zzzzzz 10yyyyyy 10xxxxxx ]. This
	/// method *does not* validate that the sequence is well-formed; the caller must
	/// still perform overlong form or out-of-range checking.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean UInt32BeginsWithUtf8FourByteMask(UInt32 value)
		=> (BitConverter.IsLittleEndian && ((value - 0x8080_80F0u) & 0xC0C0_C0F8u) == 0) ||
			(!BitConverter.IsLittleEndian && ((value - 0xF080_8080u) & 0xF8C0_C0C0u) == 0);

	/// <summary>
	/// Given a UTF-8 buffer which has been read into a DWORD in machine endianness,
	/// returns <see langword="true"/> iff the first three bytes of the buffer match
	/// the UTF-8 3-byte sequence mask [ 1110zzzz 10yyyyyy 10xxxxxx ]. This method *does not*
	/// validate that the sequence is well-formed; the caller must still perform
	/// overlong form or surrogate checking.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean UInt32BeginsWithUtf8ThreeByteMask(UInt32 value)
		=> (BitConverter.IsLittleEndian && ((value - 0x0080_80E0u) & 0x00C0_C0F0u) == 0) ||
			(!BitConverter.IsLittleEndian && ((value - 0xE080_8000u) & 0xF0C0_C000u) == 0);

	/// <summary>
	/// Given a UTF-8 buffer which has been read into a DWORD in machine endianness,
	/// returns <see langword="true"/> iff the first two bytes of the buffer match
	/// the UTF-8 2-byte sequence mask [ 110yyyyy 10xxxxxx ]. This method *does not*
	/// validate that the sequence is well-formed; the caller must still perform
	/// overlong form checking.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean UInt32BeginsWithUtf8TwoByteMask(UInt32 value)
		=> (BitConverter.IsLittleEndian && ((value - 0x0000_80C0u) & 0x0000_C0E0u) == 0) ||
			(!BitConverter.IsLittleEndian && ((value - 0xC080_0000u) & 0xE0C0_0000u) == 0);

	/// <summary>
	/// Given a UTF-8 buffer which has been read into a DWORD in machine endianness,
	/// returns <see langword="true"/> iff the first two bytes of the buffer are
	/// an overlong representation of a sequence that should be represented as one byte.
	/// This method *does not* validate that the sequence matches the appropriate
	/// 2-byte sequence mask (see <see cref="UInt32BeginsWithUtf8TwoByteMask"/>).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean UInt32EndsWithOverlongUtf8TwoByteSequence(UInt32 value)
		=> (BitConverter.IsLittleEndian && (value & 0x001E_0000u) == 0) ||
			(!BitConverter.IsLittleEndian && (value & 0x1E00u) == 0);

	/// <summary>
	/// Given a UTF-8 buffer which has been read into a DWORD in machine endianness,
	/// returns <see langword="true"/> iff the last two bytes of the buffer match
	/// the UTF-8 2-byte sequence mask [ 110yyyyy 10xxxxxx ]. This method *does not*
	/// validate that the sequence is well-formed; the caller must still perform
	/// overlong form checking.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean UInt32EndsWithUtf8TwoByteMask(UInt32 value)
		=> (BitConverter.IsLittleEndian && ((value - 0x80C0_0000u) & 0xC0E0_0000u) == 0) ||
			(!BitConverter.IsLittleEndian && ((value - 0x0000_C080u) & 0x0000_E0C0u) == 0);

	/// <summary>
	/// Given a UTF-8 buffer which has been read into a DWORD on a little-endian machine,
	/// returns <see langword="true"/> iff the first two bytes of the buffer are a well-formed
	/// UTF-8 two-byte sequence. This wraps the mask check and the overlong check into a
	/// single operation. Returns <see langword="false"/> if running on a big-endian machine.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean UInt32BeginsWithValidUtf8TwoByteSequenceLittleEndian(UInt32 value)
		=> BitConverter.IsLittleEndian && UnicodeUtility.IsInRangeInclusive(value & 0xC0FFu, 0x80C2u, 0x80DFu);

	/// <summary>
	/// Given a UTF-8 buffer which has been read into a DWORD on a little-endian machine,
	/// returns <see langword="true"/> iff the last two bytes of the buffer are a well-formed
	/// UTF-8 two-byte sequence. This wraps the mask check and the overlong check into a
	/// single operation. Returns <see langword="false"/> if running on a big-endian machine.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean UInt32EndsWithValidUtf8TwoByteSequenceLittleEndian(UInt32 value)
		=> BitConverter.IsLittleEndian &&
			UnicodeUtility.IsInRangeInclusive(value & 0xC0FF_0000u, 0x80C2_0000u, 0x80DF_0000u);

	/// <summary>
	/// Given a UTF-8 buffer which has been read into a DWORD in machine endianness,
	/// returns <see langword="true"/> iff the first byte of the buffer is ASCII.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean UInt32FirstByteIsAscii(UInt32 value)
		=> (BitConverter.IsLittleEndian && (value & 0x80u) == 0) || (!BitConverter.IsLittleEndian && (Int32)value >= 0);

	/// <summary>
	/// Given a UTF-8 buffer which has been read into a DWORD in machine endianness,
	/// returns <see langword="true"/> iff the fourth byte of the buffer is ASCII.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean UInt32FourthByteIsAscii(UInt32 value)
		=> (BitConverter.IsLittleEndian && (Int32)value >= 0) || (!BitConverter.IsLittleEndian && (value & 0x80u) == 0);

	/// <summary>
	/// Given a UTF-8 buffer which has been read into a DWORD in machine endianness,
	/// returns <see langword="true"/> iff the second byte of the buffer is ASCII.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean UInt32SecondByteIsAscii(UInt32 value)
		=> (BitConverter.IsLittleEndian && (value & 0x8000u) == 0) ||
			(!BitConverter.IsLittleEndian && (value & 0x0080_0000u) == 0);

	/// <summary>
	/// Given a UTF-8 buffer which has been read into a DWORD in machine endianness,
	/// returns <see langword="true"/> iff the third byte of the buffer is ASCII.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean UInt32ThirdByteIsAscii(UInt32 value)
		=> (BitConverter.IsLittleEndian && (value & 0x0080_0000u) == 0) ||
			(!BitConverter.IsLittleEndian && (value & 0x8000u) == 0);

	/// <summary>
	/// Given a DWORD which represents a buffer of 2 packed UTF-16 values in machine endianness,
	/// converts those scalar values to their 3-byte UTF-8 representation and writes the
	/// resulting 6 bytes to the destination buffer.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1121)]
#endif
	private static void WriteTwoUtf16CharsAsTwoUtf8ThreeByteSequences(ref Byte outputBuffer, UInt32 value)
	{
		if (BitConverter.IsLittleEndian)
		{
			UInt32 tempA = ((value << 2) & 0x3F00u) | ((value & 0x3Fu) << 16);
			UInt32 tempB = ((value >> 4) & 0x0F00_0000u) | ((value >> 12) & 0x0Fu);
			Unsafe.WriteUnaligned(ref outputBuffer, tempA + tempB + 0xE080_80E0u);
			Unsafe.WriteUnaligned(ref Unsafe.Add(ref outputBuffer, 4),
			                      (UInt16)(((value >> 22) & 0x3Fu) + ((value >> 8) & 0x3F00u) + 0x8080u));
		}
		else
		{
			Unsafe.Add(ref outputBuffer, 5) = (Byte)((value & 0x3Fu) | 0x80u);
			Unsafe.Add(ref outputBuffer, 4) = (Byte)(((value >>= 6) & 0x3Fu) | 0x80u);
			Unsafe.Add(ref outputBuffer, 3) = (Byte)(((value >>= 6) & 0x0Fu) | 0xE0u);
			Unsafe.Add(ref outputBuffer, 2) = (Byte)(((value >>= 4) & 0x3Fu) | 0x80u);
			Unsafe.Add(ref outputBuffer, 1) = (Byte)(((value >>= 6) & 0x3Fu) | 0x80u);
			outputBuffer = (Byte)((value >> 6) | 0xE0u);
		}
	}

	/// <summary>
	/// Given a DWORD which represents a buffer of 2 packed UTF-16 values in machine endianness,
	/// converts the first UTF-16 value to its 3-byte UTF-8 representation and writes the
	/// resulting 3 bytes to the destination buffer.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1121)]
#endif
	private static void WriteFirstUtf16CharAsUtf8ThreeByteSequence(ref Byte outputBuffer, UInt32 value)
	{
		if (BitConverter.IsLittleEndian)
		{
			UInt32 tempA = (value << 2) & 0x3F00u;
			UInt32 tempB = (UInt32)(UInt16)value >> 12;
			Unsafe.WriteUnaligned(ref outputBuffer, (UInt16)(tempA + tempB + 0x80E0u));
			Unsafe.Add(ref outputBuffer, 2) = (Byte)((value & 0x3Fu) | ~0x7Fu);
		}
		else
		{
			Unsafe.Add(ref outputBuffer, 2) = (Byte)(((value >>= 16) & 0x3Fu) | 0x80u);
			Unsafe.Add(ref outputBuffer, 1) = (Byte)(((value >>= 6) & 0x3Fu) | 0x80u);
			outputBuffer = (Byte)((value >> 6) | 0xE0u);
		}
	}
}
#endif