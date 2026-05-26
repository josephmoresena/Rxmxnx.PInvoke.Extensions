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

/// <summary>
/// Provides static methods for UTF-8 encoding.
/// </summary>
internal static partial class Utf8Utility
{
	/// <summary>
	/// The maximum number of bytes that can result from UTF-8 transcoding
	/// any Unicode scalar value.
	/// </summary>
	internal const Int32 MaxBytesPerScalar = 4;

	/// <summary>
	/// Returns the byte index in <paramref name="utf8Data"/> where the first invalid UTF-8 sequence begins,
	/// or -1 if the buffer contains no invalid sequences. Also outs the <paramref name="isAscii"/> parameter
	/// stating whether all data observed (up to the first invalid sequence or the end of the buffer, whichever
	/// comes first) is ASCII.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe Int32 GetIndexOfFirstInvalidUtf8Sequence(ReadOnlySpan<Byte> utf8Data, out Boolean isAscii)
	{
		fixed (Byte* pUtf8Data = &MemoryMarshal.GetReference(utf8Data))
		{
			Byte* pFirstInvalidByte =
				Utf8Utility.GetPointerToFirstInvalidByte(pUtf8Data, utf8Data.Length,
				                                         out Int32 utf16CodeUnitCountAdjustment, out _);
			Int32 index = (Int32)(void*)Unsafe.ByteOffset(ref *pUtf8Data, ref *pFirstInvalidByte);

			isAscii = utf16CodeUnitCountAdjustment == 0;
			return index < utf8Data.Length ? index : -1;
		}
	}

	/// <summary>
	/// Returns true iff the UInt32 represents four ASCII UTF-8 characters in machine endianness.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean AllBytesInUInt32AreAscii(UInt32 value) => (value & ~0x7F7F_7F7Fu) == 0;

	/// <summary>
	/// Returns true iff the UInt64 represents eighty ASCII UTF-8 characters in machine endianness.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean AllBytesInUInt64AreAscii(UInt64 value) => (value & ~0x7F7F_7F7F_7F7F_7F7Ful) == 0;

	/// <summary>
	/// Given a UInt32 that represents four ASCII UTF-8 characters, returns the invariant
	/// lowercase representation of those characters. Requires the input value to contain
	/// four ASCII UTF-8 characters in machine endianness.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static UInt32 ConvertAllAsciiBytesInUInt32ToLowercase(UInt32 value)
	{
		UInt32 lowerIndicator = value + 0x8080_8080u - 0x4141_4141u;
		UInt32 upperIndicator = value + 0x8080_8080u - 0x5B5B_5B5Bu;
		UInt32 combinedIndicator = lowerIndicator ^ upperIndicator;
		UInt32 mask = (combinedIndicator & 0x8080_8080u) >> 2;
		return value ^ mask;
	}

	/// <summary>
	/// Given a UInt32 that represents four ASCII UTF-8 characters, returns the invariant
	/// uppercase representation of those characters. Requires the input value to contain
	/// four ASCII UTF-8 characters in machine endianness.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static UInt32 ConvertAllAsciiBytesInUInt32ToUppercase(UInt32 value)
	{
		UInt32 lowerIndicator = value + 0x8080_8080u - 0x6161_6161u;
		UInt32 upperIndicator = value + 0x8080_8080u - 0x7B7B_7B7Bu;
		UInt32 combinedIndicator = lowerIndicator ^ upperIndicator;
		UInt32 mask = (combinedIndicator & 0x8080_8080u) >> 2;
		return value ^ mask;
	}

	/// <summary>
	/// Given a UInt64 that represents eight ASCII UTF-8 characters, returns the invariant
	/// uppercase representation of those characters. Requires the input value to contain
	/// eight ASCII UTF-8 characters in machine endianness.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static UInt64 ConvertAllAsciiBytesInUInt64ToUppercase(UInt64 value)
	{
		UInt64 lowerIndicator = value + 0x8080_8080_8080_8080ul - 0x6161_6161_6161_6161ul;
		UInt64 upperIndicator = value + 0x8080_8080_8080_8080ul - 0x7B7B_7B7B_7B7B_7B7Bul;
		UInt64 combinedIndicator = lowerIndicator ^ upperIndicator;
		UInt64 mask = (combinedIndicator & 0x8080_8080_8080_8080ul) >> 2;
		return value ^ mask;
	}

	/// <summary>
	/// Given a UInt64 that represents eight ASCII UTF-8 characters, returns the invariant
	/// uppercase representation of those characters. Requires the input value to contain
	/// eight ASCII UTF-8 characters in machine endianness.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static UInt64 ConvertAllAsciiBytesInUInt64ToLowercase(UInt64 value)
	{
		UInt64 lowerIndicator = value + 0x8080_8080_8080_8080ul - 0x4141_4141_4141_4141ul;
		UInt64 upperIndicator = value + 0x8080_8080_8080_8080ul - 0x5B5B_5B5B_5B5B_5B5Bul;
		UInt64 combinedIndicator = lowerIndicator ^ upperIndicator;
		UInt64 mask = (combinedIndicator & 0x8080_8080_8080_8080ul) >> 2;
		return value ^ mask;
	}

	/// <summary>
	/// Given two UInt32s that represent four ASCII UTF-8 characters each, returns true iff
	/// the two inputs are equal using an ordinal case-insensitive comparison.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean UInt32OrdinalIgnoreCaseAscii(UInt32 valueA, UInt32 valueB)
	{
		UInt32 letterMaskA = (((valueA + 0x3F3F3F3F) ^ (valueA + 0x25252525)) & 0x80808080) >> 2;
		UInt32 letterMaskB = (((valueB + 0x3F3F3F3F) ^ (valueB + 0x25252525)) & 0x80808080) >> 2;
		return (valueA | letterMaskA) == (valueB | letterMaskB);
	}

	/// <summary>
	/// Given two UInt64s that represent eight ASCII UTF-8 characters each, returns true iff
	/// the two inputs are equal using an ordinal case-insensitive comparison.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean UInt64OrdinalIgnoreCaseAscii(UInt64 valueA, UInt64 valueB)
	{
		UInt64 letterMaskA = (((valueA + 0x3F3F3F3F3F3F3F3F) ^ (valueA + 0x2525252525252525)) & 0x8080808080808080) >>
			2;
		UInt64 letterMaskB = (((valueB + 0x3F3F3F3F3F3F3F3F) ^ (valueB + 0x2525252525252525)) & 0x8080808080808080) >>
			2;
		return (valueA | letterMaskA) == (valueB | letterMaskB);
	}
}
#endif