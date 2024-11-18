namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// This interfaces exposes an allocated buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#if NET6_0
[RequiresPreviewFeatures]
#endif
public interface IManagedBuffer<T>
{
	/// <summary>
	/// Indicates whether current type is pure.
	/// </summary>
	internal static abstract Boolean IsPure { get; }
	/// <summary>
	/// Indicates whether current type is binary space.
	/// </summary>
	internal static abstract Boolean IsBinary { get; }
	/// <summary>
	/// Current type components.
	/// </summary>
	internal static abstract ManagedBufferMetadata<T>[] Components { get; }
	/// <summary>
	/// Buffer metadata.
	/// </summary>
	internal static abstract ManagedBufferMetadata<T> Metadata { get; }

	/// <summary>
	/// Appends all components from current type.
	/// </summary>
	/// <param name="components">A dictionary of components.</param>
	internal static abstract void AppendComponent(IDictionary<UInt16, ManagedBufferMetadata<T>> components);
	/// <summary>
	/// Appends a composed buffer.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the composed buffer.</typeparam>
	/// <param name="components">A dictionary of components.</param>
	internal static abstract void Append<TBuffer>(IDictionary<UInt16, ManagedBufferMetadata<T>> components)
		where TBuffer : struct, IManagedBuffer<T>;

	/// <summary>
	/// Appends all components from <typeparamref name="TBuffer"/> type.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <param name="components">A dictionary of components.</param>
	private protected static void AppendComponent<TBuffer>(IDictionary<UInt16, ManagedBufferMetadata<T>> components)
		where TBuffer : struct, IManagedBuffer<T>
	{
		if (TBuffer.IsBinary)
			IManagedBuffer<T>.AppendComponent(TBuffer.Metadata, components);
	}

	/// <summary>
	/// Retrieves the <see cref="ManagedBufferMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>The <see cref="ManagedBufferMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public static ManagedBufferMetadata<T> GetMetadata<TBuffer>() where TBuffer : struct, IManagedBuffer<T>
		=> TBuffer.Metadata;

	/// <summary>
	/// Appends all components from <paramref name="component"/> instance.
	/// </summary>
	/// <param name="component">A <see cref="ManagedBufferMetadata{T}"/> instance.</param>
	/// <param name="components">A dictionary of components.</param>
	private static void AppendComponent(ManagedBufferMetadata<T> component,
		IDictionary<UInt16, ManagedBufferMetadata<T>> components)
	{
		if (!components.TryAdd(component.Size, component)) return;
		foreach (ManagedBufferMetadata<T> metadataComponent in component.Components.AsSpan())
			IManagedBuffer<T>.AppendComponent(metadataComponent, components);
	}
}