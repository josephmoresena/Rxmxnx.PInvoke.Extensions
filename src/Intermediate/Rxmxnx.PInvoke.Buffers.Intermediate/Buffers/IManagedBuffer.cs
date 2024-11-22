namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// This interfaces exposes an allocated buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2743,
                 Justification = SuppressMessageConstants.StaticAbstractPropertyUseJustification)]
public interface IManagedBuffer<T>
{
	/// <summary>
	/// Indicates whether current type is pure.
	/// </summary>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	internal static abstract Boolean IsPure { get; }
	/// <summary>
	/// Indicates whether current type is binary space.
	/// </summary>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	internal static abstract Boolean IsBinary { get; }
	/// <summary>
	/// Current type components.
	/// </summary>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	internal static abstract BufferTypeMetadata<T>[] Components { get; }
	/// <summary>
	/// Buffer metadata.
	/// </summary>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	internal static abstract BufferTypeMetadata<T> TypeMetadata { get; }

	/// <summary>
	/// Appends all components from current type.
	/// </summary>
	/// <param name="components">A dictionary of components.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	internal static abstract void AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components);
	/// <summary>
	/// Appends a composed buffer.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the composed buffer.</typeparam>
	/// <param name="components">A dictionary of components.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	internal static abstract void Append<TBuffer>(IDictionary<UInt16, BufferTypeMetadata<T>> components)
		where TBuffer : struct, IManagedBuffer<T>;

	/// <summary>
	/// Appends all components from <typeparamref name="TBuffer"/> type.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <param name="components">A dictionary of components.</param>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	private protected static void AppendComponent<TBuffer>(IDictionary<UInt16, BufferTypeMetadata<T>> components)
		where TBuffer : struct, IManagedBuffer<T>
	{
		if (TBuffer.IsBinary)
			IManagedBuffer<T>.AppendComponent(TBuffer.TypeMetadata, components);
	}

	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.</returns>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public static BufferTypeMetadata<T> GetMetadata<TBuffer>() where TBuffer : struct, IManagedBuffer<T>
		=> TBuffer.TypeMetadata;

	/// <summary>
	/// Appends all components from <paramref name="component"/> instance.
	/// </summary>
	/// <param name="component">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	/// <param name="components">A dictionary of components.</param>
	private static void AppendComponent(BufferTypeMetadata<T> component,
		IDictionary<UInt16, BufferTypeMetadata<T>> components)
	{
		if (!components.TryAdd(component.Size, component)) return;
		foreach (BufferTypeMetadata<T> metadataComponent in component.Components.AsSpan())
			IManagedBuffer<T>.AppendComponent(metadataComponent, components);
	}
}