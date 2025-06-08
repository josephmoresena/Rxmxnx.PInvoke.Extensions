#if !PACKAGE || !NET6_0_OR_GREATER

using System_IntPtr = System.IntPtr;
#if NETCOREAPP
using UIntPtr = nuint;
using IntPtr = nint;
#endif

namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

/// <summary>
/// <see cref="MemoryMarshal"/> compatibility utilities for internal use.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static unsafe class MemoryMarshalCompat
{
	/// <summary>
	/// Target framework for the current build.
	/// </summary>
	public static readonly String TargetFramework =
#if !NETCOREAPP
		".NETStandard 2.1";
#elif NETCOREAPP3_0
		".NETCoreApp 3.0.3";
#elif NETCOREAPP3_1
		".NETCoreApp 3.1.12";
#elif NET5_0
		".NETCoreApp 5.0.17";
#else
		RuntimeInformation.FrameworkDescription;
#endif
	/// <summary>
	/// Delegate for .NET 5.0+ of GetArrayDataReference method.
	/// </summary>
	private delegate ref Byte GetArrayDataReferenceDelegate(Array array);

	/// <summary>
	/// Minimal representation of CoreCLR runtime MethodTable struct.
	/// </summary>
	[StructLayout(LayoutKind.Explicit)]
	private struct MMethodTable
	{
		[FieldOffset(4)]
		public UInt32 BaseSize;
	}

#pragma warning disable CS0649
	/// <summary>
	/// CoreCLR Object data representation.
	/// </summary>
	private sealed class CoreClrRawData
	{
		/// <summary>
		/// Object data. The value of this field should not be used, only the reference to it.
		/// </summary>
		public Byte Data;
	}

	/// <summary>
	/// Mono Array data representation.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	private class MonoRawData
	{
		/// <summary>
		/// Pointer to bounds array.
		/// </summary>
		public System_IntPtr Bounds;
		/// <summary>
		/// Total number of items.
		/// </summary>
		public System_IntPtr Count;
		/// <summary>
		/// Object data. The value of this field should not be used, only the reference to it.
		/// </summary>
		public Byte Data;
	}
#pragma warning restore CS0649

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
		Int32 length = MemoryMarshalCompat.IndexOfNull(ref *value);
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
		Int32 length = MemoryMarshalCompat.IndexOfNull(ref ref0);
		if (length < 0)
			throw new ArgumentException(null, nameof(value));
		return MemoryMarshal.CreateReadOnlySpan(ref ref0, length);
	}
	/// <summary>
	/// Indicates whether <paramref name="utfSpan"/> points to null UTF-8 text-
	/// </summary>
	/// <param name="utfSpan">A UTF-8 span.</param>
	/// <returns>
	/// <see langword="true"/> if the dynamic type was successfully emitted in the current runtime; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	public static Boolean IsNullText(ReadOnlySpan<Byte> utfSpan)
	{
		fixed (Byte* ptr = &MemoryMarshal.GetReference(utfSpan))
			return ptr == System_IntPtr.Zero.ToPointer();
	}
	/// <summary>
	/// Returns a reference to the 0th element of <paramref name="array"/>.
	/// If the array is empty, returns a reference to where the 0th element would have been stored.
	/// </summary>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	/// <returns>Managed reference to <paramref name="array"/> data.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref T GetArrayDataReference<T>(Array array)
	{
		if (!RuntimeHelpers.IsReferenceOrContainsReferences<T>())
		{
			// Arrays of unmanaged items can be pinned.
			GCHandle handle = GCHandle.Alloc(array, GCHandleType.Pinned);
			try
			{
				return ref Unsafe.As<Byte, T>(
					ref MemoryMarshalCompat.GetArrayDataReference(handle.AddrOfPinnedObject().ToPointer(), array));
			}
			finally
			{
				handle.Free();
			}
		}

		ValidationUtilities.ThrowIfNoReflection();

		Type typeofRuntimeHelpers = typeof(RuntimeHelpers);

		ref MMethodTable mtpRef =
#if PACKAGE && !NETCOREAPP || NETCOREAPP3_1_OR_GREATER
			ref Unsafe.NullRef<MMethodTable>();
#else
			ref MemoryMarshal.GetReference(Span<MMethodTable>.Empty);
#endif
		if (typeof(MemoryMarshal).GetMethod("GetArrayDataReference", 0, [typeof(Array),]) is { } methodInfoNet6)
		{
			GetArrayDataReferenceDelegate del =
				(GetArrayDataReferenceDelegate)methodInfoNet6.CreateDelegate(typeof(GetArrayDataReferenceDelegate));
			return ref Unsafe.As<Byte, T>(ref del(array));
		}

		Object[] methodParam = [array,];
		if (typeofRuntimeHelpers.GetMethod("GetMethodTable", BindingFlags.Static | BindingFlags.NonPublic) is
		    { } methodInfoNet5)
		{
			Pointer mtpPtr = (Pointer)methodInfoNet5.Invoke(null, methodParam)!;
			mtpRef = ref Unsafe.AsRef<MMethodTable>(Pointer.Unbox(mtpPtr));
		}
		else if (typeofRuntimeHelpers.GetMethod("GetObjectMethodTablePointer",
		                                        BindingFlags.Static | BindingFlags.NonPublic) is { } methodInfoCore)
		{
			System_IntPtr mtpPtr = (System_IntPtr)methodInfoCore.Invoke(null, methodParam)!;
			mtpRef = ref Unsafe.AsRef<MMethodTable>(mtpPtr.ToPointer());
		}
		else
		{
			return ref Unsafe.As<Byte, T>(ref Unsafe.As<MonoRawData>(array).Data);
		}

		return ref Unsafe.As<Byte, T>(ref MemoryMarshalCompat.GetArrayDataReference(mtpRef, array));
	}
	/// <summary>
	/// Retrieves the index of the first null occurrence in the buffer represented by <paramref name="buffer"/>.
	/// </summary>
	/// <typeparam name="T">Type of buffer element.</typeparam>
	/// <param name="buffer">Buffer reference.</param>
	/// <returns>Index of</returns>
	/// <remarks>This method is used only on .Net Standard build.</remarks>
	public static Int32 IndexOfNull<T>(ref T buffer) where T : unmanaged, IEquatable<T>
	{
		UInt32 result = 0;
		T nullValue = default;
		while (!Unsafe.Add(ref buffer, new System_IntPtr((void*)result)).Equals(nullValue))
		{
			result++;
			if (result >= Int32.MaxValue)
				return -1;
		}

		return (Int32)result;
	}

	/// <summary>
	/// Converts a <see cref="UIntPtr"/> value to be valid for use with <see cref="Unsafe"/> class.
	/// </summary>
	/// <param name="ptr">A <see cref="UIntPtr"/> value.</param>
	/// <returns>A <see cref="System_IntPtr"/> instance.</returns>
