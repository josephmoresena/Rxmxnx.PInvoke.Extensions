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
// (System.Runtime.InteropServices.MemoryMarshal / System.String /  System.SpanHelpers )

#if NETCOREAPP && (!PACKAGE || !NET6_0_OR_GREATER)
using UIntPtr = nuint;
using IntPtr = nint;

namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1199)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS907)]
#endif
internal static unsafe partial class MemoryMarshalCompat
{
#pragma warning disable CA2020
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	private static Int32 IndexOfNull(ref Byte buffer)
	{
		const Byte value = (Byte)'\0';
		const UInt32 uValue = value;
		UIntPtr offset = 0;
		UIntPtr lengthToExamine = Int32.MaxValue;

		if (Sse2.IsSupported || AdvSimd.Arm64.IsSupported)
		{
			if (Int32.MaxValue >= (UInt32)Vector128<Byte>.Count * 2)
				lengthToExamine = MemoryMarshalCompat.UnalignedCountVector128(ref buffer);
		}
		else if (Vector.IsHardwareAccelerated && Int32.MaxValue >= (UInt32)Vector<Byte>.Count * 2)
		{
			lengthToExamine = MemoryMarshalCompat.UnalignedCountVector(ref buffer);
		}
		SequentialScan:
		while (lengthToExamine >= 8)
		{
			lengthToExamine -= 8;

			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset)))
				goto Found;
			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset + 1)))
				goto Found1;
			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset + 2)))
				goto Found2;
			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset + 3)))
				goto Found3;
			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset + 4)))
				goto Found4;
			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset + 5)))
				goto Found5;
			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset + 6)))
				goto Found6;
			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset + 7)))
				goto Found7;

			offset += 8;
		}

		if (lengthToExamine >= 4)
		{
			lengthToExamine -= 4;

			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset)))
				goto Found;
			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset + 1)))
				goto Found1;
			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset + 2)))
				goto Found2;
			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset + 3)))
				goto Found3;

			offset += 4;
		}

		while (lengthToExamine > 0)
		{
			lengthToExamine -= 1;

			if (uValue == Unsafe.AddByteOffset(ref buffer, MemoryMarshalCompat.ToByteOffset(offset)))
				goto Found;

			offset += 1;
		}

		if (Avx2.IsSupported)
		{
			if (offset >= Int32.MaxValue) return -1;

			if ((((UInt32)Unsafe.AsPointer(ref buffer) + offset) & (UIntPtr)(Vector256<Byte>.Count - 1)) != 0)
			{
				Vector128<Byte> values = Vector128.Create(value);
				Vector128<Byte> search = MemoryMarshalCompat.LoadVector128(ref buffer, offset);

				Int32 matches = Sse2.MoveMask(Sse2.CompareEqual(values, search));
				if (matches == 0)
					offset += (UIntPtr)Vector128<Byte>.Count;
				else
					return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches));
			}

			lengthToExamine = MemoryMarshalCompat.GetByteVector256SpanLength(offset);
			if (lengthToExamine > offset)
			{
				Vector256<Byte> values = Vector256.Create(value);
				do
				{
					Vector256<Byte> search = MemoryMarshalCompat.LoadVector256(ref buffer, offset);
					Int32 matches = Avx2.MoveMask(Avx2.CompareEqual(values, search));

					if (matches != 0) return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches));

					offset += (UIntPtr)Vector256<Byte>.Count;
				} while (lengthToExamine > offset);
			}

			lengthToExamine = MemoryMarshalCompat.GetByteVector128SpanLength(offset);
			if (lengthToExamine > offset)
			{
				Vector128<Byte> values = Vector128.Create(value);
				Vector128<Byte> search = MemoryMarshalCompat.LoadVector128(ref buffer, offset);

				Int32 matches = Sse2.MoveMask(Sse2.CompareEqual(values, search));
				if (matches == 0)
					offset += (UIntPtr)Vector128<Byte>.Count;
				else
					return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches));
			}

			if (offset >= Int32.MaxValue) return -1;

			lengthToExamine = Int32.MaxValue - offset;
			goto SequentialScan;
		}
		if (Sse2.IsSupported)
		{
			if (offset >= Int32.MaxValue) return -1;

			lengthToExamine = MemoryMarshalCompat.GetByteVector128SpanLength(offset);

			Vector128<Byte> values = Vector128.Create(value);
			while (lengthToExamine > offset)
			{
				Vector128<Byte> search = MemoryMarshalCompat.LoadVector128(ref buffer, offset);

				// Same method as above
				Int32 matches = Sse2.MoveMask(Sse2.CompareEqual(values, search));
				if (matches != 0) return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches));

				offset += (UIntPtr)Vector128<Byte>.Count;
			}

			if (offset >= Int32.MaxValue) return -1;
			lengthToExamine = Int32.MaxValue - offset;
			goto SequentialScan;
		}
		if (AdvSimd.Arm64.IsSupported)
		{
			if (offset >= Int32.MaxValue) return -1;

			lengthToExamine = MemoryMarshalCompat.GetByteVector128SpanLength(offset);

			Vector128<Byte> mask = Vector128.Create((UInt16)0x1001).AsByte();
			Int32 matchedLane = 0;

			Vector128<Byte> values = Vector128.Create(value);
			while (lengthToExamine > offset)
			{
				Vector128<Byte> search = MemoryMarshalCompat.LoadVector128(ref buffer, offset);
				Vector128<Byte> compareResult = AdvSimd.CompareEqual(values, search);

				if (MemoryMarshalCompat.TryFindFirstMatchedLane(mask, compareResult, ref matchedLane))
					return (Int32)(offset + (UInt32)matchedLane);

				offset += (UIntPtr)Vector128<Byte>.Count;
			}

			if (offset >= Int32.MaxValue) return -1;
			lengthToExamine = Int32.MaxValue - offset;
			goto SequentialScan;
		}
		if (!Vector.IsHardwareAccelerated) return -1;
		{
			if (offset >= Int32.MaxValue) return -1;

			lengthToExamine = MemoryMarshalCompat.GetByteVectorSpanLength(offset, Int32.MaxValue);

			Vector<Byte> values = new(value);

			while (lengthToExamine > offset)
			{
				Vector<Byte> matches = Vector.Equals(values, MemoryMarshalCompat.LoadVector(ref buffer, offset));
				if (!Vector<Byte>.Zero.Equals(matches))
					return (Int32)offset + MemoryMarshalCompat.LocateFirstFoundByte(matches);

				offset += (UIntPtr)Vector<Byte>.Count;
			}

			if (offset >= Int32.MaxValue) return -1;

			lengthToExamine = Int32.MaxValue - offset;
			goto SequentialScan;
		}
		Found:
		return (Int32)offset;
		Found1:
		return (Int32)(offset + 1);
		Found2:
		return (Int32)(offset + 2);
		Found3:
		return (Int32)(offset + 3);
		Found4:
		return (Int32)(offset + 4);
		Found5:
		return (Int32)(offset + 5);
		Found6:
		return (Int32)(offset + 6);
		Found7:
		return (Int32)(offset + 7);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	private static Int32 IndexOfNull(ref Char searchSpace)
	{
		const UInt16 value = '\0';
		IntPtr offset = 0;
		IntPtr lengthToExamine = Int32.MaxValue;

		if (((Int32)Unsafe.AsPointer(ref searchSpace) & 1) != 0)
		{
			// Input isn't char aligned, we won't be able to align it to a Vector
		}
		else if (Sse2.IsSupported || AdvSimd.Arm64.IsSupported)
		{
			if (Int32.MaxValue >= (UInt32)Vector128<UInt16>.Count * 2)
				lengthToExamine = MemoryMarshalCompat.UnalignedCountVector128(ref searchSpace);
		}
		else if (Vector.IsHardwareAccelerated && Int32.MaxValue >= (UInt32)Vector<UInt16>.Count * 2)
		{
			lengthToExamine = MemoryMarshalCompat.UnalignedCountVector(ref searchSpace);
		}

		SequentialScan:
		while (lengthToExamine >= 4)
		{
			ref Char current = ref Unsafe.Add(ref searchSpace, offset);

			if (value == current)
				goto Found;
			if (value == Unsafe.Add(ref current, 1))
				goto Found1;
			if (value == Unsafe.Add(ref current, 2))
				goto Found2;
			if (value == Unsafe.Add(ref current, 3))
				goto Found3;

			offset += 4;
			lengthToExamine -= 4;
		}

		while (lengthToExamine > 0)
		{
			if (value == Unsafe.Add(ref searchSpace, offset))
				goto Found;

			offset++;
			lengthToExamine--;
		}

		if (Avx2.IsSupported)
		{
			if (offset >= Int32.MaxValue) return -1;

			if (((IntPtr)Unsafe.AsPointer(ref Unsafe.Add(ref searchSpace, offset)) & (Vector256<Byte>.Count - 1)) != 0)
			{
				Vector128<UInt16> values = Vector128.Create(value);
				Vector128<UInt16> search = MemoryMarshalCompat.LoadVector128(ref searchSpace, offset);
				Int32 matches = Sse2.MoveMask(Sse2.CompareEqual(values, search).AsByte());
				if (matches == 0)
					offset += Vector128<UInt16>.Count;
				else
					return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches) / sizeof(Char));
			}

			lengthToExamine = MemoryMarshalCompat.GetCharVector256SpanLength(offset);
			if (lengthToExamine > 0)
			{
				Vector256<UInt16> values = Vector256.Create(value);
				do
				{
					Vector256<UInt16> search = MemoryMarshalCompat.LoadVector256(ref searchSpace, offset);
					Int32 matches = Avx2.MoveMask(Avx2.CompareEqual(values, search).AsByte());

					if (matches != 0)
						return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches) / sizeof(Char));

					offset += Vector256<UInt16>.Count;
					lengthToExamine -= Vector256<UInt16>.Count;
				} while (lengthToExamine > 0);
			}

			lengthToExamine = MemoryMarshalCompat.GetCharVector128SpanLength(offset);
			if (lengthToExamine > 0)
			{
				Vector128<UInt16> values = Vector128.Create(value);
				Vector128<UInt16> search = MemoryMarshalCompat.LoadVector128(ref searchSpace, offset);

				Int32 matches = Sse2.MoveMask(Sse2.CompareEqual(values, search).AsByte());
				if (matches == 0)
					offset += Vector128<UInt16>.Count;
				else
					return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches) / sizeof(Char));
			}

			if (offset >= Int32.MaxValue) return -1;

			lengthToExamine = Int32.MaxValue - offset;
			goto SequentialScan;
		}
		if (Sse2.IsSupported)
		{
			if (offset >= Int32.MaxValue) return -1;

			lengthToExamine = MemoryMarshalCompat.GetCharVector128SpanLength(offset);
			if (lengthToExamine > 0)
			{
				Vector128<UInt16> values = Vector128.Create(value);
				do
				{
					Vector128<UInt16> search = MemoryMarshalCompat.LoadVector128(ref searchSpace, offset);
					Int32 matches = Sse2.MoveMask(Sse2.CompareEqual(values, search).AsByte());
					if (matches != 0)
						return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches) / sizeof(Char));

					offset += Vector128<UInt16>.Count;
					lengthToExamine -= Vector128<UInt16>.Count;
				} while (lengthToExamine > 0);
			}

			if (offset >= Int32.MaxValue) return -1;

			lengthToExamine = Int32.MaxValue - offset;
			goto SequentialScan;
		}
		if (AdvSimd.Arm64.IsSupported)
		{
			if (offset >= Int32.MaxValue) return -1;

			lengthToExamine = MemoryMarshalCompat.GetCharVector128SpanLength(offset);
			if (lengthToExamine > 0)
			{
				Vector128<UInt16> values = Vector128.Create(value);
				Int32 matchedLane = 0;

				do
				{
					Vector128<UInt16> search = MemoryMarshalCompat.LoadVector128(ref searchSpace, offset);
					Vector128<UInt16> compareResult = AdvSimd.CompareEqual(values, search);

					if (MemoryMarshalCompat.TryFindFirstMatchedLane(compareResult, ref matchedLane))
						return (Int32)(offset + matchedLane);

					offset += Vector128<UInt16>.Count;
					lengthToExamine -= Vector128<UInt16>.Count;
				} while (lengthToExamine > 0);
			}

			if (offset >= Int32.MaxValue) return -1;

			lengthToExamine = Int32.MaxValue - offset;
			goto SequentialScan;
		}
		if (!Vector.IsHardwareAccelerated) return -1;

		{
			if (offset >= Int32.MaxValue) return -1;
			lengthToExamine = MemoryMarshalCompat.GetCharVectorSpanLength(offset, Int32.MaxValue);

			if (lengthToExamine > 0)
			{
				Vector<UInt16> values = new(value);
				do
				{
					Vector<UInt16> matches =
						Vector.Equals(values, MemoryMarshalCompat.LoadVector(ref searchSpace, offset));
					if (!Vector<UInt16>.Zero.Equals(matches))
						return (Int32)(offset + MemoryMarshalCompat.LocateFirstFoundChar(matches));

					offset += Vector<UInt16>.Count;
					lengthToExamine -= Vector<UInt16>.Count;
				} while (lengthToExamine > 0);
			}

			if (offset >= Int32.MaxValue) return -1;

			lengthToExamine = Int32.MaxValue - offset;
			goto SequentialScan;
		}
		Found3:
		return (Int32)(offset + 3);
		Found2:
		return (Int32)(offset + 2);
		Found1:
		return (Int32)(offset + 1);
		Found:
		return (Int32)offset;
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr UnalignedCountVector(ref Char searchSpace)
	{
		const Int32 elementsPerByte = sizeof(UInt16) / sizeof(Byte);
		return (IntPtr)(UInt32)(-(Int32)Unsafe.AsPointer(ref searchSpace) / elementsPerByte) &
			(Vector<UInt16>.Count - 1);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr UnalignedCountVector(ref Byte searchSpace)
	{
		IntPtr unaligned = (IntPtr)Unsafe.AsPointer(ref searchSpace) & (Vector<Byte>.Count - 1);
		return (UIntPtr)((Vector<Byte>.Count - unaligned) & (Vector<Byte>.Count - 1));
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr UnalignedCountVector128(ref Char searchSpace)
	{
		const Int32 elementsPerByte = sizeof(UInt16) / sizeof(Byte);
		return (IntPtr)(UInt32)(-(Int32)Unsafe.AsPointer(ref searchSpace) / elementsPerByte) &
			(Vector128<UInt16>.Count - 1);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr UnalignedCountVector128(ref Byte searchSpace)
	{
		IntPtr unaligned = (IntPtr)Unsafe.AsPointer(ref searchSpace) & (Vector128<Byte>.Count - 1);
		return (UInt32)((Vector128<Byte>.Count - unaligned) & (Vector128<Byte>.Count - 1));
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 LocateFirstFoundChar(Vector<UInt16> match)
	{
		Vector<UInt64> vector64 = Vector.AsVectorUInt64(match);
		UInt64 candidate = 0;
		Int32 i = 0;
		for (; i < Vector<UInt64>.Count; i++)
		{
			candidate = vector64[i];
			if (candidate != 0) break;
		}
		return i * 4 + MemoryMarshalCompat.LocateFirstFoundChar(candidate);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 LocateFirstFoundChar(UInt64 match) => BitOperations.TrailingZeroCount(match) >> 4;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 LocateFirstFoundByte(Vector<Byte> match)
	{
		Vector<UInt64> vector64 = Vector.AsVectorUInt64(match);
		UInt64 candidate = 0;
		Int32 i = 0;

		for (; i < Vector<UInt64>.Count; i++)
		{
			candidate = vector64[i];
			if (candidate != 0) break;
		}

		return i * 8 + MemoryMarshalCompat.LocateFirstFoundByte(candidate);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 LocateFirstFoundByte(UInt64 match) => BitOperations.TrailingZeroCount(match) >> 3;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<UInt16> LoadVector128(ref Char start, IntPtr offset)
		=> Unsafe.ReadUnaligned<Vector128<UInt16>>(ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref start, offset)));
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<Byte> LoadVector128(ref Byte start, UIntPtr offset)
		=> Unsafe.ReadUnaligned<Vector128<Byte>>(
			ref Unsafe.AddByteOffset(ref start, MemoryMarshalCompat.ToByteOffset(offset)));
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector<Byte> LoadVector(ref Byte start, UIntPtr offset)
		=> Unsafe.ReadUnaligned<Vector<Byte>>(
			ref Unsafe.AddByteOffset(ref start, MemoryMarshalCompat.ToByteOffset(offset)));
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector<UInt16> LoadVector(ref Char start, IntPtr offset)
		=> Unsafe.ReadUnaligned<Vector<UInt16>>(ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref start, offset)));
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector256<UInt16> LoadVector256(ref Char start, IntPtr offset)
		=> Unsafe.ReadUnaligned<Vector256<UInt16>>(ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref start, offset)));
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector256<Byte> LoadVector256(ref Byte start, UIntPtr offset)
		=> Unsafe.ReadUnaligned<Vector256<Byte>>(
			ref Unsafe.AddByteOffset(ref start, MemoryMarshalCompat.ToByteOffset(offset)));
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr GetCharVectorSpanLength(IntPtr offset, IntPtr length)
		=> (length - offset) & ~(Vector<UInt16>.Count - 1);
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr GetCharVector256SpanLength(IntPtr offset)
		=> (Int32.MaxValue - offset) & ~(Vector256<UInt16>.Count - 1);
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr GetCharVector128SpanLength(IntPtr offset)
		=> (Int32.MaxValue - offset) & ~(Vector128<UInt16>.Count - 1);
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr GetByteVector128SpanLength(UIntPtr offset)
		=> (UInt32)((Int32.MaxValue - (Int32)offset) & ~(Vector128<Byte>.Count - 1));
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr GetByteVector256SpanLength(UIntPtr offset)
		=> (UInt32)((Int32.MaxValue - (Int32)offset) & ~(Vector256<Byte>.Count - 1));
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr GetByteVectorSpanLength(UIntPtr offset, Int32 length)
		=> (UInt32)((length - (Int32)offset) & ~(Vector<Byte>.Count - 1));
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean TryFindFirstMatchedLane(Vector128<UInt16> compareResult, ref Int32 matchedLane)
	{
		Vector128<Byte> pairwiseSelectedLane =
			AdvSimd.Arm64.AddPairwise(compareResult.AsByte(), compareResult.AsByte());
		UInt64 selectedLanes = pairwiseSelectedLane.AsUInt64().ToScalar();
		if (selectedLanes == 0) return false;
		matchedLane = BitOperations.TrailingZeroCount(selectedLanes) >> 3;
		return true;
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean TryFindFirstMatchedLane(Vector128<Byte> mask, Vector128<Byte> compareResult,
		ref Int32 matchedLane)
	{
		Vector128<Byte> maskedSelectedLanes = AdvSimd.And(compareResult, mask);
		Vector128<Byte> pairwiseSelectedLane = AdvSimd.Arm64.AddPairwise(maskedSelectedLanes, maskedSelectedLanes);
		UInt64 selectedLanes = pairwiseSelectedLane.AsUInt64().ToScalar();
		if (selectedLanes == 0) return false;
		matchedLane = BitOperations.TrailingZeroCount(selectedLanes) >> 2;
		return true;
	}
#if !NET5_0_OR_GREATER
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1172)]
#endif
	private static class AdvSimd
	{
		public static Vector128<T> CompareEqual<T>(Vector128<T> values, Vector128<T> _) where T : unmanaged => values;
		public static Vector128<Byte> And(Vector128<Byte> compareResult, Vector128<Byte> _) => compareResult;

		public static class Arm64
		{
			public static Boolean IsSupported => false;
			public static Vector128<Byte> AddPairwise(Vector128<Byte> maskedSelectedLanes, Vector128<Byte> _)
				=> maskedSelectedLanes;
		}
	}
#endif
#pragma warning restore CA2020
}
#endif