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
// (System.Text.UnicodeUtility)

#if !NETCOREAPP
namespace System.Text;

/// <summary>
/// Unicode text utilities.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal static class UnicodeUtility
{
	/// <summary>
	/// The Unicode replacement character U+FFFD.
	/// </summary>
	public const UInt32 ReplacementChar = 0xFFFD;

	/// <summary>
	/// Returns the Unicode plane (0 through 16, inclusive) which contains this code point.
	/// </summary>
	public static Int32 GetPlane(UInt32 codePoint) => (Int32)(codePoint >> 16);

	/// <summary>
	/// Returns a Unicode scalar value from two code points representing a UTF-16 surrogate pair.
	/// </summary>
	public static UInt32 GetScalarFromUtf16SurrogatePair(UInt32 highSurrogateCodePoint, UInt32 lowSurrogateCodePoint)
		=> (highSurrogateCodePoint << 10) + lowSurrogateCodePoint - ((0xD800U << 10) + 0xDC00U - (1 << 16));

	/// <summary>
	/// Given a Unicode scalar value, gets the number of UTF-16 code units required to represent this value.
	/// </summary>
	public static Int32 GetUtf16SequenceLength(UInt32 value)
	{
		value -= 0x10000;
		value += 2 << 24;
		value >>= 24;
		return (Int32)value;
	}

	/// <summary>
	/// Decomposes an astral Unicode scalar into UTF-16 high and low surrogate code units.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void GetUtf16SurrogatesFromSupplementaryPlaneScalar(UInt32 value, out Char highSurrogateCodePoint,
		out Char lowSurrogateCodePoint)
	{
		highSurrogateCodePoint = (Char)((value + ((0xD800u - 0x40u) << 10)) >> 10);
		lowSurrogateCodePoint = (Char)((value & 0x3FFu) + 0xDC00u);
	}

	/// <summary>
	/// Given a Unicode scalar value, gets the number of UTF-8 code units required to represent this value.
	/// </summary>
	public static Int32 GetUtf8SequenceLength(UInt32 value)
	{
		Int32 a = ((Int32)value - 0x0800) >> 31;
		value ^= 0xF800u;
		value -= 0xF880u;
		value += 4 << 24;
		value >>= 24;
		return (Int32)value + a * 2;
	}

	/// <summary>
	/// Returns <see langword="true"/> iff <paramref name="value"/> is an ASCII
	/// character ([ U+0000..U+007F ]).
	/// </summary>
	/// <remarks>
	/// Per http://www.unicode.org/glossary/#ASCII, ASCII is only U+0000..U+007F.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsAsciiCodePoint(UInt32 value) => value <= 0x7Fu;

	/// <summary>
	/// Returns <see langword="true"/> iff <paramref name="value"/> is in the
	/// Basic Multilingual Plane (BMP).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsBmpCodePoint(UInt32 value) => value <= 0xFFFFu;

	/// <summary>
	/// Returns <see langword="true"/> iff <paramref name="value"/> is a UTF-16 high surrogate code point,
	/// i.e., is in [ U+D800..U+DBFF ], inclusive.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsHighSurrogateCodePoint(UInt32 value)
		=> UnicodeUtility.IsInRangeInclusive(value, 0xD800U, 0xDBFFU);

	/// <summary>
	/// Returns <see langword="true"/> iff <paramref name="value"/> is between
	/// <paramref name="lowerBound"/> and <paramref name="upperBound"/>, inclusive.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsInRangeInclusive(UInt32 value, UInt32 lowerBound, UInt32 upperBound)
		=> value - lowerBound <= upperBound - lowerBound;

	/// <summary>
	/// Returns <see langword="true"/> iff <paramref name="value"/> is a UTF-16 low surrogate code point,
	/// i.e., is in [ U+DC00..U+DFFF ], inclusive.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsLowSurrogateCodePoint(UInt32 value)
		=> UnicodeUtility.IsInRangeInclusive(value, 0xDC00U, 0xDFFFU);

	/// <summary>
	/// Returns <see langword="true"/> iff <paramref name="value"/> is a UTF-16 surrogate code point,
	/// i.e., is in [ U+D800..U+DFFF ], inclusive.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsSurrogateCodePoint(UInt32 value)
		=> UnicodeUtility.IsInRangeInclusive(value, 0xD800U, 0xDFFFU);

	/// <summary>
	/// Returns <see langword="true"/> iff <paramref name="codePoint"/> is a valid Unicode code
	/// point, i.e., is in [ U+0000..U+10FFFF ], inclusive.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsValidCodePoint(UInt32 codePoint) => codePoint <= 0x10FFFFU;

	/// <summary>
	/// Returns <see langword="true"/> iff <paramref name="value"/> is a valid Unicode scalar
	/// value, i.e., is in [ U+0000..U+D7FF ], inclusive; or [ U+E000..U+10FFFF ], inclusive.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsValidUnicodeScalar(UInt32 value) => ((value - 0x110000u) ^ 0xD800u) >= 0xFFEF0800u;
}
#endif