#if !NETCOREAPP3_1_OR_GREATER
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static System_IntPtr ToByteOffset(UIntPtr ptr) => new((void*)ptr);
#else
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr ToByteOffset(UIntPtr ptr) => ptr;
#endif
	/// <summary>
	/// Returns a reference to the 0th element of <paramref name="array"/> using its pinned reference.
	/// </summary>
	/// <param name="addrOfPinnedArray">Address of pinned array.</param>
	/// <param name="array">Pinned array.</param>
	/// <returns>Managed reference to <paramref name="array"/> data.</returns>
	private static ref Byte GetArrayDataReference(void* addrOfPinnedArray, Array array)
	{
		ref Byte rawDataRef = ref Unsafe.As<CoreClrRawData>(array).Data;
		fixed (void* rawDataPtr = &rawDataRef)
		{
#if NETCOREAPP
			UIntPtr offset = (UIntPtr)((IntPtr)addrOfPinnedArray - (IntPtr)rawDataPtr);
#else
			UIntPtr offset = System_IntPtr.Size == sizeof(Int32) ?
				(UIntPtr)((Int32)addrOfPinnedArray - (Int32)rawDataPtr) :
				(UIntPtr)((Int64)addrOfPinnedArray - (Int64)rawDataPtr);
#endif
			return ref Unsafe.AddByteOffset(ref rawDataRef, MemoryMarshalCompat.ToByteOffset(offset));
		}
	}

	#region CORECLRCODE
	private static ref Byte GetArrayDataReference(MMethodTable mtpRef, Array array)
	{
		ref Byte rawDataRef = ref Unsafe.As<CoreClrRawData>(array).Data;
		UIntPtr byteOffset = (UIntPtr)(mtpRef.BaseSize - 2 * System_IntPtr.Size);
		return ref Unsafe.AddByteOffset(ref rawDataRef, MemoryMarshalCompat.ToByteOffset(byteOffset));
	}
#if NETCOREAPP
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
		else if (Vector.IsHardwareAccelerated)
		{
			if (Int32.MaxValue >= (UInt32)Vector<Byte>.Count * 2)
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
	[MethodImpl(MethodImplOptions.AggressiveOptimization)]
	private static Int32 IndexOfNull(ref Char searchSpace)
	{
		const Char value = '\0';
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
		else if (Vector.IsHardwareAccelerated)
		{
			if (Int32.MaxValue >= (UInt32)Vector<UInt16>.Count * 2)
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
						return (Int32)(offset + ((UInt32)BitOperations.TrailingZeroCount(matches) / sizeof(Char)));

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
		return (Int32)(offset);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector<Byte> LoadVector(ref Byte start, UIntPtr offset)
		=> Unsafe.ReadUnaligned<Vector<Byte>>(
			ref Unsafe.AddByteOffset(ref start, MemoryMarshalCompat.ToByteOffset(offset)));
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
		=> Unsafe.ReadUnaligned<Vector128<Byte>>(
			ref Unsafe.AddByteOffset(ref start, MemoryMarshalCompat.ToByteOffset(offset)));
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
	private static IntPtr GetCharVector128SpanLength(IntPtr offset)
		=> (Int32.MaxValue - offset) & ~(Vector128<UInt16>.Count - 1);
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector256<UInt16> LoadVector256(ref Char start, IntPtr offset)
		=> Unsafe.ReadUnaligned<Vector256<UInt16>>(ref Unsafe.As<Char, Byte>(ref Unsafe.Add(ref start, offset)));
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Vector256<Byte> LoadVector256(ref Byte start, UIntPtr offset)
		=> Unsafe.ReadUnaligned<Vector256<Byte>>(
			ref Unsafe.AddByteOffset(ref start, MemoryMarshalCompat.ToByteOffset(offset)));
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IntPtr GetCharVector256SpanLength(IntPtr offset)
		=> (Int32.MaxValue - offset) & ~(Vector256<UInt16>.Count - 1);
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

		return i * 8 + MemoryMarshalCompat.LocateFirstFoundByte(candidate);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 LocateFirstFoundByte(UInt64 match) => BitOperations.TrailingZeroCount(match) >> 3;
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr GetByteVector128SpanLength(UIntPtr offset)
		=> (UInt32)((Int32.MaxValue - (Int32)offset) & ~(Vector128<Byte>.Count - 1));
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static UIntPtr GetByteVector256SpanLength(UIntPtr offset)
		=> (UInt32)((Int32.MaxValue - (Int32)offset) & ~(Vector256<Byte>.Count - 1));
#if !NET5_0_OR_GREATER
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
#endif
	#endregion
}

#endif