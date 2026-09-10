// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

// Adopted and adapted by Joseph Moreno in 2025 based on code from .NET 6.0 CoreCLR
// (System.Convert / System.HexConverter )

namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// <see cref="Convert"/> compatibility utilities for internal use.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static class ConvertCompat
{
	/// <summary>
	/// Converts a span of 8-bit unsigned integers to its equivalent string representation that is
	/// encoded with uppercase hex characters.
	/// </summary>
	/// <param name="bytes">A span of 8-bit unsigned integers.</param>
	/// <returns>The string representation in hex of the elements in <paramref name="bytes"/>.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	public static unsafe String ToHexString(ReadOnlySpan<Byte> bytes)
	{
#if !PACKAGE || !NET5_0_OR_GREATER
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		fixed (Byte* bytesPtr = &MemoryMarshal.GetReference(bytes))
		{
			return String.Create(bytes.Length * 2, (Ptr: (IntPtr)bytesPtr, bytes.Length), static (chars, args) =>
			{
				ReadOnlySpan<Byte> source = new((Byte*)args.Ptr, args.Length);
				for (Int32 pos = 0; pos < args.Length; ++pos)
					ConvertCompat.ToCharsBuffer(source[pos], chars, pos * 2);
			});
		}
#else
		Int32 charCount = bytes.Length * 2;
		Span<Char> chars = charCount <= StackAllocationHelper.StackallocByteThreshold ?
			stackalloc Char[charCount] :
			new Char[charCount];
		fixed (Byte* _ = &MemoryMarshal.GetReference(bytes))
		{
			for (Int32 pos = 0; pos < bytes.Length; ++pos)
				ConvertCompat.ToCharsBuffer(bytes[pos], chars, pos * 2);
		}
		return chars.ToString();
#endif
#else
		return Convert.ToHexString(bytes);
#endif
	}

	#region CORECLR
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void ToCharsBuffer(Byte value, Span<Char> buffer, Int32 startingIndex = 0)
	{
		UInt32 difference = ((value & 0xF0U) << 4) + (value & 0x0FU) - 0x8989U;
		UInt32 packedResult = (((UInt32)(-(Int32)difference) & 0x7070U) >> 4) + difference + 0xB9B9U;

		buffer[startingIndex + 1] = (Char)(packedResult & 0xFF);
		buffer[startingIndex] = (Char)(packedResult >> 8);
	}
	#endregion
}