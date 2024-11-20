namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a safe representation of scoped buffer of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
public readonly ref struct ScopedBuffer<T>
{
	/// <summary>
	/// Indicates whether current buffer is heap allocated.
	/// </summary>
	private readonly Boolean _heapAllocated;

	/// <summary>
	/// Current buffer span.
	/// </summary>
	public Span<T> Span { get; }
	/// <summary>
	/// Indicates whether current buffer is stack allocated.
	/// </summary>
	public Boolean InStack => !this._heapAllocated;
	/// <summary>
	/// Allocated buffer full length.
	/// </summary>
	public Int32 FullLength { get; }

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="span">Buffer span.</param>
	/// <param name="heapAllocated">Indicates whether current buffer is heap allocated.</param>
	/// <param name="fullLength">Allocated buffer full length.</param>
	internal ScopedBuffer(Span<T> span, Boolean heapAllocated, Int32 fullLength)
	{
		this.Span = span;
		this._heapAllocated = heapAllocated;
		this.FullLength = fullLength;
	}

	/// <summary>
	/// Defines an implicit conversion of a given span to <see cref="ScopedBuffer{T}"/>.
	/// </summary>
	/// <param name="span">A <typeparamref name="T"/> span to implicitly convert.</param>
	public static implicit operator ScopedBuffer<T>(Span<T> span) => new(span, true, span.Length);
}