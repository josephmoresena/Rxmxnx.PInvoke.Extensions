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

// Adopted and adapted by Joseph Moreno in 2026 based on code from .NET 10.0 CoreCLR
// (System.Marvin / System.String )

#if !NET7_0_OR_GREATER && NET5_0_OR_GREATER
// ReSharper disable once BuiltInTypeReferenceStyle
using UIntPtr = nuint;

#elif !NETCOREAPP3_1_OR_GREATER
// ReSharper disable once BuiltInTypeReferenceStyle
using IntPtr = nint;

#endif

namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// Marvin hash class.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS907)]
#endif
internal static class MarvinCompat
{
	/// <summary>
	/// Default Marvin seed.
	/// </summary>
	public static readonly UInt64? DefaultSeed;

	/// <summary>
	/// Static constructor.
	/// </summary>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
	[UnconditionalSuppressMessage("Trimming", "IL2075")]
	static MarvinCompat()
	{
		if (TrimInfo.SafeGetType(typeof(String), "System.Marvin") is not { } marvinType) return;
		if (marvinType.GetProperty("DefaultSeed") is not { } defaultSeedProp) return;
		MarvinCompat.DefaultSeed = Convert.ToUInt64(defaultSeedProp.GetValue(default));
	}

	/// <summary>
	/// Returns the hash code for the provided read-only UTF-8 unit span.
	/// </summary>
	/// <param name="value">A read-only UTF-8 unit span.</param>
	/// <returns>A 32-bit signed integer hash code.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Int32 GetHashCode(ReadOnlySpan<Byte> value)
	{
#if !PACKAGE
		const Int32 maxChars = 16;
#else
		const Int32 maxChars = StackAllocationHelper.StackallocByteThreshold / 2;
#endif
		if (Encoding.UTF8.GetMaxCharCount(value.Length) <= maxChars)
		{
			Span<Char> chars = stackalloc Char[maxChars];
#if !NETCOREAPP
			Int32 charCount = Encoding.UTF8.GetChars(value, chars);
			return MarvinCompat.GetHashCode(chars[..charCount]);
#else
			Utf8.ToUtf16(value, chars, out _, out Int32 charCount);
			return String.GetHashCode(chars[..charCount]);
#endif
		}

		unchecked
		{
			UInt32 seed0 = (UInt32)MarvinCompat.DefaultSeed!.Value;
			UInt32 seed1 = (UInt32)(MarvinCompat.DefaultSeed.Value >> 32);
			return MarvinCompat.ComputeUtf8Hash32(value, seed0, seed1);
		}
	}

#if !PACKAGE || !NETCOREAPP
	/// <summary>
	/// Returns the hash code for the provided read-only character span.
	/// </summary>
	/// <param name="value">A read-only character span.</param>
	/// <returns>A 32-bit signed integer hash code.</returns>
#if !PACKAGE
	public static Int32 GetHashCode(ReadOnlySpan<Char> value)
#else
	private static Int32 GetHashCode(ReadOnlySpan<Char> value)
#endif
	{
		unchecked
		{
			ref Byte refData0 = ref Unsafe.As<Char, Byte>(ref MemoryMarshal.GetReference(value));
			UInt32 dataLength = (UInt32)value.Length * 2;
			UInt32 seed0 = (UInt32)MarvinCompat.DefaultSeed!.Value;
			UInt32 seed1 = (UInt32)(MarvinCompat.DefaultSeed.Value >> 32);
			return MarvinCompat.ComputeUtf16Hash32(ref refData0, dataLength, seed0, seed1);
		}
	}
	/// <summary>
	/// Compute a Marvin hash and collapse it into a 32-bit hash.
	/// </summary>
	/// <returns>A 32-bit signed integer hash code.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.NoInlining)]
	private static Int32 ComputeUtf16Hash32(ref Byte utf16Data, UInt32 count, UInt32 p0, UInt32 p1)
	{
		if (count < 8)
		{
			if (count >= 4)
				goto Between4And7BytesRemain;
			goto InputTooSmallToEnterMainLoop;
		}

		UInt32 loopCount = count / 8;

		do
		{
			p0 += Unsafe.ReadUnaligned<UInt32>(ref utf16Data);
#if !NETCOREAPP3_1_OR_GREATER
			UInt32 nextUInt32 = Unsafe.ReadUnaligned<UInt32>(ref Unsafe.AddByteOffset(ref utf16Data, (IntPtr)4));
#else
			UInt32 nextUInt32 = Unsafe.ReadUnaligned<UInt32>(ref Unsafe.AddByteOffset(ref utf16Data, 4));
#endif
			MarvinCompat.Block(ref p0, ref p1);
			p0 += nextUInt32;
			MarvinCompat.Block(ref p0, ref p1);
#if !NETCOREAPP3_1_OR_GREATER
			utf16Data = ref Unsafe.AddByteOffset(ref utf16Data, (IntPtr)8);
#else
			utf16Data = ref Unsafe.AddByteOffset(ref utf16Data, 8);
#endif
		} while (--loopCount > 0);

		if ((count & 0b_0100) == 0) goto DoFinalPartialRead;

		Between4And7BytesRemain:

		p0 += Unsafe.ReadUnaligned<UInt32>(ref utf16Data);
		MarvinCompat.Block(ref p0, ref p1);

		DoFinalPartialRead:

#if !NETCOREAPP3_1_OR_GREATER
		UInt32 partialResult;
		unchecked
		{
			partialResult =
				Unsafe.ReadUnaligned<UInt32>(
					ref Unsafe.Add(ref Unsafe.AddByteOffset(ref utf16Data, (IntPtr)(count & 7)), -4));
		}
#else
#if !NET5_0_OR_GREATER
		UIntPtr byteOffset = (UIntPtr)(count & 7);
#else
		UIntPtr byteOffset = count & 7;
#endif
		UInt32 partialResult =
			Unsafe.ReadUnaligned<UInt32>(ref Unsafe.Add(ref Unsafe.AddByteOffset(ref utf16Data, byteOffset), -4));
#endif
		count = ~count << 3;
		if (BitConverter.IsLittleEndian)
		{
			partialResult >>= 8;
			partialResult |= 0x8000_0000u;
			partialResult >>= (Int32)count & 0x1F;
		}
		else
		{
			partialResult <<= 8;
			partialResult |= 0x80u;
			partialResult <<= (Int32)count & 0x1F;
		}
		DoFinalRoundsAndReturn:

		p0 += partialResult;
		MarvinCompat.Block(ref p0, ref p1);
		MarvinCompat.Block(ref p0, ref p1);

		return (Int32)(p1 ^ p0);

		InputTooSmallToEnterMainLoop:

		partialResult = BitConverter.IsLittleEndian ? 0x80u : 0x80000000u;

		if ((count & 0b_0001) != 0)
		{
#if !NETCOREAPP3_1_OR_GREATER
			partialResult = Unsafe.AddByteOffset(ref utf16Data, (IntPtr)(count & 2));
#else
#if !NET5_0_OR_GREATER
			byteOffset = (UIntPtr)(count & 2);
#else
			byteOffset = count & 2;
#endif
			partialResult = Unsafe.AddByteOffset(ref utf16Data, byteOffset);
#endif

			if (BitConverter.IsLittleEndian)
			{
				partialResult |= 0x8000;
			}
			else
			{
				partialResult <<= 24;
				partialResult |= 0x800000u;
			}
		}

		if ((count & 0b_0010) != 0)
		{
			if (BitConverter.IsLittleEndian)
			{
				partialResult <<= 16;
				partialResult |= Unsafe.ReadUnaligned<UInt16>(ref utf16Data);
			}
			else
			{
				partialResult |= Unsafe.ReadUnaligned<UInt16>(ref utf16Data);
				partialResult = MarvinCompat.RotateLeft(partialResult, 16);
			}
		}
		goto DoFinalRoundsAndReturn;
	}
#endif
#if !NETCOREAPP
	/// <summary>
	/// Determines how many UTF-8 bytes can be decoded into the available UTF-16 space,
	/// gracefully handling invalid or incomplete sequences (Maximal Subpart Rule).
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void CountChunk(ReadOnlySpan<Byte> source, Int32 availableChars, out Int32 bytesConsumed,
		out Int32 charsWritten)
	{
		bytesConsumed = 0;
		charsWritten = 0;

		while (bytesConsumed < source.Length && charsWritten < availableChars)
		{
			Byte b1 = source[bytesConsumed];
			// 1. FAST-PATH para ASCII (1 byte -> 1 char)
			if (b1 < 0x80)
			{
				bytesConsumed++;
				charsWritten++;
				continue;
			}

			Int32 sequenceLength = 0;
			Int32 utf16Chars = 1;
			Boolean isValid = false;

			switch (b1)
			{
				case >= 0xC2 and <= 0xDF:
				{
					if (bytesConsumed + 1 < source.Length && MarvinCompat.IsTrailByte(source[bytesConsumed + 1]))
					{
						sequenceLength = 2;
						isValid = true;
					}
					break;
				}
				case >= 0xE0 and <= 0xEF:
				{
					if (bytesConsumed + 2 < source.Length)
					{
						Byte b2 = source[bytesConsumed + 1];
						Byte b3 = source[bytesConsumed + 2];
						if (MarvinCompat.IsTrailByte(b3) && ((b1 == 0xE0 && b2 is >= 0xA0 and <= 0xBF) ||
							    (b1 is >= 0xE1 and <= 0xEC && MarvinCompat.IsTrailByte(b2)) ||
							    (b1 == 0xED && b2 is >= 0x80 and <= 0x9F) ||
							    (b1 is >= 0xEE and <= 0xEF && MarvinCompat.IsTrailByte(b2))))
						{
							sequenceLength = 3;
							isValid = true;
						}
					}
					break;
				}
				case >= 0xF0 and <= 0xF4:
				{
					if (bytesConsumed + 3 < source.Length)
					{
						Byte b2 = source[bytesConsumed + 1];
						Byte b3 = source[bytesConsumed + 2];
						Byte b4 = source[bytesConsumed + 3];

						if (MarvinCompat.IsTrailByte(b3) && MarvinCompat.IsTrailByte(b4) &&
						    ((b1 == 0xF0 && b2 is >= 0x90 and <= 0xBF) ||
							    (b1 is >= 0xF1 and <= 0xF3 && MarvinCompat.IsTrailByte(b2)) ||
							    (b1 == 0xF4 && b2 is >= 0x80 and <= 0x8F)))
						{
							sequenceLength = 4;
							utf16Chars = 2;
							isValid = true;
						}
					}
					break;
				}
			}

			if (isValid)
			{
				if (charsWritten + utf16Chars > availableChars)
					break;

				bytesConsumed += sequenceLength;
				charsWritten += utf16Chars;
			}
			else
			{
				if (charsWritten + 1 > availableChars)
					break;

				Int32 invalidBytesToConsume = 1;
				if (b1 is >= 0xC2 and <= 0xF4 && bytesConsumed + 1 < source.Length)
				{
					Byte b2 = source[bytesConsumed + 1];

					switch (b1)
					{
						case >= 0xE0 and <= 0xEF:
						{
							if ((b1 == 0xE0 && b2 is >= 0xA0 and <= 0xBF) ||
							    (b1 is >= 0xE1 and <= 0xEC && MarvinCompat.IsTrailByte(b2)) ||
							    (b1 == 0xED && b2 is >= 0x80 and <= 0x9F) ||
							    (b1 is >= 0xEE and <= 0xEF && MarvinCompat.IsTrailByte(b2)))
								invalidBytesToConsume = 2;
							break;
						}
						case >= 0xF0 and <= 0xF4:
						{
							if ((b1 == 0xF0 && b2 is >= 0x90 and <= 0xBF) ||
							    (b1 is >= 0xF1 and <= 0xF3 && MarvinCompat.IsTrailByte(b2)) ||
							    (b1 == 0xF4 && b2 is >= 0x80 and <= 0x8F))
							{
								invalidBytesToConsume = 2;
								if (bytesConsumed + 2 < source.Length &&
								    MarvinCompat.IsTrailByte(source[bytesConsumed + 2]))
									invalidBytesToConsume = 3;
							}
							break;
						}
					}
				}
				bytesConsumed += invalidBytesToConsume;
				charsWritten += 1;
			}
		}
	}
	/// <summary>
	/// Indicates whether <paramref name="b"/> is a UTF-8 trail byte.
	/// </summary>
	/// <param name="b">A UTF-8 unit.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="b"/> is a UTF-8 trail byte; otherwise <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsTrailByte(Byte b) => (b & 0xC0) == 0x80;
#endif
	/// <summary>
	/// Compute a Marvin hash and collapse it into a 32-bit hash.
	/// </summary>
	/// <returns>A 32-bit signed integer hash code.</returns>
	private static Int32 ComputeUtf8Hash32(ReadOnlySpan<Byte> value, UInt32 p0, UInt32 p1)
	{
		UInt32 count = 0;
		Int32 carryOffset = 0;
		Span<Char> buffer = stackalloc Char[5];
		ref Byte utf16Data = ref Unsafe.As<Char, Byte>(ref MemoryMarshal.GetReference(buffer));
		while (true)
		{
			Span<Char> destination = buffer.Slice(carryOffset, 5 - carryOffset);
#if !NETCOREAPP
			MarvinCompat.CountChunk(value, destination.Length, out Int32 bytesConsumed, out Int32 charsWritten);
			Encoding.UTF8.GetChars(value[..bytesConsumed], destination);
#else
			Utf8.ToUtf16(value, destination, out Int32 bytesConsumed, out Int32 charsWritten);
#endif
			UInt32 consumption = (UInt32)(2 * charsWritten);
			value = value[bytesConsumed..];
			count += consumption;

			if (consumption + 2 * carryOffset < 8) break;
			p0 += Unsafe.ReadUnaligned<UInt32>(ref utf16Data);
#if !NETCOREAPP3_1_OR_GREATER
			UInt32 nextUInt32 = Unsafe.ReadUnaligned<UInt32>(ref Unsafe.AddByteOffset(ref utf16Data, (IntPtr)4));
#else
			UInt32 nextUInt32 = Unsafe.ReadUnaligned<UInt32>(ref Unsafe.AddByteOffset(ref utf16Data, 4));
#endif
			MarvinCompat.Block(ref p0, ref p1);
			p0 += nextUInt32;
			MarvinCompat.Block(ref p0, ref p1);
			if (charsWritten != destination.Length)
			{
				carryOffset = 0;
				continue;
			}
			buffer[0] = buffer[^1];
			carryOffset = 1;
		}

		if ((count & 0b_0100) == 0) goto DoFinalPartialRead;

		p0 += Unsafe.ReadUnaligned<UInt32>(ref utf16Data);
		MarvinCompat.Block(ref p0, ref p1);

		DoFinalPartialRead:
#if !NETCOREAPP3_1_OR_GREATER
		UInt32 partialResult;
		unchecked
		{
			partialResult =
				Unsafe.ReadUnaligned<UInt32>(
					ref Unsafe.Add(ref Unsafe.AddByteOffset(ref utf16Data, (IntPtr)(count & 7)), -4));
		}
#else
#if !NET5_0_OR_GREATER
		UIntPtr byteOffset = (UIntPtr)(count & 7);
#else
		UIntPtr byteOffset = count & 7;
#endif
		UInt32 partialResult =
			Unsafe.ReadUnaligned<UInt32>(ref Unsafe.Add(ref Unsafe.AddByteOffset(ref utf16Data, byteOffset), -4));
#endif
		count = ~count << 3;
		if (BitConverter.IsLittleEndian)
		{
			partialResult >>= 8;
			partialResult |= 0x8000_0000u;
			partialResult >>= (Int32)count & 0x1F;
		}
		else
		{
			partialResult <<= 8;
			partialResult |= 0x80u;
			partialResult <<= (Int32)count & 0x1F;
		}

		p0 += partialResult;
		MarvinCompat.Block(ref p0, ref p1);
		MarvinCompat.Block(ref p0, ref p1);

		return (Int32)(p1 ^ p0);
	}
	/// <summary>
	/// Executes a single Marvin hash mixing round on the specified state.
	/// </summary>
	/// <param name="rp0">The first state value.</param>
	/// <param name="rp1">The second state value.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void Block(ref UInt32 rp0, ref UInt32 rp1)
	{
		UInt32 p0 = rp0;
		UInt32 p1 = rp1;

		p1 ^= p0;
		p0 = MarvinCompat.RotateLeft(p0, 20);

		p0 += p1;
		p1 = MarvinCompat.RotateLeft(p1, 9);

		p1 ^= p0;
		p0 = MarvinCompat.RotateLeft(p0, 27);

		p0 += p1;
		p1 = MarvinCompat.RotateLeft(p1, 19);

		rp0 = p0;
		rp1 = p1;
	}
	/// <summary>
	/// Rotates the bits of a 32-bit unsigned integer to the left.
	/// </summary>
	/// <param name="value">The value whose bits are rotated.</param>
	/// <param name="shift">The number of bit positions to rotate.</param>
	/// <returns>The value resulting from the left bit rotation.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UInt32 RotateLeft(UInt32 value, Int32 shift) => (value << shift) | (value >> (32 - shift));
}