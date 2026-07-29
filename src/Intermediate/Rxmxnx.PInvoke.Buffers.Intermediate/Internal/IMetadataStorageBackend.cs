namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Defines the internal operations required to access metadata storage.
/// </summary>
/// <remarks>
/// Implementations must be read-only structs to enable devirtualization of calls and avoid relying on generic math
/// abstractions.
/// </remarks>
internal interface IMetadataStorageBackend
{
	/// <summary>
	/// Maximum storage capacity.
	/// </summary>
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	Int32 MaxStorageCapacity => UInt16.MaxValue;
#else
	Int32 MaxStorageCapacity { get; }
#endif

	/// <summary>
	/// Tries to add the current component
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="component">The <see cref="BufferTypeMetadata{T}"/> instance to add.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="component"/> was added successfully; otherwise, <see langword="false"/>.
	/// </returns>
	Boolean TryAdd<T>(BufferTypeMetadata<T> component);
	/// <inheritdoc cref="BinaryStore{TInitial,T}.CurrentCapacity"/>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <returns>The current capacity for <typeparamref name="T"/>.</returns>
	Int32 GetCurrentCapacity<T>();
	/// <summary>
	/// Retrieves a managed reference to the <see cref="BufferTypeMetadata{T}"/> instance for <paramref name="componentSize"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="componentSize">Size of the requested metadata.</param>
	/// <returns>A managed <see cref="BufferTypeMetadata{T}"/> reference.</returns>
	ref BufferTypeMetadata<T>? GetBinaryReference<T>(UInt16 componentSize);
	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance for <paramref name="componentSize"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="componentSize">Size of the requested metadata.</param>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	BufferTypeMetadata<T>? GetBinaryValue<T>(UInt16 componentSize);
	/// <summary>
	/// Computes the binary metadata required for a buffer with <paramref name="count"/> items.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="storage">A <see cref="MetadataStorage"/> instance.</param>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <param name="nonBinaryMinimal">Indicates the value fo the non-binary buffer minimal.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	BufferTypeMetadata<T>? ComputeBinaryMetadata<T>(MetadataStorage storage, UInt16 count, Int32 nonBinaryMinimal);
	/// <summary>
	/// Retrieves the fundamental component of size <paramref name="space"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="storage">A <see cref="MetadataStorage"/> instance.</param>
	/// <param name="space">Size of fundamental component.</param>
	/// <returns>A <see cref="BufferTypeMetadata"/> instance.</returns>
	BufferTypeMetadata<T>? GetFundamental<T>(MetadataStorage storage, UInt16 space);
#if !PACKAGE
	/// <inheritdoc cref="BinaryStore{TInitial,T}.Initial"/>
	ReadOnlySpan<BufferTypeMetadata<T>?> GetInitial<T>();
	/// <inheritdoc cref="BinaryStore{TInitial,T}.Slots"/>
	ReadOnlySpan<BufferTypeMetadata<T>?[]?> GetSlots<T>();
#endif
}