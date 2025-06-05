#if !NET6_0_OR_GREATER
using UIntPtr = nuint;
using IntPtr = nint;

namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helps to find unsafe null-terminated strings length.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static unsafe class MemoryMarshallUtilities
{
	/// <summary>
	/// Creates a new read-only span for a null-terminated UTF8 string.
	/// </summary>
	/// <param name="value">The pointer to the null-terminated string of bytes.</param>
	/// <returns>
	/// A read-only span representing the specified null-terminated string, or an empty span if the pointer is null.
	/// </returns>
	/// <remarks>
	/// The returned span does not include the null terminator, nor does it validate the well-formedness of the UTF8 data.
	/// </remarks>
	/// <exception cref="ArgumentException">The string is longer than <see cref="Int32.MaxValue"/>.</exception>
	public static ReadOnlySpan<Byte> CreateReadOnlySpanFromNullTerminated(Byte* value)
	{
		ref Byte ref0 = ref *value;
		Int32 length = MemoryMarshallUtilities.IndexOf(ref *value, (Byte)'\0', Int32.MaxValue);
		if (length < 0)
			throw new ArgumentException(null, nameof(value));
		return MemoryMarshal.CreateReadOnlySpan(ref ref0, length);
	}
	/// <summary>
	/// Creates a new read-only span for a null-terminated string.
	/// </summary>
	/// <param name="value">The pointer to the null-terminated string of characters.</param>
	/// <returns>
	/// A read-only span representing the specified null-terminated string, or an empty span if the pointer is null.
	/// </returns>
	/// <remarks>The returned span does not include the null terminator.</remarks>
	/// <exception cref="ArgumentException">The string is longer than <see cref="int.MaxValue"/>.</exception>
	public static ReadOnlySpan<Char> CreateReadOnlySpanFromNullTerminated(Char* value)
	{
		ref Char ref0 = ref *value;
		Int32 length = MemoryMarshallUtilities.IndexOf(ref ref0, '\0', Int32.MaxValue);
		if (length < 0)
			throw new ArgumentException(null, nameof(value));
		return MemoryMarshal.CreateReadOnlySpan(ref ref0, length);
	}

	#region CORECLRCODE
	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	private static Int32 IndexOf(ref Byte searchSpace, Byte value, Int32 length)
	{
		UInt32 uValue = value; // Use uint for comparisons to avoid unnecessary 8->32 extensions
		UIntPtr offset = 0; // Use nuint for arithmetic to avoid unnecessary 64->32->64 truncations
		UIntPtr lengthToExamine = (UInt32)length;

		if (Sse2.IsSupported || AdvSimd.Arm64.IsSupported)
		{
			if (length >= Vector128<Byte>.Count * 2)
				lengthToExamine = MemoryMarshallUtilities.UnalignedCountVector128(ref searchSpace);
		}
		else if (Vector.IsHardwareAccelerated)
		{
			if (length >= Vector<Byte>.Count * 2)
				lengthToExamine = MemoryMarshallUtilities.UnalignedCountVector(ref searchSpace);
		}
		SequentialScan:
		while (lengthToExamine >= 8)
		{
			lengthToExamine -= 8;

			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)offset)))
				goto Found;
			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)(offset + 1))))
				goto Found1;
			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)(offset + 2))))
				goto Found2;
			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)(offset + 3))))
				goto Found3;
			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)(offset + 4))))
				goto Found4;
			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)(offset + 5))))
				goto Found5;
			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)(offset + 6))))
				goto Found6;
			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)(offset + 7))))
				goto Found7;

			offset += 8;
		}

		if (lengthToExamine >= 4)
		{
			lengthToExamine -= 4;

			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)offset)))
				goto Found;
			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)(offset + 1))))
				goto Found1;
			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)(offset + 2))))
				goto Found2;
			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)(offset + 3))))
				goto Found3;

			offset += 4;
		}

		while (lengthToExamine > 0)
		{
			lengthToExamine -= 1;

			if (uValue == Unsafe.AddByteOffset(ref searchSpace, new((void*)offset)))
				goto Found;

			offset += 1;
		}

		if (Avx2.IsSupported)
		{
			if (offset >= (UInt32)length) return -1;

			if ((((UInt32)Unsafe.AsPointer(ref searchSpace) + offset) & (UIntPtr)(Vector256<Byte>.Count - 1)) != 0)
			{
				Vector128<Byte> values = Vector128.Create(value);
				Vector128<Byte> search = MemoryMarshallUtilities.LoadVector128(ref searchSpace, offset);

				Int32 matches = Sse2.MoveMask(Sse2.CompareEqual(values, search));
				if (matches == 0)
					offset += (UIntPtr)Vector128<Byte>.Count;
				else
					return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches));
			}

			lengthToExamine = MemoryMarshallUtilities.GetByteVector256SpanLength(offset, length);
			if (lengthToExamine > offset)
			{
				Vector256<Byte> values = Vector256.Create(value);
				do
				{
					Vector256<Byte> search = MemoryMarshallUtilities.LoadVector256(ref searchSpace, offset);
					Int32 matches = Avx2.MoveMask(Avx2.CompareEqual(values, search));

					if (matches != 0) return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches));

					offset += (UIntPtr)Vector256<Byte>.Count;
				} while (lengthToExamine > offset);
			}

			lengthToExamine = MemoryMarshallUtilities.GetByteVector128SpanLength(offset, length);
			if (lengthToExamine > offset)
			{
				Vector128<Byte> values = Vector128.Create(value);
				Vector128<Byte> search = MemoryMarshallUtilities.LoadVector128(ref searchSpace, offset);

				Int32 matches = Sse2.MoveMask(Sse2.CompareEqual(values, search));
				if (matches == 0)
					offset += (UIntPtr)Vector128<Byte>.Count;
				else
					return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches));
			}

			if (offset >= (UInt32)length) return -1;

			lengthToExamine = ((UInt32)length - offset);
			goto SequentialScan;
		}
		if (Sse2.IsSupported)
		{
			if (offset >= (UInt32)length) return -1;

			lengthToExamine = MemoryMarshallUtilities.GetByteVector128SpanLength(offset, length);

			Vector128<Byte> values = Vector128.Create(value);
			while (lengthToExamine > offset)
			{
				Vector128<Byte> search = MemoryMarshallUtilities.LoadVector128(ref searchSpace, offset);

				// Same method as above
				Int32 matches = Sse2.MoveMask(Sse2.CompareEqual(values, search));
				if (matches != 0) return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches));

				offset += (UIntPtr)Vector128<Byte>.Count;
			}

			if (offset >= (UInt32)length) return -1;
			lengthToExamine = (UInt32)length - offset;
			goto SequentialScan;
		}
		if (AdvSimd.Arm64.IsSupported)
		{
			if (offset >= (UInt32)length) return -1;

			lengthToExamine = MemoryMarshallUtilities.GetByteVector128SpanLength(offset, length);

			Vector128<Byte> mask = Vector128.Create((UInt16)0x1001).AsByte();
			Int32 matchedLane = 0;

			Vector128<Byte> values = Vector128.Create(value);
			while (lengthToExamine > offset)
			{
				Vector128<Byte> search = MemoryMarshallUtilities.LoadVector128(ref searchSpace, offset);
				Vector128<Byte> compareResult = AdvSimd.CompareEqual(values, search);

				if (MemoryMarshallUtilities.TryFindFirstMatchedLane(mask, compareResult, ref matchedLane))
					return (Int32)(offset + (UInt32)matchedLane);

				offset += (UIntPtr)Vector128<Byte>.Count;
			}

			if (offset >= (UInt32)length) return -1;
			lengthToExamine = (UInt32)length - offset;
			goto SequentialScan;
		}
		if (Vector.IsHardwareAccelerated)
		{
			if (offset >= (UInt32)length) return -1;

			lengthToExamine = MemoryMarshallUtilities.GetByteVectorSpanLength(offset, length);

			Vector<Byte> values = new(value);

			while (lengthToExamine > offset)
			{
				Vector<Byte> matches =
					Vector.Equals(values, MemoryMarshallUtilities.LoadVector(ref searchSpace, offset));
				if (!Vector<Byte>.Zero.Equals(matches))
					return (Int32)offset + MemoryMarshallUtilities.LocateFirstFoundByte(matches);

				offset += (UIntPtr)Vector<Byte>.Count;
			}

			if (offset >= (UInt32)length) return -1;

			lengthToExamine = ((UInt32)length - offset);
			goto SequentialScan;
		}
		return -1;
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
	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	private static Int32 IndexOf(ref Char searchSpace, Char value, Int32 length)
	{
		IntPtr offset = 0;
		IntPtr lengthToExamine = length;

		if (((Int32)Unsafe.AsPointer(ref searchSpace) & 1) != 0)
		{
			// Input isn't char aligned, we won't be able to align it to a Vector
		}
		else if (Sse2.IsSupported || AdvSimd.Arm64.IsSupported)
		{
			if (length >= Vector128<UInt16>.Count * 2)
				lengthToExamine = MemoryMarshallUtilities.UnalignedCountVector128(ref searchSpace);
		}
		else if (Vector.IsHardwareAccelerated)
		{
			if (length >= Vector<UInt16>.Count * 2)
				lengthToExamine = MemoryMarshallUtilities.UnalignedCountVector(ref searchSpace);
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
			if (offset >= length) return -1;

			if (((IntPtr)Unsafe.AsPointer(ref Unsafe.Add(ref searchSpace, offset)) & (Vector256<Byte>.Count - 1)) != 0)
			{
				Vector128<UInt16> values = Vector128.Create(value);
				Vector128<UInt16> search = MemoryMarshallUtilities.LoadVector128(ref searchSpace, offset);
				Int32 matches = Sse2.MoveMask(Sse2.CompareEqual(values, search).AsByte());
				if (matches == 0)
					offset += Vector128<UInt16>.Count;
				else
					return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches) / sizeof(Char));
			}

			lengthToExamine = MemoryMarshallUtilities.GetCharVector256SpanLength(offset, length);
			if (lengthToExamine > 0)
			{
				Vector256<UInt16> values = Vector256.Create(value);
				do
				{
					Vector256<UInt16> search = MemoryMarshallUtilities.LoadVector256(ref searchSpace, offset);
					Int32 matches = Avx2.MoveMask(Avx2.CompareEqual(values, search).AsByte());

					if (matches != 0)
						return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches) / sizeof(Char));

					offset += Vector256<UInt16>.Count;
					lengthToExamine -= Vector256<UInt16>.Count;
				} while (lengthToExamine > 0);
			}

			lengthToExamine = MemoryMarshallUtilities.GetCharVector128SpanLength(offset, length);
			if (lengthToExamine > 0)
			{
				Vector128<UInt16> values = Vector128.Create(value);
				Vector128<UInt16> search = MemoryMarshallUtilities.LoadVector128(ref searchSpace, offset);

				Int32 matches = Sse2.MoveMask(Sse2.CompareEqual(values, search).AsByte());
				if (matches == 0)
					offset += Vector128<UInt16>.Count;
				else
					return (Int32)(offset + (UInt32)BitOperations.TrailingZeroCount(matches) / sizeof(Char));
			}

			if (offset >= length) return -1;

			lengthToExamine = length - offset;
			goto SequentialScan;
		}
		if (Sse2.IsSupported)
		{
			if (offset >= length) return -1;

			lengthToExamine = MemoryMarshallUtilities.GetCharVector128SpanLength(offset, length);
			if (lengthToExamine > 0)
			{
				Vector128<UInt16> values = Vector128.Create(value);
				do
				{
					Vector128<UInt16> search = MemoryMarshallUtilities.LoadVector128(ref searchSpace, offset);
					Int32 matches = Sse2.MoveMask(Sse2.CompareEqual(values, search).AsByte());
					if (matches != 0)
						return (Int32)(offset + ((UInt32)BitOperations.TrailingZeroCount(matches) / sizeof(Char)));

					offset += Vector128<UInt16>.Count;
					lengthToExamine -= Vector128<UInt16>.Count;
				} while (lengthToExamine > 0);
			}

			if (offset >= length) return -1;

			lengthToExamine = length - offset;
			goto SequentialScan;
		}
		if (AdvSimd.Arm64.IsSupported)
		{
			if (offset >= length) return -1;

			lengthToExamine = MemoryMarshallUtilities.GetCharVector128SpanLength(offset, length);
			if (lengthToExamine > 0)
			{
				Vector128<UInt16> values = Vector128.Create(value);
				Int32 matchedLane = 0;

				do
				{
					Vector128<UInt16> search = MemoryMarshallUtilities.LoadVector128(ref searchSpace, offset);
					Vector128<UInt16> compareResult = AdvSimd.CompareEqual(values, search);

					if (MemoryMarshallUtilities.TryFindFirstMatchedLane(compareResult, ref matchedLane))
						return (Int32)(offset + matchedLane);

					offset += Vector128<UInt16>.Count;
					lengthToExamine -= Vector128<UInt16>.Count;
				} while (lengthToExamine > 0);
			}

			if (offset >= length) return -1;

			lengthToExamine = length - offset;
			goto SequentialScan;
		}
		if (Vector.IsHardwareAccelerated)
		{
			if (offset >= length) return -1;
			lengthToExamine = MemoryMarshallUtilities.GetCharVectorSpanLength(offset, length);

			if (lengthToExamine > 0)
			{
				Vector<UInt16> values = new(value);
				do
				{
					Vector<UInt16> matches =
						Vector.Equals(values, MemoryMarshallUtilities.LoadVector(ref searchSpace, offset));
					if (!Vector<UInt16>.Zero.Equals(matches))
						return (Int32)(offset + MemoryMarshallUtilities.LocateFirstFoundChar(matches));

					offset += Vector<UInt16>.Count;
					lengthToExamine -= Vector<UInt16>.Count;
				} while (lengthToExamine > 0);
			}

			if (offset >= length) return -1;

			lengthToExamine = length - offset;
			goto SequentialScan;
		}
		return -1;
		Found3:
		return (Int32)(offset + 3);
		Found2:
		return (Int32)(offset + 2);
		Found1:
		return (Int32)(offset + 1);
		Found:
		return (Int32)(offset);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 LocateLastFoundByte(UInt64 match) => BitOperations.Log2(match) >> 3;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector<Byte> LoadVector(ref Byte start, UIntPtr offset)
		=> Unsafe.ReadUnaligned<Vector<Byte>>(ref Unsafe.AddByteOffset(ref start, new((void*)offset)));
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr UnalignedCountVector128(ref Char searchSpace)
	{
		const Int32 elementsPerByte = sizeof(UInt16) / sizeof(Byte);
		return (IntPtr)(UInt32)(-(Int32)Unsafe.AsPointer(ref searchSpace) / elementsPerByte) &
			(Vector128<UInt16>.Count - 1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr UnalignedCountVector128(ref Byte searchSpace)
	{
		IntPtr unaligned = (IntPtr)Unsafe.AsPointer(ref searchSpace) & (Vector128<Byte>.Count - 1);
		return (UInt32)((Vector128<Byte>.Count - unaligned) & (Vector128<Byte>.Count - 1));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr UnalignedCountVector(ref Char searchSpace)
	{
		const Int32 elementsPerByte = sizeof(UInt16) / sizeof(Byte);
		return (IntPtr)(UInt32)(-(Int32)Unsafe.AsPointer(ref searchSpace) / elementsPerByte) &
			(Vector<UInt16>.Count - 1);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr UnalignedCountVector(ref Byte searchSpace)
	{
		IntPtr unaligned = (IntPtr)Unsafe.AsPointer(ref searchSpace) & (Vector<Byte>.Count - 1);
		return (UIntPtr)((Vector<Byte>.Count - unaligned) & (Vector<Byte>.Count - 1));
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<UInt16> LoadVector128(ref Char start, IntPtr offset)
		=> Unsafe.ReadUnaligned<Vector128<UInt16>>(ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref start, offset)));
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector128<Byte> LoadVector128(ref Byte start, UIntPtr offset)
		=> Unsafe.ReadUnaligned<Vector128<Byte>>(ref Unsafe.AddByteOffset(ref start, new((void*)offset)));
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 LocateFirstFoundChar(Vector<ushort> match)
	{
		Vector<UInt64> vector64 = Vector.AsVectorUInt64(match);
		UInt64 candidate = 0;
		Int32 i = 0;
		for (; i < Vector<UInt64>.Count; i++)
		{
			candidate = vector64[i];
			if (candidate != 0) break;
		}
		return i * 4 + MemoryMarshallUtilities.LocateFirstFoundChar(candidate);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 LocateFirstFoundChar(UInt64 match) => BitOperations.TrailingZeroCount(match) >> 4;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector<UInt16> LoadVector(ref Char start, IntPtr offset)
		=> Unsafe.ReadUnaligned<Vector<UInt16>>(ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref start, offset)));
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr GetCharVectorSpanLength(IntPtr offset, IntPtr length)
		=> (length - offset) & ~(Vector<UInt16>.Count - 1);
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr GetCharVector128SpanLength(IntPtr offset, IntPtr length)
		=> (length - offset) & ~(Vector128<UInt16>.Count - 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector256<UInt16> LoadVector256(ref Char start, IntPtr offset)
		=> Unsafe.ReadUnaligned<Vector256<UInt16>>(ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref start, offset)));
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector256<byte> LoadVector256(ref Byte start, UIntPtr offset)
		=> Unsafe.ReadUnaligned<Vector256<Byte>>(ref Unsafe.AddByteOffset(ref start, new((void*)offset)));
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr GetCharVector256SpanLength(IntPtr offset, IntPtr length)
		=> (length - offset) & ~(Vector256<UInt16>.Count - 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr GetByteVectorSpanLength(UIntPtr offset, Int32 length)
		=> (UInt32)((length - (Int32)offset) & ~(Vector<Byte>.Count - 1));
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

		return i * 8 + LocateFirstFoundByte(candidate);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 LocateFirstFoundByte(UInt64 match) => BitOperations.TrailingZeroCount(match) >> 3;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr GetByteVector128SpanLength(UIntPtr offset, Int32 length)
		=> (UInt32)((length - (Int32)offset) & ~(Vector128<Byte>.Count - 1));
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr GetByteVector256SpanLength(UIntPtr offset, Int32 length)
		=> (UInt32)((length - (Int32)offset) & ~(Vector256<Byte>.Count - 1));
	#endregion
}
#endif