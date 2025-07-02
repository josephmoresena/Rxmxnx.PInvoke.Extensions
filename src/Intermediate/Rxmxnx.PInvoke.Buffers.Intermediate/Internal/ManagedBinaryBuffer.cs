namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for <see cref="IManagedBinaryBuffer{T}"/> implementations.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
internal static class ManagedBinaryBuffer<T>
{
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
	public static BufferTypeMetadata<T>? GetMetadata(
#if NET5_0_OR_GREATER
		[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)]
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