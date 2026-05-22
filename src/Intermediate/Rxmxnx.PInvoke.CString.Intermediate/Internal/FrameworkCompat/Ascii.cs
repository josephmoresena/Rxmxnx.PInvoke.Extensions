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
// (System.text.Ascii)

#if !NETCOREAPP
namespace System.Text;

// ReSharper disable BuiltInTypeReferenceStyle
using UIntPtr = nuint;

/// <summary>
/// Provides static methods for ASCII encoding.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1199)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS907)]
#endif
internal static unsafe class Ascii
{
	/// <summary>
	/// A mask which selects only the high bit of each byte of the given <see cref="uint"/>.
	/// </summary>
	private const UInt32 uInt32HighBitsOnlyMask = 0x80808080u;
	/// <summary>
	/// Returns <see langword="true"/> iff all chars in <paramref name="value"/> are ASCII.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean AllCharsInUInt32AreAscii(UInt32 value) => (value & ~0x007F007Fu) == 0;

	/// <summary>
	/// Returns <see langword="true"/> iff all chars in <paramref name="value"/> are ASCII.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean AllCharsInUInt64AreAscii(UInt64 value) => (value & ~0x007F007F_007F007Ful) == 0;

	/// <summary>
	/// Given a DWORD which represents two packed chars in machine-endian order,
	/// <see langword="true"/> iff the first char (in machine-endian order) is ASCII.
	/// </summary>
	/// <param name="value"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean FirstCharInUInt32IsAscii(UInt32 value)
		=> (BitConverter.IsLittleEndian && (value & 0xFF80u) == 0) ||
			(!BitConverter.IsLittleEndian && (value & 0xFF800000u) == 0);

	/// <summary>
	/// Returns the index in <paramref name="pBuffer"/> where the first non-ASCII byte is found.
	/// Returns <paramref name="bufferLength"/> if the buffer is empty or all-ASCII.
	/// </summary>
	/// <returns>An ASCII byte is defined as 0x00 - 0x7F, inclusive.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static UIntPtr GetIndexOfFirstNonAsciiByte(Byte* pBuffer, UIntPtr bufferLength)
		=> Ascii.GetIndexOfFirstNonAsciiByte_Vector(pBuffer, bufferLength);

	private static UIntPtr GetIndexOfFirstNonAsciiByte_Vector(Byte* pBuffer, UIntPtr bufferLength)
	{
		Byte* pOriginalBuffer = pBuffer;
		UInt32 currentUInt32;
		for (; bufferLength >= 8; bufferLength -= 8)
		{
			currentUInt32 = Unsafe.ReadUnaligned<UInt32>(pBuffer);
			UInt32 nextUInt32 = Unsafe.ReadUnaligned<UInt32>(pBuffer + 4);

			if (!Ascii.AllBytesInUInt32AreAscii(currentUInt32 | nextUInt32))
			{
				if (Ascii.AllBytesInUInt32AreAscii(currentUInt32))
				{
					currentUInt32 = nextUInt32;
					pBuffer += 4;
				}

				goto FoundNonAsciiData;
			}

			pBuffer += 8;
		}

		if ((bufferLength & 4) != 0)
		{
			currentUInt32 = Unsafe.ReadUnaligned<UInt32>(pBuffer);
			if (!Ascii.AllBytesInUInt32AreAscii(currentUInt32)) goto FoundNonAsciiData;

			pBuffer += 4;
		}

		if ((bufferLength & 2) != 0)
		{
			currentUInt32 = Unsafe.ReadUnaligned<UInt16>(pBuffer);
			if (!Ascii.AllBytesInUInt32AreAscii(currentUInt32))
			{
				if (!BitConverter.IsLittleEndian) currentUInt32 <<= 16;
				goto FoundNonAsciiData;
			}

			pBuffer += 2;
		}
		if ((bufferLength & 1) != 0)
			if (*(SByte*)pBuffer >= 0)
				pBuffer++;

		Finish:

		UIntPtr totalNumBytesRead = (UIntPtr)pBuffer - (UIntPtr)pOriginalBuffer;
		return totalNumBytesRead;

		FoundNonAsciiData:
		pBuffer += Ascii.CountNumberOfLeadingAsciiBytesFromUInt32WithSomeNonAsciiData(currentUInt32);
		goto Finish;
	}

	/// <summary>
	/// Returns the index in <paramref name="pBuffer"/> where the first non-ASCII char is found.
	/// Returns <paramref name="bufferLength"/> if the buffer is empty or all-ASCII.
	/// </summary>
	/// <returns>An ASCII char is defined as 0x0000 - 0x007F, inclusive.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static UIntPtr GetIndexOfFirstNonAsciiChar(Char* pBuffer, UIntPtr bufferLength)
		=> Ascii.GetIndexOfFirstNonAsciiChar_Vector(pBuffer, bufferLength);

