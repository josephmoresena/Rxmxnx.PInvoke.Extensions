#if NET6_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

internal partial class ArrayMemoryManager<T>
{
	/// <inheritdoc cref="MemoryManager{T}.Memory"/>
	/// <param name="array">An <see cref="Array"/> instance.</param>
	public static Memory<T> GetMemory(Array? array)
		=> array is not null ? new ArrayMemoryManager<T>(array).Memory : Memory<T>.Empty;
	/// <inheritdoc cref="MemoryManager{T}.GetSpan()"/>
	/// <param name="array">An <see cref="Array"/> instance.</param>
	public static Span<T> GetSpan(Array? array)
	{
		if (array is null) return default;
		ref T managedRef = ref ArrayMemoryManager<T>.GetArrayDataReference(array);
		Span<T> span = MemoryMarshal.CreateSpan(ref managedRef, array.Length);
		return span;
	}

	/// <summary>
	/// Returns a reference to the 0th element of <paramref name="array"/>.
	/// </summary>
	/// <returns>A reference to the 0th element of array.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref T GetArrayDataReference(Array array)
	{
		ref Byte byteRef = ref MemoryMarshal.GetArrayDataReference(array);
		return ref Unsafe.As<Byte, T>(ref byteRef);
	}
}
#endif