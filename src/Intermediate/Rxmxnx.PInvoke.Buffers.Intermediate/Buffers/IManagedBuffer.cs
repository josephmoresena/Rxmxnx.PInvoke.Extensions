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
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal static BufferTypeMetadata<T>[] Components => [];
	/// <summary>
	/// Buffer type metadata.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private protected static BufferTypeMetadata<T> TypeMetadata => default!;

	/// <summary>
	/// Appends all components from current type.
	/// </summary>
	/// <param name="components">A dictionary of components.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	// ReSharper disable once UnusedParameter.Global
	internal static void AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components)
	{
		// This method is declared for compatibility with .NET 7.0 APIs but is not usable. 
	}
#endif

	/// <summary>
	/// Appends all components from <typeparamref name="TBuffer"/> type.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <param name="components">A dictionary of components.</param>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
#if !PACKAGE && !NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	private protected static void AppendComponent<TBuffer>(IDictionary<UInt16, BufferTypeMetadata<T>> components)
		where TBuffer : struct, IManagedBuffer<T>
	{
#if !NET7_0_OR_GREATER
		BufferTypeMetadata<T> bufferTypeMetadata = BufferManager.GetStaticMetadata<T, TBuffer>();
#else
		BufferTypeMetadata<T> bufferTypeMetadata = TBuffer.TypeMetadata;
#endif
		if (bufferTypeMetadata.IsBinary)
			ManagedBuffer<T>.AppendComponent(bufferTypeMetadata, components);
	}

	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public static BufferTypeMetadata<T> GetMetadata<TBuffer>() where TBuffer : struct, IManagedBuffer<T>
#if !NET7_0_OR_GREATER
		=> BufferManager.GetStaticMetadata<T, TBuffer>();
#else
		=> TBuffer.TypeMetadata;
#endif
}