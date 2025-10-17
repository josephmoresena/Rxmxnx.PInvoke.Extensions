namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// This interfaces exposes a managed buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2743)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2326)]
#endif
public interface IManagedBuffer<T>
{
	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance.
	/// </summary>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	internal BufferTypeMetadata<T> GetStaticTypeMetadata();

#if NET7_0_OR_GREATER
	/// <summary>
	/// Current type components.
	/// </summary>
	internal static abstract BufferTypeMetadata<T>[] Components { get; }
	/// <summary>
	/// Buffer type metadata.
	/// </summary>
	private protected static abstract BufferTypeMetadata<T> TypeMetadata { get; }

	/// <summary>
	/// Appends all components from current type.
	/// </summary>
	/// <param name="components">A dictionary of components.</param>
	internal static abstract void AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components);
#else
	/// <summary>
	/// Current type components.
	/// </summary>
	internal static BufferTypeMetadata<T>[] Components => [];
	/// <summary>
	/// Buffer type metadata.
	/// </summary>
	private protected static BufferTypeMetadata<T> TypeMetadata => default!;

	/// <summary>
	/// Appends all components from current type.
	/// </summary>
	/// <param name="components">A dictionary of components.</param>
	internal static void AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components) { }
#endif

	/// <summary>
	/// Appends all components from <typeparamref name="TBuffer"/> type.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <param name="components">A dictionary of components.</param>
	private protected static void AppendComponent<TBuffer>(IDictionary<UInt16, BufferTypeMetadata<T>> components)
		where TBuffer : struct, IManagedBuffer<T>
	{
		if (BufferManager.GetStaticMetadata<T, TBuffer>().IsBinary)
			ManagedBuffer<T>.AppendComponent(BufferManager.GetStaticMetadata<T, TBuffer>(), components);
	}

	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public static BufferTypeMetadata<T> GetMetadata<TBuffer>() where TBuffer : struct, IManagedBuffer<T>
		=> BufferManager.GetStaticMetadata<T, TBuffer>();
}