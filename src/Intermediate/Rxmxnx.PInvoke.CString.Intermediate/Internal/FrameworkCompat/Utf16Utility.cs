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
// (System.text.Unicode.Utf16Utility)

#if !NETCOREAPP
namespace System.Text.Unicode;

/// <summary>
/// UTF-16 text utilities.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal static class Utf16Utility
{
	/// <summary>
	/// Returns true iff the UInt32 represents two ASCII UTF-16 characters in machine endianness.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean AllCharsInUInt32AreAscii(UInt32 value) => (value & ~0x007F_007Fu) == 0;

	/// <summary>
	/// Returns true iff the UInt64 represents four ASCII UTF-16 characters in machine endianness.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean AllCharsInUInt64AreAscii(UInt64 value) => (value & ~0x007F_007F_007F_007Ful) == 0;

	/// <summary>
	/// Given a UInt32 that represents two ASCII UTF-16 characters, returns the invariant
	/// lowercase representation of those characters. Requires the input value to contain
	/// two ASCII UTF-16 characters in machine endianness.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static UInt32 ConvertAllAsciiCharsInUInt32ToLowercase(UInt32 value)
	{
		UInt32 lowerIndicator = value + 0x0080_0080u - 0x0041_0041u;
		UInt32 upperIndicator = value + 0x0080_0080u - 0x005B_005Bu;
		UInt32 combinedIndicator = lowerIndicator ^ upperIndicator;
		UInt32 mask = (combinedIndicator & 0x0080_0080u) >> 2;
		return value ^ mask;
	}

	/// <summary>
	/// Given a UInt32 that represents two ASCII UTF-16 characters, returns the invariant
	/// uppercase representation of those characters. Requires the input value to contain
	/// two ASCII UTF-16 characters in machine endianness.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static UInt32 ConvertAllAsciiCharsInUInt32ToUppercase(UInt32 value)
	{
		UInt32 lowerIndicator = value + 0x0080_0080u - 0x0061_0061u;
		UInt32 upperIndicator = value + 0x0080_0080u - 0x007B_007Bu;
		UInt32 combinedIndicator = lowerIndicator ^ upperIndicator;
		UInt32 mask = (combinedIndicator & 0x0080_0080u) >> 2;
		return value ^ mask;
	}

	/// <summary>
	/// Given a UInt64 that represents four ASCII UTF-16 characters, returns the invariant
	/// uppercase representation of those characters. Requires the input value to contain
	/// four ASCII UTF-16 characters in machine endianness.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static UInt64 ConvertAllAsciiCharsInUInt64ToUppercase(UInt64 value)
	{
		UInt64 lowerIndicator = value + 0x0080_0080_0080_0080ul - 0x0061_0061_0061_0061ul;
		UInt64 upperIndicator = value + 0x0080_0080_0080_0080ul - 0x007B_007B_007B_007Bul;
		UInt64 combinedIndicator = lowerIndicator ^ upperIndicator;
		UInt64 mask = (combinedIndicator & 0x0080_0080_0080_0080ul) >> 2;
		return value ^ mask;
	}

	/// <summary>
	/// Given a UInt64 that represents four ASCII UTF-16 characters, returns the invariant
	/// lowercase representation of those characters. Requires the input value to contain
	/// four ASCII UTF-16 characters in machine endianness.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static UInt64 ConvertAllAsciiCharsInUInt64ToLowercase(UInt64 value)
	{
		UInt64 lowerIndicator = value + 0x0080_0080_0080_0080ul - 0x0041_0041_0041_0041ul;
		UInt64 upperIndicator = value + 0x0080_0080_0080_0080ul - 0x005B_005B_005B_005Bul;
		UInt64 combinedIndicator = lowerIndicator ^ upperIndicator;
		UInt64 mask = (combinedIndicator & 0x0080_0080_0080_0080ul) >> 2;
		return value ^ mask;
	}

	/// <summary>
	/// Given a UInt32 that represents two ASCII UTF-16 characters, returns true iff
	/// the input contains one or more lowercase ASCII characters.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean UInt32ContainsAnyLowercaseAsciiChar(UInt32 value)
	{
		UInt32 lowerIndicator = value + 0x0080_0080u - 0x0061_0061u;
		UInt32 upperIndicator = value + 0x0080_0080u - 0x007B_007Bu;
		UInt32 combinedIndicator = lowerIndicator ^ upperIndicator;
		return (combinedIndicator & 0x0080_0080u) != 0;
	}

	/// <summary>
	/// Given a UInt32 that represents two ASCII UTF-16 characters, returns true iff
	/// the input contains one or more uppercase ASCII characters.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean UInt32ContainsAnyUppercaseAsciiChar(UInt32 value)
	{
		UInt32 lowerIndicator = value + 0x0080_0080u - 0x0041_0041u;
		UInt32 upperIndicator = value + 0x0080_0080u - 0x005B_005Bu;
		UInt32 combinedIndicator = lowerIndicator ^ upperIndicator;
		return (combinedIndicator & 0x0080_0080u) != 0;
	}

	/// <summary>
	/// Given two UInt32s that represent two ASCII UTF-16 characters each, returns true iff
	/// the two inputs are equal using an ordinal case-insensitive comparison.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean UInt32OrdinalIgnoreCaseAscii(UInt32 valueA, UInt32 valueB)
	{
		UInt32 differentBits = (valueA ^ valueB) << 2;
		UInt32 indicator = valueA + 0x0005_0005u;
		indicator |= 0x00A0_00A0u;
		indicator += 0x001A_001Au;
		indicator |= 0xFF7F_FF7Fu;
		return (differentBits & indicator) == 0;
	}

	/// <summary>
	/// Given two UInt64s that represent four ASCII UTF-16 characters each, returns true iff
	/// the two inputs are equal using an ordinal case-insensitive comparison.
	/// </summary>
	/// <remarks>
	/// This is a branchless implementation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean UInt64OrdinalIgnoreCaseAscii(UInt64 valueA, UInt64 valueB)
	{
		UInt64 differentBits = (valueA ^ valueB) << 2;
		UInt64 indicator = valueA + 0x0005_0005_0005_0005ul;
		indicator |= 0x00A0_00A0_00A0_00A0ul;
		indicator += 0x001A_001A_001A_001Aul;
		indicator |= 0xFF7F_FF7F_FF7F_FF7Ful;
		return (differentBits & indicator) == 0;
	}
}
#endif