	private static UIntPtr GetIndexOfFirstNonAsciiChar_Vector(Char* pBuffer, UIntPtr bufferLength)
	{
		Char* pOriginalBuffer = pBuffer;
		UInt32 currentUInt32;

		for (; bufferLength >= 4; bufferLength -= 4)
		{
			currentUInt32 = Unsafe.ReadUnaligned<UInt32>(pBuffer);
			UInt32 nextUInt32 = Unsafe.ReadUnaligned<UInt32>(pBuffer + 4 / sizeof(Char));

			if (!Ascii.AllCharsInUInt32AreAscii(currentUInt32 | nextUInt32))
			{
				if (Ascii.AllCharsInUInt32AreAscii(currentUInt32))
				{
					currentUInt32 = nextUInt32;
					pBuffer += 2;
				}

				goto FoundNonAsciiData;
			}

			pBuffer += 4;
		}

		if ((bufferLength & 2) != 0)
		{
			currentUInt32 = Unsafe.ReadUnaligned<UInt32>(pBuffer);
			if (!Ascii.AllCharsInUInt32AreAscii(currentUInt32)) goto FoundNonAsciiData;

			pBuffer += 2;
		}

		if ((bufferLength & 1) != 0)
			if (*pBuffer <= 0x007F)
				pBuffer++;

		Finish:

		UIntPtr totalNumBytesRead = (UIntPtr)pBuffer - (UIntPtr)pOriginalBuffer;
		return totalNumBytesRead / sizeof(Char);

		FoundNonAsciiData:
		if (Ascii.FirstCharInUInt32IsAscii(currentUInt32)) pBuffer++;

		goto Finish;
	}

	/// <summary>
	/// Given a QWORD which represents a buffer of 4 ASCII chars in machine-endian order,
	/// narrows each WORD to a BYTE, then writes the 4-byte result to the output buffer
	/// also in machine-endian order.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void NarrowFourUtf16CharsToAsciiAndWriteToBuffer(ref Byte outputBuffer, UInt64 value)
	{
		if (BitConverter.IsLittleEndian)
		{
			outputBuffer = (Byte)value;
			value >>= 16;
			Unsafe.Add(ref outputBuffer, 1) = (Byte)value;
			value >>= 16;
			Unsafe.Add(ref outputBuffer, 2) = (Byte)value;
			value >>= 16;
			Unsafe.Add(ref outputBuffer, 3) = (Byte)value;
		}
		else
		{
			Unsafe.Add(ref outputBuffer, 3) = (Byte)value;
			value >>= 16;
			Unsafe.Add(ref outputBuffer, 2) = (Byte)value;
			value >>= 16;
			Unsafe.Add(ref outputBuffer, 1) = (Byte)value;
			value >>= 16;
			outputBuffer = (Byte)value;
		}
	}

	/// <summary>
	/// Given a DWORD which represents a buffer of 2 ASCII chars in machine-endian order,
	/// narrows each WORD to a BYTE, then writes the 2-byte result to the output buffer also in
	/// machine-endian order.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void NarrowTwoUtf16CharsToAsciiAndWriteToBuffer(ref Byte outputBuffer, UInt32 value)
	{
		if (BitConverter.IsLittleEndian)
		{
			outputBuffer = (Byte)value;
			Unsafe.Add(ref outputBuffer, 1) = (Byte)(value >> 16);
		}
		else
		{
			Unsafe.Add(ref outputBuffer, 1) = (Byte)value;
			outputBuffer = (Byte)(value >> 16);
		}
	}

