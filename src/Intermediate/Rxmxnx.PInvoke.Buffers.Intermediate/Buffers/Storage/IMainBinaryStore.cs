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
	ref BufferTypeMetadata<T>? this[Int32 index] { get; }
	/// <summary>
	/// The number of slots required.
	/// </summary>
	Int32 SlotCount => BuffersHelper.GetLeadingZeros(this.Length);
#if !PACKAGE
	/// <summary>
	/// Initial storage span.
	/// </summary>
	Span<BufferTypeMetadata<T>?> Span { get; }
#endif
}