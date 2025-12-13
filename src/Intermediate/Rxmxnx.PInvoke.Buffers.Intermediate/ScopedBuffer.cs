namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a safe representation of scoped buffer of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
public readonly ref struct ScopedBuffer<T>
{
	/// <summary>
	/// Current buffer span.
	/// </summary>
	public Span<T> Span { get; }
	/// <summary>
	/// Indicates whether current buffer is stack allocated.
	/// </summary>
	public Boolean InStack => !field;
	/// <summary>
	/// Allocated buffer full length.
	/// </summary>
	public Int32 FullLength { get; }
	/// <summary>
	/// Metadata used for allocation.
	/// </summary>
	public BufferTypeMetadata? BufferMetadata { get; }

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="span">Buffer span.</param>
	/// <param name="heapAllocated">Indicates whether current buffer is heap allocated.</param>
	/// <param name="fullLength">Allocated buffer full length.</param>
	/// <param name="metadata">Allocated buffer metadata.</param>
	internal ScopedBuffer(Span<T> span, Boolean heapAllocated, Int32 fullLength, BufferTypeMetadata? metadata = default)
	{
		this.Span = span;
		this.InStack = heapAllocated;
		this.FullLength = fullLength;
		this.BufferMetadata = metadata;
	}
}