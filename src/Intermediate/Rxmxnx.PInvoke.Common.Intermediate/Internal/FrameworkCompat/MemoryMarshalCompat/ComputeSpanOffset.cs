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
	/// Computes the offsets of the fields in any instance of <see cref="Span{T}"/>.
	/// </summary>
	/// <returns>A <see cref="SpanOffset"/> instance.</returns>
	private static SpanOffset ComputeOffset()
	{
#pragma warning disable CS8500
		Int32 lengthA = 0x13579BDF;
		Int32 lengthB = 0x2468ACE1;

		Int32 spanSize = sizeof(Span<Byte>);
		Int32 minimumSize = 2 * IntPtr.Size + sizeof(Int32);

		if (spanSize < minimumSize)
			throw new PlatformNotSupportedException("The current runtime does not use the three-field Span<T> layout.");

		Byte* scratch = stackalloc Byte[64];
		void* pointerA = scratch + 7;
		void* pointerB = scratch + 43;

		// _pinnable   = null
		// _byteOffset = pointerA / pointerB
		// _length     = lengthA / lengthB
		Span<Byte> unmanagedA = new(pointerA, lengthA);
		Span<Byte> unmanagedB = new(pointerB, lengthB);

		Byte[] arrayA = new Byte[1];
		Byte[] arrayB = new Byte[2];

		// _pinnable   = arrayA / arrayB
		// _byteOffset = Type offset
		// _length     = 1 / 2
		Span<Byte> managedA = new(arrayA);
		Span<Byte> managedB = new(arrayB);

		B1 referenceA = new();
		B1 referenceB = new();

		IntPtr pointerValueA = (IntPtr)pointerA;
		IntPtr pointerValueB = (IntPtr)pointerB;
		ReadOnlySpan<Byte> pointerMarkerA = new(&pointerValueA, sizeof(IntPtr));
		ReadOnlySpan<Byte> pointerMarkerB = new(&pointerValueB, sizeof(IntPtr));
		ReadOnlySpan<Byte> lengthMarkerA = new(&lengthA, sizeof(Int32));
		ReadOnlySpan<Byte> lengthMarkerB = new(&lengthB, sizeof(Int32));

		ref Span<Byte> unmanagedARef = ref unmanagedA;
		ref Span<Byte> unmanagedBRef = ref unmanagedB;
		ref Span<Byte> managedARef = ref managedA;
		ref Span<Byte> managedBRef = ref managedB;

		SpanOffset result;

		Unsafe.As<B1, Byte[]>(ref referenceA) = arrayA;
		Unsafe.As<B1, Byte[]>(ref referenceB) = arrayB;

		fixed (Byte* pinnedA = arrayA)
		fixed (Byte* pinnedB = arrayB)
		fixed (void* unmanagedAPtr = &unmanagedARef)
		fixed (void* unmanagedBPtr = &unmanagedBRef)
		fixed (void* managedAPtr = &managedARef)
		fixed (void* managedBPtr = &managedBRef)
		{
			_ = pinnedA;
			_ = pinnedB;
			ReadOnlySpan<Byte> unmanagedBytesA = new(unmanagedAPtr, spanSize);
			ReadOnlySpan<Byte> unmanagedBytesB = new(unmanagedBPtr, spanSize);

			ReadOnlySpan<Byte> managedBytesA = new(managedAPtr, spanSize);
			ReadOnlySpan<Byte> managedBytesB = new(managedBPtr, spanSize);

			ReadOnlySpan<Byte> referenceMarkerA = new(Unsafe.AsPointer(ref referenceA), IntPtr.Size);
			ReadOnlySpan<Byte> referenceMarkerB = new(Unsafe.AsPointer(ref referenceB), IntPtr.Size);

			Int32 byteOffsetOffset = MemoryMarshalCompat.FindUniqueFieldOffset(
				new(unmanagedBytesA, pointerMarkerA), new(unmanagedBytesB, pointerMarkerB));

			Int32 lengthOffset = MemoryMarshalCompat.FindUniqueFieldOffset(
				new(unmanagedBytesA, lengthMarkerA), new(unmanagedBytesB, lengthMarkerB));

			Int32 pinnableOffset = MemoryMarshalCompat.FindUniqueFieldOffset(
				new(managedBytesA, referenceMarkerA), new(managedBytesB, referenceMarkerB));

			MemoryMarshalCompat.ValidateSpanLayout(spanSize, pinnableOffset, byteOffsetOffset, lengthOffset);
			result = new()
			{
				PinnableOffset = pinnableOffset, PointerOffset = byteOffsetOffset, LengthOffset = lengthOffset,
			};
		}

		GC.KeepAlive(arrayA);
		GC.KeepAlive(arrayB);

		return result;
#pragma warning restore CS8500
	}

	/// <summary>
	/// Validates the detected field offsets for a three-field <see cref="Span{T}"/> representation.
	/// </summary>
	/// <param name="spanSize">The total size, in bytes, of the <see cref="Span{T}"/> structure.</param>
	/// <param name="pinnableOffset"> The byte offset of the field containing the backing object reference.</param>
	/// <param name="byteOffsetOffset">
	/// The byte offset of the field containing the runtime-specific byte offset or unmanaged memory address.
	/// </param>
	/// <param name="lengthOffset">The byte offset of the field containing the span length.</param>
	/// <exception cref="PlatformNotSupportedException">
	/// One or more fields do not fit within the structure, or the detected field ranges overlap.
	/// </exception>
	private static void ValidateSpanLayout(Int32 spanSize, Int32 pinnableOffset, Int32 byteOffsetOffset,
		Int32 lengthOffset)
	{
		if (!MemoryMarshalCompat.Fits(pinnableOffset, IntPtr.Size, spanSize) ||
		    !MemoryMarshalCompat.Fits(byteOffsetOffset, IntPtr.Size, spanSize) ||
		    !MemoryMarshalCompat.Fits(lengthOffset, sizeof(Int32), spanSize))
			throw new PlatformNotSupportedException("Unable to identify the three-field Span<T> layout.");

		if (MemoryMarshalCompat.Overlaps(pinnableOffset, IntPtr.Size, byteOffsetOffset, IntPtr.Size) ||
		    MemoryMarshalCompat.Overlaps(pinnableOffset, IntPtr.Size, lengthOffset, sizeof(Int32)) ||
		    MemoryMarshalCompat.Overlaps(byteOffsetOffset, IntPtr.Size, lengthOffset, sizeof(Int32)))
			throw new PlatformNotSupportedException("The detected Span<T> fields overlap.");
	}
	/// <summary>
	/// Determines whether a field range fits completely within a containing memory range.
	/// </summary>
	/// <param name="offset">The zero-based byte offset at which the field begins.</param>
	/// <param name="fieldSize">The size, in bytes, of the field. </param>
	/// <param name="containerSize">The total size, in bytes, of the containing memory range.</param>
	/// <returns>
	/// <see langword="true"/> if the field range is nonnegative and fits entirely within the containing range;
	/// otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean Fits(Int32 offset, Int32 fieldSize, Int32 containerSize)
		=> offset >= 0 && fieldSize >= 0 && offset <= containerSize - fieldSize;
	/// <summary>
	/// Determines whether two half-open byte ranges overlap.
	/// </summary>
	/// <param name="firstOffset">The zero-based byte offset at which the first range begins.</param>
	/// <param name="firstSize">The size, in bytes, of the first range.</param>
	/// <param name="secondOffset">The zero-based byte offset at which the second range begins.</param>
	/// <param name="secondSize"> The size, in bytes, of the second range.</param>
	/// <returns>
	/// <see langword="true"/> if the ranges share at least one byte; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean Overlaps(Int32 firstOffset, Int32 firstSize, Int32 secondOffset, Int32 secondSize)
		=> firstOffset < secondOffset + secondSize && secondOffset < firstOffset + firstSize;
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