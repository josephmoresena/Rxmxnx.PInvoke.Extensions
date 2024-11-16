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
	/// Internal metadata.
	/// </summary>
	private static readonly IBufferTypeMetadata<T> metadata = new BufferTypeMetadata<Primordial<T>, T>(1);

	/// <summary>
	/// Internal value.
	/// </summary>
	private T _val0;

	static Boolean IAllocatedBuffer<T>.IsPure => true;
	static Boolean IAllocatedBuffer<T>.IsBinary => true;
	static IBufferTypeMetadata<T> IAllocatedBuffer<T>.Metadata => Primordial<T>.metadata;

	static void IAllocatedBuffer<T>.AppendComponent(IDictionary<UInt16, IBufferTypeMetadata<T>> components) { }
	static void IAllocatedBuffer<T>.Append<TBuffer>(IDictionary<UInt16, IBufferTypeMetadata<T>> components)
	{
		UInt16 initialSize = TBuffer.Metadata.Size;
		components.TryAdd(initialSize, TBuffer.Metadata);
	}
	static IBufferTypeMetadata<T>[] IAllocatedBuffer<T>.Components => [];
}
#pragma warning restore CA2252