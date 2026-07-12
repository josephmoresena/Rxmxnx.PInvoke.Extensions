namespace Rxmxnx.PInvoke.Internal;

internal abstract partial class MetadataStorage
{
	/// <summary>
	/// Maximum storage capacity.
	/// </summary>
	protected virtual Int32 MaxStorageCapacity => UInt16.MaxValue;

	/// <inheritdoc cref="BinaryStore{TInitial,T}.CurrentCapacity"/>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <returns>The current capacity for <typeparamref name="T"/>.</returns>
	protected abstract Int32 GetCurrentCapacity<T>();
	/// <summary>
	/// Retrieves a managed reference to the <see cref="BufferTypeMetadata{T}"/> instance for <paramref name="componentSize"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="componentSize">Size of the requested metadata.</param>
	/// <returns>A managed <see cref="BufferTypeMetadata{T}"/> reference.</returns>
	protected abstract ref BufferTypeMetadata<T>? GetBinaryReference<T>(UInt16 componentSize);
	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance for <paramref name="componentSize"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="componentSize">Size of the requested metadata.</param>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	protected abstract BufferTypeMetadata<T>? GetBinaryValue<T>(UInt16 componentSize);
	/// <summary>
	/// Computes the binary metadata required for a buffer with <paramref name="count"/> items.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <param name="allowMinimal">Allow to return minimal buffer.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	protected abstract BufferTypeMetadata<T>? ComputeBinaryMetadata<T>(UInt16 count, Boolean allowMinimal);
	/// <summary>
	/// Retrieves the fundamental component of size <paramref name="space"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="space">Size of fundamental component.</param>
	/// <returns>A <see cref="BufferTypeMetadata"/> instance.</returns>
	protected abstract BufferTypeMetadata<T>? GetFundamental<T>(UInt16 space);

#if !PACKAGE
	/// <inheritdoc cref="BinaryStore{TInitial,T}.Initial"/>
	protected abstract ReadOnlySpan<BufferTypeMetadata<T>?> GetInitial<T>();
	/// <inheritdoc cref="BinaryStore{TInitial,T}.Slots"/>
	protected abstract ReadOnlySpan<BufferTypeMetadata<T>?[]?> GetSlots<T>();
#endif
}