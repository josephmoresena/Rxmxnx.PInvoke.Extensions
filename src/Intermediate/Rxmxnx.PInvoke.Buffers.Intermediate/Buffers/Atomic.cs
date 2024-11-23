namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// Primordial buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#pragma warning disable CA2252
[StructLayout(LayoutKind.Sequential)]
public struct Atomic<T> : IManagedBuffer<T>
{
	/// <summary>
	/// Internal metadata.
	/// </summary>
	private static readonly BufferTypeMetadata<T> typeMetadata = new BufferTypeMetadata<Atomic<T>, T>(1);

	/// <summary>
	/// Internal value.
	/// </summary>
	private T _val0;

	static BufferTypeMetadata<T> IManagedBuffer<T>.TypeMetadata => Atomic<T>.typeMetadata;
	static BufferTypeMetadata<T>[] IManagedBuffer<T>.Components => [];

	static void IManagedBuffer<T>.AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components) { }
	static void IManagedBuffer<T>.Append<TBuffer>(IDictionary<UInt16, BufferTypeMetadata<T>> components)
	{
		UInt16 initialSize = TBuffer.TypeMetadata.Size;
		components.TryAdd(initialSize, TBuffer.TypeMetadata);
	}
}
#pragma warning restore CA2252