namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// Primordial buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#pragma warning disable CA2252
[StructLayout(LayoutKind.Sequential)]
public struct Primordial<T> : IAllocatedBuffer<T>
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private T _val0;

	/// <inheritdoc/>
	public static Int32 Capacity => 1;

	static void IAllocatedBuffer<T>.AppendComponent(IDictionary<UInt16, IBufferTypeMetadata<T>> component) { }
}
#pragma warning restore CA2252