	/// <summary>
	/// Copies as many ASCII characters (U+0000..U+007F) as possible from <paramref name="pUtf16Buffer"/>
	/// to <paramref name="pAsciiBuffer"/>, stopping when the first non-ASCII character is encountered
	/// or once <paramref name="elementCount"/> elements have been converted. Returns the total number
	/// of elements that were able to be converted.
	/// </summary>
	internal static UIntPtr NarrowUtf16ToAscii(Char* pUtf16Buffer, Byte* pAsciiBuffer, UIntPtr elementCount)
	{
		UIntPtr currentOffset = 0;
		UInt32 utf16Data32BitsHigh = 0, utf16Data32BitsLow = 0;
		UInt64 utf16Data64Bits = 0;
		UIntPtr remainingElementCount = elementCount - currentOffset;

		if (remainingElementCount >= 4)
		{
			UIntPtr finalOffsetWhereCanLoop = currentOffset + remainingElementCount - 4;
			do
			{
				if (IntPtr.Size >= 8)
				{
					utf16Data64Bits = Unsafe.ReadUnaligned<UInt64>(pUtf16Buffer + currentOffset);
					if (!Ascii.AllCharsInUInt64AreAscii(utf16Data64Bits)) goto FoundNonAsciiDataIn64BitRead;
					Ascii.NarrowFourUtf16CharsToAsciiAndWriteToBuffer(ref pAsciiBuffer[currentOffset], utf16Data64Bits);
				}
				else
				{
					utf16Data32BitsHigh = Unsafe.ReadUnaligned<uint>(pUtf16Buffer + currentOffset);
					utf16Data32BitsLow = Unsafe.ReadUnaligned<uint>(pUtf16Buffer + currentOffset + 4 / sizeof(Char));
					if (!Ascii.AllCharsInUInt32AreAscii(utf16Data32BitsHigh | utf16Data32BitsLow))
						goto FoundNonAsciiDataIn64BitRead;

					Ascii.NarrowTwoUtf16CharsToAsciiAndWriteToBuffer(ref pAsciiBuffer[currentOffset],
					                                                 utf16Data32BitsHigh);
					Ascii.NarrowTwoUtf16CharsToAsciiAndWriteToBuffer(ref pAsciiBuffer[currentOffset + 2],
					                                                 utf16Data32BitsLow);
				}

				currentOffset += 4;
			} while (currentOffset <= finalOffsetWhereCanLoop);
		}
		if (((UInt32)remainingElementCount & 2) != 0)
		{
			utf16Data32BitsHigh = Unsafe.ReadUnaligned<UInt32>(pUtf16Buffer + currentOffset);
			if (!Ascii.AllCharsInUInt32AreAscii(utf16Data32BitsHigh)) goto FoundNonAsciiDataInHigh32Bits;

			Ascii.NarrowTwoUtf16CharsToAsciiAndWriteToBuffer(ref pAsciiBuffer[currentOffset], utf16Data32BitsHigh);
			currentOffset += 2;
		}

		if (((UInt32)remainingElementCount & 1) != 0)
		{
			utf16Data32BitsHigh = pUtf16Buffer[currentOffset];
			if (utf16Data32BitsHigh <= 0x007Fu)
			{
				pAsciiBuffer[currentOffset] = (Byte)utf16Data32BitsHigh;
				currentOffset++;
			}
		}

		Finish:

		return currentOffset;

		FoundNonAsciiDataIn64BitRead:

		if (IntPtr.Size >= 8)
		{
			if (BitConverter.IsLittleEndian)
				utf16Data32BitsHigh = (UInt32)utf16Data64Bits;
			else
				utf16Data32BitsHigh = (UInt32)(utf16Data64Bits >> 32);

			if (Ascii.AllCharsInUInt32AreAscii(utf16Data32BitsHigh))
			{
				Ascii.NarrowTwoUtf16CharsToAsciiAndWriteToBuffer(ref pAsciiBuffer[currentOffset], utf16Data32BitsHigh);

				if (BitConverter.IsLittleEndian)
					utf16Data32BitsHigh = (UInt32)(utf16Data64Bits >> 32);
				else
					utf16Data32BitsHigh = (UInt32)utf16Data64Bits;

				currentOffset += 2;
			}
		}
		else
		{
			if (Ascii.AllCharsInUInt32AreAscii(utf16Data32BitsHigh))
			{
				Ascii.NarrowTwoUtf16CharsToAsciiAndWriteToBuffer(ref pAsciiBuffer[currentOffset], utf16Data32BitsHigh);
				utf16Data32BitsHigh = utf16Data32BitsLow;
				currentOffset += 2;
			}
		}

		FoundNonAsciiDataInHigh32Bits:

		if (Ascii.FirstCharInUInt32IsAscii(utf16Data32BitsHigh))
		{
			if (!BitConverter.IsLittleEndian) utf16Data32BitsHigh >>= 16;
			pAsciiBuffer[currentOffset] = (Byte)utf16Data32BitsHigh;
			currentOffset++;
		}

		goto Finish;
	}

