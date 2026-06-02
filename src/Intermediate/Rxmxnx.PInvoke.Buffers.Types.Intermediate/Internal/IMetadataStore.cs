namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Exposes a buffer metadata store.
/// </summary>
internal interface IMetadataStore
{
	/// <summary>
	/// Tries to add the current component
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="component">The <see cref="BufferTypeMetadata{T}"/> instance to add.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="component"/> was added successfully; otherwise, <see langword="false"/>.
	/// </returns>
	Boolean TryAdd<T>(BufferTypeMetadata<T> component);
	/// <summary>
	/// Retrieves metadata required for a buffer with <paramref name="count"/> items.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	BufferTypeMetadata<T>? GetMetadata<T>(UInt16 count);
	/// <summary>
	/// Prepares internal metadata cache for allocations of <paramref name="count"/> items.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <exception cref="InvalidOperationException">Throw if missing metadata for any buffer component.</exception>
	void PrepareBinaryMetadata<T>(UInt16 count);
	/// <summary>
	/// Registers buffer type.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	void RegisterBuffer<T, [DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer>()
		where TBuffer : struct, IManagedBuffer<T>;
	/// <summary>
	/// Adds <paramref name="typeMetadata"/> to binary cache.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	/// <returns><paramref name="typeMetadata"/>.</returns>
	[return: NotNullIfNotNull(nameof(typeMetadata))]
	BufferTypeMetadata<T>? AddBinaryMetadata<T>(BufferTypeMetadata<T>? typeMetadata);
#if !PACKAGE
	/// <summary>
	/// Prints metadata dictionary.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="trace">Indicates whether trace should be written.</param>
	void PrintMetadata<T>(Boolean trace);
#endif
}