#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
#if PACKAGE && !NET5_0_OR_GREATER
using B1 = Rxmxnx.PInvoke.Buffers.Atomic<System.Object>;
#elif !NET5_0_OR_GREATER
using B1 = Rxmxnx.PInvoke.NativeUtilities.B1;
#endif

namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static unsafe partial class MemoryMarshalCompat
{
	/// <summary>
	/// Computed <see cref="SpanOffset"/> value.
	/// </summary>
	private static SpanOffset? unsafeSpanOffset;

	/// <summary>
	/// Retrieves the offsets of the fields in any instance of <see cref="Span{T}"/>
	/// </summary>
	private static SpanOffset UnsafeSpanOffset
		=> MemoryMarshalCompat.unsafeSpanOffset ??= MemoryMarshalCompat.ComputeOffset();

	/// <summary>
	/// Sets the <see cref="Pinnable{T}"/> field value.
	/// </summary>
	/// <typeparam name="T">The type of the data items.</typeparam>
	/// <param name="pinnable">The <see cref="Pinnable{T}"/> instance.</param>
	/// <param name="pResult">Pointer to the span layout.</param>
	/// <param name="offsets">Computed span offsets.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void SetPinnableField<T>(Pinnable<T> pinnable, void* pResult, SpanOffset offsets)
	{
		Byte* pinnableSlotPtr = (Byte*)pResult + offsets.PinnableOffset;
		ref Pinnable<T> pinnableSlotRef = ref Unsafe.AsRef<Pinnable<T>>(pinnableSlotPtr);
		pinnableSlotRef = pinnable;
	}
	/// <summary>
	/// Computes the offsets of the fields in any instance of <see cref="Span{T}"/>.
	/// </summary>
	/// <returns>A <see cref="SpanOffset"/> instance.</returns>
	private static SpanOffset ComputeOffset()
	{
#pragma warning disable CS8500
		Int32 spanSize = sizeof(Span<Byte>);
		if (spanSize <= 2 * IntPtr.Size)
			return new()
			{
				SpanSize = spanSize, PinnableOffset = -1, PointerOffset = -1, LengthOffset = -1,
			};
		Byte* scratch = stackalloc Byte[64];
		ReadOnlySpan<Int32> lengths = [0x13579BDF, 0x2468ACE1,];
		ReadOnlySpan<IntPtr> pointers = [new(scratch + 7), new(scratch + 43),];
		// _pinnable   = null
		// _byteOffset = pointerA / pointerB
		// _length     = lengthA / lengthB
		Span<Byte> uA = new((void*)pointers[0], lengths[0]), uB = new((void*)pointers[1], lengths[1]);
		Byte[] arrayA = new Byte[1], arrayB = new Byte[2];
		// _pinnable   = arrayA / arrayB
		// _byteOffset = Type offset
		// _length     = 1 / 2
		Span<Byte> mA = new(arrayA), mB = new(arrayB);
		B1 wA = new(), wB = new();
		ref Span<Byte> uAr = ref uA, uBr = ref uB, mAr = ref mA, mBr = ref mB;
		SpanOffset result;

		Unsafe.As<B1, Byte[]>(ref wA) = arrayA;
		Unsafe.As<B1, Byte[]>(ref wB) = arrayB;

		fixed (Byte* _ = arrayA)
		fixed (Byte* __ = arrayB)
		fixed (void* unmanagedAPtr = &uAr)
		fixed (void* unmanagedBPtr = &uBr)
		fixed (void* managedAPtr = &mAr)
		fixed (void* managedBPtr = &mBr)
		{
			ReadOnlySpan<Byte> uBytesA = new(unmanagedAPtr, spanSize), uBytesB = new(unmanagedBPtr, spanSize);
			ReadOnlySpan<Byte> mBytesA = new(managedAPtr, spanSize), mBytesB = new(managedBPtr, spanSize);

			Int32 byteOffsetOffset = MemoryMarshalCompat.FindUniqueFieldOffset(
				new(uBytesA, MemoryMarshal.AsBytes(pointers[..1])), new(uBytesB, MemoryMarshal.AsBytes(pointers[1..])));
			Int32 lengthOffset = MemoryMarshalCompat.FindUniqueFieldOffset(
				new(uBytesA, MemoryMarshal.AsBytes(lengths[..1])), new(uBytesB, MemoryMarshal.AsBytes(lengths[1..])));
			Int32 pinnableOffset = MemoryMarshalCompat.FindUniqueFieldOffset(
				new(mBytesA, new(Unsafe.AsPointer(ref wA), IntPtr.Size)),
				new(mBytesB, new(Unsafe.AsPointer(ref wB), IntPtr.Size)));
			result = new()
			{
				SpanSize = spanSize,
				PinnableOffset = pinnableOffset,
				PointerOffset = byteOffsetOffset,
				LengthOffset = lengthOffset,
			};
		}

		GC.KeepAlive(arrayA);
		GC.KeepAlive(arrayB);
		return result.ValidateLayout();
#pragma warning restore CS8500
	}
	/// <summary>
	/// Finds the unique byte offset at which two field markers occur in their corresponding structure memory images.
	/// </summary>
	/// <param name="first">The first field-layout probe.</param>
	/// <param name="second">
	/// The second field-layout probe containing an independently initialized value for the same field.
	/// </param>
	/// <returns>
	/// The unique byte offset shared by both field markers, or <c>-1</c> if the probes are incompatible, no common
	/// offset exists, or more than one common offset is found.
	/// </returns>
	/// <remarks>
	/// A candidate offset is accepted only when each probe's marker matches its associated memory image at that same
	/// offset.
	/// </remarks>
	private static Int32 FindUniqueFieldOffset(in FieldProbe first, in FieldProbe second)
	{
		if (!first.IsCompatibleWith(second))
			return -1;

		Int32 result = -1;

		for (Int32 offset = 0; offset <= first.MaximumOffset; offset++)
		{
			if (!first.EqualsAt(offset) || !second.EqualsAt(offset))
				continue;

			if (result >= 0)
				return -1;

			result = offset;
		}

		return result;
	}
}
#endif