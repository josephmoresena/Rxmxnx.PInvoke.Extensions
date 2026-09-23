#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
#if NETFRAMEWORK && !NET46_OR_GREATER
using Array = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArrayCompat;
#endif
#if PACKAGE && !NET5_0_OR_GREATER
using B2 =
	Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
		Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>;
#elif !NET5_0_OR_GREATER
using B2 = Rxmxnx.PInvoke.NativeUtilities.B2;
#endif
// ReSharper disable once BuiltInTypeReferenceStyle
using IntPtr = nint;

namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static unsafe partial class MemoryMarshalCompat
{
	/// <summary>
	/// Offset to the <see cref="Pinnable{T}"/> instance.
	/// </summary>
	private static Int32? pinnableOffset;
	/// <summary>
	/// Offset to the <see cref="Pinnable{T}"/> instance on read-only span.
	/// </summary>
	private static Int32? pinnableOffsetR;

	/// <summary>
	/// Retrieves the offsets of the fields in any instance of <see cref="Span{T}"/>
	/// </summary>
	private static Int32 PinnableOffset
		=> MemoryMarshalCompat.pinnableOffset ??= MemoryMarshalCompat.ComputePinnableOffset();

#pragma warning disable CS8500
	/// <summary>
	/// Retrieves the <c>_pinnable</c> field value.
	/// </summary>
	/// <param name="pSpan">Pointer to the span layout.</param>
	/// <returns>The managed pinnable instance.</returns>
	private static Object? GetPinnableField(void* pSpan)
	{
		Byte* pinnableSlotPtr = (Byte*)pSpan + MemoryMarshalCompat.PinnableOffset;
		return Unsafe.AsRef<Object?>(pinnableSlotPtr);
	}
	/// <summary>
	/// Retrieves the <c>_pinnable</c> field value.
	/// </summary>
	/// <param name="pSpan">Pointer to the read-only span layout.</param>
	/// <returns>The managed pinnable instance.</returns>
	private static Object? GetReadOnlyPinnableField(void* pSpan)
	{
		Byte* pinnableSlotPtr = (Byte*)pSpan +
			(MemoryMarshalCompat.pinnableOffsetR ??= MemoryMarshalCompat.ComputeReadOnlyPinnableOffset());
		return Unsafe.AsRef<Object?>(pinnableSlotPtr);
	}
	/// <summary>
	/// Sets the <c>_pinnable</c> field value.
	/// </summary>
	/// <param name="pinnable">The managed pinnable instance.</param>
	/// <param name="pSpan">Pointer to the span layout.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecurityCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void SetPinnableField(Object pinnable, void* pSpan)
	{
		Byte* pinnableSlotPtr = (Byte*)pSpan + MemoryMarshalCompat.PinnableOffset;
		ref Object pinnableSlotRef = ref Unsafe.AsRef<Object>(pinnableSlotPtr);
		pinnableSlotRef = pinnable;
	}
	/// <summary>
	/// Determines the byte offset of the <c>_pinnable</c> field in the runtime representation of
	/// <see cref="Span{T}"/>.
	/// </summary>
	/// <returns>
	/// The byte offset of the <c>_pinnable</c> field, or <c>-1</c> if the runtime uses the compact two-field
	/// span representation.
	/// </returns>
	/// <exception cref="PlatformNotSupportedException">
	/// The runtime uses a non-compact span representation whose <c>_pinnable</c> field cannot be identified.
	/// </exception>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private static Int32 ComputePinnableOffset()
	{
		if (sizeof(Span<Byte>) <= 2 * sizeof(IntPtr)) return -1;

		B2 buffer = new();
		Span<Object> arrays = MemoryMarshalCompat.CreateUnsafeSpan<Object>(&buffer, 2);

		arrays[0] = Array.Empty<Byte>();
		arrays[1] = Array.Empty<SByte>();

		GCHandle firstHandle = GCHandle.Alloc(arrays[0], GCHandleType.Pinned);
		GCHandle secondHandle = GCHandle.Alloc(arrays[1], GCHandleType.Pinned);
		try
		{
			Span<Byte> span0 = new(Unsafe.As<Object, Byte[]>(ref arrays[0]));
			Span<Byte> span1 = new(Unsafe.As<Object, Byte[]>(ref arrays[1]));
			return MemoryMarshalCompat.ComputePinnableOffset(arrays, in span0, in span1);
		}
		finally
		{
			secondHandle.Free();
			firstHandle.Free();
		}
	}
	/// <summary>
	/// Locates the <c>_pinnable</c> field by comparing the runtime memory representations of two spans backed by
	/// distinct pinned arrays.
	/// </summary>
	/// <param name="arrays">
	/// A two-element span containing the pinned array references used to construct <paramref name="span0"/> and
	/// <paramref name="span1"/>.
	/// </param>
	/// <param name="span0">A read-only reference to the first span used as a layout probe.</param>
	/// <param name="span1">
	/// A read-only reference to the second span used as a layout probe.
	/// </param>
	/// <returns>
	/// The zero-based byte offset of the <c>_pinnable</c> field within the runtime span representation.
	/// </returns>
	/// <exception cref="PlatformNotSupportedException">
	/// The runtime uses a non-compact span representation whose <c>_pinnable</c> field cannot be identified.
	/// </exception>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private static Int32 ComputePinnableOffset(Span<Object> arrays, in Span<Byte> span0, in Span<Byte> span1)
	{
		fixed (void* pSpan0 = &span0)
		fixed (void* pSpan = &span1)
			return MemoryMarshalCompat.ComputePinnableOffset(arrays, pSpan0, pSpan);
	}
	/// <summary>
	/// Locates the <c>_pinnable</c> field by comparing the runtime memory representations of two spans backed by
	/// distinct pinned arrays.
	/// </summary>
	/// <param name="arrays">
	/// A two-element span containing the pinned array references used to construct <paramref name="span0"/> and
	/// <paramref name="span1"/>.
	/// </param>
	/// <param name="span0">A read-only reference to the first read-only span used as a layout probe.</param>
	/// <param name="span1">A read-only reference to the second read-only span used as a layout probe.</param>
	/// <returns>
	/// The zero-based byte offset of the <c>_pinnable</c> field within the runtime span representation.
	/// </returns>
	/// <exception cref="PlatformNotSupportedException">
	/// The runtime uses a non-compact span representation whose <c>_pinnable</c> field cannot be identified.
	/// </exception>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private static Int32 ComputePinnableOffset(Span<Object> arrays, in ReadOnlySpan<Byte> span0,
		in ReadOnlySpan<Byte> span1)
	{
		fixed (void* pSpan0 = &span0)
		fixed (void* pSpan = &span1)
			return MemoryMarshalCompat.ComputePinnableOffset(arrays, pSpan0, pSpan);
	}
	/// <summary>
	/// Locates the <c>_pinnable</c> field by comparing the runtime memory representations of two spans backed by
	/// distinct pinned arrays.
	/// </summary>
	/// <param name="arrays">
	/// A two-element span containing the pinned array references used to construct <paramref name="pSpan0"/> and
	/// <paramref name="pSpan1"/>.
	/// </param>
	/// <param name="pSpan0">A pointer to the first span used as a layout probe.</param>
	/// <param name="pSpan1">A pointer to the second span used as a layout probe.</param>
	/// <returns>
	/// The zero-based byte offset of the <c>_pinnable</c> field within the runtime span representation.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private static Int32 ComputePinnableOffset(Span<Object> arrays, void* pSpan0, void* pSpan1)
	{
		ReadOnlySpan<Byte> spanXorSpan =
			MemoryMarshalCompat.GetSpanXorSpan(stackalloc Byte[sizeof(Span<Byte>)], pSpan0, pSpan1);
		IntPtr referenceXor = Unsafe.As<Object, IntPtr>(ref arrays[0]) ^ Unsafe.As<Object, IntPtr>(ref arrays[1]);
		Int32 result = spanXorSpan.IndexOf(new ReadOnlySpan<Byte>(&referenceXor, sizeof(IntPtr)));

		if (result >= 0) return result;
		IMessageResource resource = MessageResource.GetInstance();
		throw new PlatformNotSupportedException(resource.InvalidSpanLayout);
	}
	/// <summary>
	/// Determines the byte offset of the <c>_pinnable</c> field in the runtime representation of
	/// <see cref="Span{T}"/>.
	/// </summary>
	/// <returns>
	/// The byte offset of the <c>_pinnable</c> field, or <c>-1</c> if the runtime uses the compact two-field
	/// span representation.
	/// </returns>
	/// <exception cref="PlatformNotSupportedException">
	/// The runtime uses a non-compact span representation whose <c>_pinnable</c> field cannot be identified.
	/// </exception>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private static Int32 ComputeReadOnlyPinnableOffset()
	{
		B2 buffer = new();
		Span<Object> arrays = MemoryMarshalCompat.CreateUnsafeSpan<Object>(&buffer, 2);

		arrays[0] = Array.Empty<Byte>();
		arrays[1] = Array.Empty<SByte>();

		GCHandle firstHandle = GCHandle.Alloc(arrays[0], GCHandleType.Pinned);
		GCHandle secondHandle = GCHandle.Alloc(arrays[1], GCHandleType.Pinned);
		try
		{
			ReadOnlySpan<Byte> span0 = new(Unsafe.As<Object, Byte[]>(ref arrays[0]));
			ReadOnlySpan<Byte> span1 = new(Unsafe.As<Object, Byte[]>(ref arrays[1]));
			return MemoryMarshalCompat.ComputePinnableOffset(arrays, in span0, in span1);
		}
		finally
		{
			secondHandle.Free();
			firstHandle.Free();
		}
	}
	/// <summary>
	/// Computes the bytewise exclusive OR of two span memory representations.
	/// </summary>
	/// <param name="spanXor">
	/// The destination buffer that receives the bytewise XOR result. Its length determines the number of bytes read
	/// from each memory representation.
	/// </param>
	/// <param name="firstSpanPtr">A pointer to the first span memory representation.</param>
	/// <param name="secondSpanPtr">A pointer to the second span memory representation.</param>
	/// <returns>
	/// A read-only view of <paramref name="spanXor"/> containing the computed XOR bytes.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ReadOnlySpan<Byte> GetSpanXorSpan(Span<Byte> spanXor, void* firstSpanPtr, void* secondSpanPtr)
	{
		ReadOnlySpan<Byte> firstSpanBytes = new(firstSpanPtr, spanXor.Length);
		ReadOnlySpan<Byte> secondSpanBytes = new(secondSpanPtr, spanXor.Length);
		for (Int32 index = 0; index < spanXor.Length; index++)
			spanXor[index] = (Byte)(firstSpanBytes[index] ^ secondSpanBytes[index]);
		return spanXor;
	}
#pragma warning restore CS8500
}
#endif