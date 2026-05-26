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
// (System.Numerics.BitOperations)

#if !NETCOREAPP
namespace System.Numerics;

/// <summary>
/// Utility methods for intrinsic bit-twiddling operations.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal static class BitOperations
{
	private static ReadOnlySpan<Byte> TrailingZeroCountDeBruijn
		=> // 32
		[
			00, 01, 28, 02, 29, 14, 24, 03,
			30, 22, 20, 15, 25, 17, 04, 08,
			31, 27, 13, 23, 21, 19, 16, 07,
			26, 12, 18, 06, 11, 05, 10, 09,
		];

	/// <summary>
	/// Count the number of trailing zero bits in an integer value.
	/// Similar in behavior to the x86 instruction TZCNT.
	/// </summary>
	/// <param name="value">The value.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Int32 TrailingZeroCount(UInt32 value)
	{
		if (value == 0)
			return 32;

		return Unsafe.AddByteOffset(ref MemoryMarshal.GetReference(BitOperations.TrailingZeroCountDeBruijn),
		                            (IntPtr)(Int32)(((value & (UInt32)(-(Int32)value)) * 0x077CB531u) >> 27));
	}

	/// <summary>
	/// Rotates the specified value left by the specified number of bits.
	/// Similar in behavior to the x86 instruction ROL.
	/// </summary>
	/// <param name="value">The value to rotate.</param>
	/// <param name="offset">
	/// The number of bits to rotate by.
	/// Any value outside the range [0..31] is treated as congruent mod 32.
	/// </param>
	/// <returns>The rotated value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UInt32 RotateLeft(UInt32 value, Int32 offset) => (value << offset) | (value >> (32 - offset));

	/// <summary>
	/// Rotates the specified value right by the specified number of bits.
	/// Similar in behavior to the x86 instruction ROR.
	/// </summary>
	/// <param name="value">The value to rotate.</param>
	/// <param name="offset">
	/// The number of bits to rotate by.
	/// Any value outside the range [0..31] is treated as congruent mod 32.
	/// </param>
	/// <returns>The rotated value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UInt32 RotateRight(UInt32 value, Int32 offset) => (value >> offset) | (value << (32 - offset));
}
#endif