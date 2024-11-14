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
	/// Buffer metadata.
	/// </summary>
	private protected static abstract IBufferTypeMetadata<T> Metadata { get; }

	internal static abstract void AppendComponent(IDictionary<UInt16, IBufferTypeMetadata<T>> component);

	private protected static void AppendComponent<TBuffer>(IDictionary<UInt16, IBufferTypeMetadata<T>> component)
		where TBuffer : struct, IAllocatedBuffer<T>
	{
		UInt16 key = TBuffer.Metadata.Size;
		if (component.TryAdd(key, TBuffer.Metadata))
			TBuffer.AppendComponent(component);
	}

	internal static IBufferTypeMetadata<T> GetMetadata<TBuffer>() where TBuffer : struct, IAllocatedBuffer<T>
		=> TBuffer.Metadata;
}