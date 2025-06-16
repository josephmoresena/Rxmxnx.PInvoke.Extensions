namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// This interfaces exposes a binary managed buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
public interface IManagedBinaryBuffer<T> : IManagedBuffer<T>
{
	/// <summary>
	/// Buffer type metadata.
	/// </summary>
	BufferTypeMetadata<T> Metadata { get; }

	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance from <paramref name="bufferType"/>.
	/// </summary>
	/// <param name="bufferType">Type of buffer</param>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance from <paramref name="bufferType"/>.</returns>
	/// <remarks>
	/// This method allocates in heap a <paramref name="bufferType"/> instance to retrieve the
	/// <see cref="BufferTypeMetadata{T}"/> instance.
	/// </remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal static BufferTypeMetadata<T>? GetMetadata(
#if NET5_0_OR_GREATER
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
		Type? bufferType)
	{
		if (bufferType is null) return default;
		try
		{
			// This allocates a buffer in heap temporally.
			IManagedBinaryBuffer<T> binaryBuffer = (IManagedBinaryBuffer<T>)Activator.CreateInstance(bufferType)!;
			return binaryBuffer.Metadata;
		}
		catch (Exception)
		{
			// ignored
		}
		return default;
	}
}

/// <summary>
/// This interfaces exposes a binary managed buffer.
/// </summary>
/// <typeparam name="TBuffer">The type of buffer.</typeparam>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS107)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
#endif
public partial interface IManagedBinaryBuffer<
#if NET5_0_OR_GREATER
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
#endif
	TBuffer, T> : IManagedBinaryBuffer<T> where TBuffer : struct, IManagedBinaryBuffer<TBuffer, T>
{
	BufferTypeMetadata<T> IManagedBinaryBuffer<T>.Metadata
#if !PACKAGE && NET6_0 || NET7_0_OR_GREATER
		=> IManagedBuffer<T>.GetMetadata<TBuffer>();
#else
		=> BufferManager.MetadataManager<T>.GetMetadata(typeof(TBuffer))!;
#endif
}