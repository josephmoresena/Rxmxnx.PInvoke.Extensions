#if !NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

// ReSharper disable once ClassCannotBeInstantiated
internal partial class ArrayMemoryManager<T>
{
	/// <summary>
	/// Internal offset storage.
	/// </summary>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
	[FixedAddressValueType]
#endif
	// ReSharper disable once StaticMemberInGenericType
	private static ArrayOffsets arrayOffsets = ArrayOffsets.Create();

	/// <summary>
	/// Retrieves the array offset for given array.
	/// </summary>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	/// <returns>A managed reference to the array offset.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref IntPtr? GetArrayOffset(Array array)
	{
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
		ref IntPtr? r0 = ref Unsafe.As<ArrayOffsets, IntPtr?>(ref ArrayMemoryManager<T>.arrayOffsets);
		return ref Unsafe.Add(ref r0, array.Rank - 2);
#else
		ref IntPtr?[] rArray = ref Unsafe.As<ArrayOffsets, IntPtr?[]>(ref ArrayMemoryManager<T>.arrayOffsets);
		return ref rArray[array.Rank - 2];
#endif
	}
}
#endif