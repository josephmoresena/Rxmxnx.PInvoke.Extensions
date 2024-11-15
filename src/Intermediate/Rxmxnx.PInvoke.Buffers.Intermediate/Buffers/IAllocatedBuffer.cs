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
	/// Indicates whether current type is binary space.
	/// </summary>
	internal static abstract Boolean IsBinary { get; }
	/// <summary>
	/// Buffer metadata.
	/// </summary>
	private protected static abstract IBufferTypeMetadata<T> Metadata { get; }

	/// <summary>
	/// Appends all components from current type.
	/// </summary>
	/// <param name="components">A dictionary of components.</param>
	internal static abstract void AppendComponent(IDictionary<UInt16, IBufferTypeMetadata<T>> components);

	/// <summary>
	/// Retrieves the <see cref="IBufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>The <see cref="IBufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public static IBufferTypeMetadata<T> GetMetadata<TBuffer>() where TBuffer : struct, IAllocatedBuffer<T>
		=> TBuffer.Metadata;

	/// <summary>
	/// Appends all components from <typeparamref name="TBuffer"/> type.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <param name="components">A dictionary of components.</param>
	private protected static void AppendComponent<TBuffer>(IDictionary<UInt16, IBufferTypeMetadata<T>> components)
		where TBuffer : struct, IAllocatedBuffer<T>
	{
		UInt16 key = TBuffer.Metadata.Size;
		if (TBuffer.IsBinary && components.TryAdd(key, TBuffer.Metadata))
			TBuffer.AppendComponent(components);
	}
}