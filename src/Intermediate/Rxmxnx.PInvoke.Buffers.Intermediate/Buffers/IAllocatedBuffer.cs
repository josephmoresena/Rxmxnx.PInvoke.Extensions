namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// This interfaces exposes an allocated buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#if NET6_0
[RequiresPreviewFeatures]
#endif
public interface IAllocatedBuffer<T>
{
	/// <summary>
	/// Buffer capacity.
	/// </summary>
	internal static abstract Int32 Capacity { get; }
	internal static abstract void AppendComponent(IDictionary<UInt16, IBufferTypeMetadata<T>> component);

	private protected static void AppendComponent<TBuffer>(IDictionary<UInt16, IBufferTypeMetadata<T>> component)
		where TBuffer : struct, IAllocatedBuffer<T>
	{
		UInt16 key = (UInt16)TBuffer.Capacity;
		if (component.ContainsKey(key)) return;
		component.Add(key, new BufferTypeMetadata<TBuffer, T>());
		TBuffer.AppendComponent(component);
	}
}