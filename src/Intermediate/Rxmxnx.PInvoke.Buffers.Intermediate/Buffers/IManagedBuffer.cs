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
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
	[Browsable(false)]
#endif
	internal BufferTypeMetadata<T> GetStaticTypeMetadata();

#if NET7_0_OR_GREATER
	/// <summary>
	/// Buffer type metadata.
	/// </summary>
	private protected static abstract BufferTypeMetadata<T> TypeMetadata { get; }

	/// <summary>
	/// Appends all components from the current type.
	/// </summary>
	/// <param name="storage">An <see cref="IMetadataStorage"/> instance.</param>
	internal static abstract void AppendComponent(IMetadataStorage storage);
	/// <summary>
	/// Appends all components from <typeparamref name="TBuffer"/> type.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <param name="storage">An <see cref="IMetadataStorage"/> instance.</param>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
#if !PACKAGE && !NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	private protected static void AppendComponent<TBuffer>(IMetadataStorage storage)
		where TBuffer : struct, IManagedBuffer<T>
	{
#if !NET7_0_OR_GREATER
		BufferTypeMetadata<T> bufferTypeMetadata = BuffersHelper.GetStaticMetadata<T, TBuffer>();
#else
		BufferTypeMetadata<T> bufferTypeMetadata = TBuffer.TypeMetadata;
#endif
		if (bufferTypeMetadata.IsBinary)
			ManagedBuffer<T>.AppendComponent(bufferTypeMetadata, storage);
	}
#endif

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance from <typeparamref name="TBuffer"/>.</returns>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public static BufferTypeMetadata<T> GetMetadata<TBuffer>() where TBuffer : struct, IManagedBuffer<T>
#if !NET7_0_OR_GREATER
		=> BuffersHelper.GetStaticMetadata<T, TBuffer>();
#else
		=> TBuffer.TypeMetadata;
#endif
#endif
}