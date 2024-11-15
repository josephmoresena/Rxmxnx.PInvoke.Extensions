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

	static IBufferTypeMetadata<T> IAllocatedBuffer<T>.Metadata => Primordial<T>.metadata;
	static void IAllocatedBuffer<T>.AppendComponent(IDictionary<UInt16, IBufferTypeMetadata<T>> components) { }
}
#pragma warning restore CA2252