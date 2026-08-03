#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
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
	/// Retrieves the offsets of the fields in any instance of <see cref="Span{T}"/>
	/// </summary>
	private static Int32 PinnableOffset
		=> MemoryMarshalCompat.pinnableOffset ??= MemoryMarshalCompat.ComputePinnableOffset();

#pragma warning disable CS8500
	/// <summary>
	/// Sets the <c>_pinnable</c> field value.
	/// </summary>
	/// <param name="pinnable">The managed pinnable instance.</param>
	/// <param name="pSpan">Pointer to the span layout.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void SetPinnableField(Object pinnable, void* pSpan)
	{
		Byte* pinnableSlotPtr = (Byte*)pSpan + MemoryMarshalCompat.PinnableOffset;
		ref Object pinnableSlotRef = ref Unsafe.AsRef<Object>(pinnableSlotPtr);
		pinnableSlotRef = pinnable;
	}
	/// <summary>
	/// Computes the byte offset of the <c>_pinnable</c> field in the three-field <see cref="Span{T}"/> representation.
	/// </summary>
	/// <returns>
	/// The byte offset of the <c>_pinnable</c> field, or <c>-1</c> when the runtime uses the two-field span
	/// representation.
	/// </returns>
	/// <exception cref="PlatformNotSupportedException">
	/// The runtime uses a non-fast implementation, but the <c>_pinnable</c> field could not be identified.
	/// </exception>
	private static Int32 ComputePinnableOffset()
	{
		Int32 spanSize = sizeof(Span<Byte>);
		if (spanSize <= 2 * sizeof(IntPtr)) return -1;

		B2 buffer = new();
		Span<Object> arrays = MemoryMarshalCompat.CreateUnsafeSpan<Object>(&buffer, 2);

		arrays[0] = Array.Empty<Byte>();
		arrays[1] = Array.Empty<SByte>();

		GCHandle firstHandle = GCHandle.Alloc(arrays[0], GCHandleType.Pinned);
		GCHandle secondHandle = GCHandle.Alloc(arrays[1], GCHandleType.Pinned);
		try
		{
			Span<Byte> firstSpan = new(Unsafe.As<Object, Byte[]>(ref arrays[0]));
			Span<Byte> secondSpan = new(Unsafe.As<Object, Byte[]>(ref arrays[1]));
			ref Span<Byte> firstSpanRef = ref firstSpan;
			ref Span<Byte> secondSpanRef = ref secondSpan;
			fixed (void* firstSpanPtr = &firstSpanRef)
			fixed (void* secondSpanPtr = &secondSpanRef)
			{
				ReadOnlySpan<Byte> spanXorSpan =
					MemoryMarshalCompat.GetSpanXorSpan(stackalloc Byte[spanSize], firstSpanPtr, secondSpanPtr);
				IntPtr referenceXor =
					Unsafe.As<Object, IntPtr>(ref arrays[0]) ^ Unsafe.As<Object, IntPtr>(ref arrays[1]);
				Int32 result = spanXorSpan.IndexOf(new ReadOnlySpan<Byte>(&referenceXor, sizeof(IntPtr)));
				if (result < 0)
					//TODO: ValidationUtilities
					throw new PlatformNotSupportedException(
						"Unable to identify the pinnable field in the Span<T> layout.");
				return result;
			}
		}
		finally
		{
			secondHandle.Free();
			firstHandle.Free();
		}
	}
	/// <summary>
	/// Computes the bytewise XOR of two span memory representations and writes the result to the provided buffer.
	/// </summary>
	/// <param name="spanXor">The destination buffer that receives the XOR result.</param>
	/// <param name="firstSpanPtr">Pointer to the first span memory representation.</param>
	/// <param name="secondSpanPtr">Pointer to the second span memory representation.</param>
	/// <returns>
	/// A read-only view of <paramref name="spanXor"/> containing the computed XOR bytes.
	/// </returns>
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