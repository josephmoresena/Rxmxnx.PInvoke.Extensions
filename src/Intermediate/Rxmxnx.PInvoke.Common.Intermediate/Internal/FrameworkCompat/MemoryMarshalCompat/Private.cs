#if !PACKAGE || !NET6_0_OR_GREATER
using System_IntPtr = System.IntPtr;
#if NETCOREAPP
using UIntPtr = nuint;
using IntPtr = nint;
#endif

namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal static unsafe partial class MemoryMarshalCompat
{
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
}
#endif