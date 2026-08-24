namespace Rxmxnx.PInvoke.Buffers.Storage;

/// <summary>
/// Represents the main binary store.
/// </summary>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal interface IMainBinaryStore<T>
{
	/// <summary>
	/// Initial storage length.
	/// </summary>
	UInt16 Length { get; }
	/// <summary>
	/// Gets the element at the specified zero-based index.
	/// </summary>
	/// <param name="index">The zero-based index of the element.</param>
#if !NET5_0_OR_GREATER
	BufferTypeMetadata<T>? this[Int32 index] { get; }
#else
	ref BufferTypeMetadata<T>? this[Int32 index] { get; }
#endif
	/// <summary>
	/// The number of slots required.
	/// </summary>
	Int32 SlotCount { get; }
#if !PACKAGE
	/// <summary>
	/// Initial storage span.
	/// </summary>
	Span<BufferTypeMetadata<T>?> Span { get; }
#endif
	/// <summary>
	/// Sets the element at the current instance.
	/// </summary>
	/// <param name="index">The zero-based index of the element.</param>
	/// <param name="component">The value that replaces the destination value.</param>
	/// <returns>The <paramref name="component"/> value.</returns>
	BufferTypeMetadata<T> Set(Int32 index, BufferTypeMetadata<T> component);
	/// <summary>
	/// Compares the original value and <paramref name="component"/> for reference equality and, if they are equal,
	/// replaces the first one, as an atomic operation.
	/// </summary>
	/// <param name="index">The zero-based index of the element.</param>
	/// <param name="component">
	/// The value that replaces the destination value if the comparison by reference results in equality.
	/// </param>
	/// <returns>The original value.</returns>
	BufferTypeMetadata<T>? CompareExchange(Int32 index, BufferTypeMetadata<T> component);
	/// <summary>
	/// Searches for the first available metadata entry in initial.
	/// </summary>
	/// <param name="start">Zero-based index of the first entry to inspect.</param>
	/// <param name="count">Number of entries to inspect.</param>
	/// <returns>
	/// The first available metadata entry within the specified range; otherwise, <see langword="null"/>.
	/// </returns>
	BufferTypeMetadata<T>? Search(Int32 start, Int32 count);
}