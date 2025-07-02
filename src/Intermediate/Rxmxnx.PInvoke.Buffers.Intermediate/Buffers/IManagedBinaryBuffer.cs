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
	[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)]
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