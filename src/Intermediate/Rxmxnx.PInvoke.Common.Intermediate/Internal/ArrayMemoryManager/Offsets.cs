#if !NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

// ReSharper disable once ClassCannotBeInstantiated
internal partial class ArrayMemoryManager<T>
{
	/// <summary>
	/// Internal offset storage.
	/// </summary>
	[FixedAddressValueType]
	private static ArrayOffsets arrayOffsets;

	/// <summary>
	/// Retrieves the array offset for given array.
	/// </summary>
	/// <param name="array">A <see cref="Array"/> instance.</param>
	/// <returns>A managed reference to the array offset.</returns>
	private static ref IntPtr? GetArrayOffset(Array array)
	{
		ref IntPtr? r0 = ref Unsafe.As<ArrayOffsets, IntPtr?>(ref ArrayMemoryManager<T>.arrayOffsets);
		return ref Unsafe.Add(ref r0, array.Rank - 2);
	}
}
#endif