	/// <summary>
	/// Copies as many ASCII bytes (00..7F) as possible from <paramref name="pAsciiBuffer"/>
	/// to <paramref name="pUtf16Buffer"/>, stopping when the first non-ASCII byte is encountered
	/// or once <paramref name="elementCount"/> elements have been converted. Returns the total number
	/// of elements that were able to be converted.
	/// </summary>
	internal static UIntPtr WidenAsciiToUtf16(Byte* pAsciiBuffer, Char* pUtf16Buffer, UIntPtr elementCount)
	{
		UIntPtr currentOffset = 0;
		UIntPtr remainingElementCount = elementCount - currentOffset;
		UInt32 asciiData;

		if (remainingElementCount >= 4)
		{
			UIntPtr finalOffsetWhereCanLoop = currentOffset + remainingElementCount - 4;
			do
			{
				asciiData = Unsafe.ReadUnaligned<UInt32>(pAsciiBuffer + currentOffset);
				if (!Ascii.AllBytesInUInt32AreAscii(asciiData)) goto FoundNonAsciiData;

				Ascii.WidenFourAsciiBytesToUtf16AndWriteToBuffer(ref pUtf16Buffer[currentOffset], asciiData);
				currentOffset += 4;
			} while (currentOffset <= finalOffsetWhereCanLoop);
		}
		if (((UInt32)remainingElementCount & 2) != 0)
		{
			asciiData = Unsafe.ReadUnaligned<UInt16>(pAsciiBuffer + currentOffset);
			if (!Ascii.AllBytesInUInt32AreAscii(asciiData))
			{
				if (!BitConverter.IsLittleEndian) asciiData <<= 16;
				goto FoundNonAsciiData;
			}

			if (BitConverter.IsLittleEndian)
			{
				pUtf16Buffer[currentOffset] = (Char)(Byte)asciiData;
				pUtf16Buffer[currentOffset + 1] = (Char)(asciiData >> 8);
			}
			else
			{
				pUtf16Buffer[currentOffset + 1] = (Char)(Byte)asciiData;
				pUtf16Buffer[currentOffset] = (Char)(asciiData >> 8);
			}

			currentOffset += 2;
		}

		// Try to widen 8 bits -> 16 bits.

		if (((UInt32)remainingElementCount & 1) != 0)
		{
			asciiData = pAsciiBuffer[currentOffset];
			if (((Byte)asciiData & 0x80) != 0) goto Finish;

			pUtf16Buffer[currentOffset] = (Char)asciiData;
			currentOffset++;
		}

		Finish:

		return currentOffset;

		FoundNonAsciiData:
		if (BitConverter.IsLittleEndian)
			while (((Byte)asciiData & 0x80) == 0)
			{
				pUtf16Buffer[currentOffset] = (Char)(Byte)asciiData;
				currentOffset++;
				asciiData >>= 8;
			}
		else
			while ((asciiData & 0x80000000) == 0)
			{
				asciiData = BitOperations.RotateLeft(asciiData, 8);
				pUtf16Buffer[currentOffset] = (Char)(Byte)asciiData;
				currentOffset++;
			}

		goto Finish;
	}

	/// <summary>
	/// Given a DWORD which represents a buffer of 4 bytes, widens the buffer into 4 WORDs and
	/// writes them to the output buffer with machine endianness.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static void WidenFourAsciiBytesToUtf16AndWriteToBuffer(ref char outputBuffer, uint value)
	{
		if (BitConverter.IsLittleEndian)
		{
			outputBuffer = (Char)(Byte)value;
			value >>= 8;
			Unsafe.Add(ref outputBuffer, 1) = (Char)(Byte)value;
			value >>= 8;
			Unsafe.Add(ref outputBuffer, 2) = (Char)(Byte)value;
			value >>= 8;
			Unsafe.Add(ref outputBuffer, 3) = (Char)value;
		}
		else
		{
			Unsafe.Add(ref outputBuffer, 3) = (Char)(Byte)value;
			value >>= 8;
			Unsafe.Add(ref outputBuffer, 2) = (Char)(Byte)value;
			value >>= 8;
			Unsafe.Add(ref outputBuffer, 1) = (Char)(Byte)value;
			value >>= 8;
			outputBuffer = (Char)value;
		}
	}

	/// <summary>
	/// Returns <see langword="true"/> iff all bytes in <paramref name="value"/> are ASCII.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean AllBytesInUInt32AreAscii(UInt32 value) => (value & Ascii.uInt32HighBitsOnlyMask) == 0;

	/// <summary>
	/// Given a DWORD which represents a four-byte buffer read in machine endianness, and which
	/// the caller has asserted contains a non-ASCII byte *somewhere* in the data, counts the
	/// number of consecutive ASCII bytes starting from the beginning of the buffer. Returns
	/// a value 0 - 3, inclusive. (The caller is responsible for ensuring that the buffer doesn't
	/// contain all-ASCII data.)
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static UInt32 CountNumberOfLeadingAsciiBytesFromUInt32WithSomeNonAsciiData(UInt32 value)
	{
		if (BitConverter.IsLittleEndian)
			return (UInt32)BitOperations.TrailingZeroCount(value & Ascii.uInt32HighBitsOnlyMask) >> 3;
		value = ~value;
		value = BitOperations.RotateLeft(value, 1);

		UInt32 allBytesUpToNowAreAscii = value & 1;
		UInt32 numAsciiBytes = allBytesUpToNowAreAscii;

		value = BitOperations.RotateLeft(value, 8);
		allBytesUpToNowAreAscii &= value;
		numAsciiBytes += allBytesUpToNowAreAscii;

		value = BitOperations.RotateLeft(value, 8);
		allBytesUpToNowAreAscii &= value;
		numAsciiBytes += allBytesUpToNowAreAscii;

		return numAsciiBytes;
	}
}
#endif