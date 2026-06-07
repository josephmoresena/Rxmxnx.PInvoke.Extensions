namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal ref-struct for binary type metadata instances.
/// </summary>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal readonly ref struct BinaryMap<T>
{
	/// <summary>
	/// Internal binary type metadata span.
	/// </summary>
	private readonly Span<BufferTypeMetadata<T>?> _initial;
	/// <summary>
	/// Additional binary type metadata slots.
	/// </summary>
	private readonly Span<BufferTypeMetadata<T>?[]?> _slots;

	/// <summary>
	/// Gets the element at the specified size.
	/// </summary>
	/// <param name="size">The requested buffer type size.</param>
	/// <returns>The element at the specified index.</returns>
	public ref BufferTypeMetadata<T>? this[UInt16 size]
	{
		get
		{
			Debug.Assert(size > 0);
			if (size <= this._initial.Length)
				return ref this._initial[size - 1];
			ref BufferTypeMetadata<T>?[]? page = ref MemoryMarshal.GetReference(this._slots);
			Int32 acc = this._initial.Length + 1;
			do
			{
				Debug.Assert(page is not null);
				if (size < page.Length + acc) break;
				page = ref Unsafe.Add(ref page, 1)!;
				acc *= 2;
			} while (size > acc);
			return ref Unsafe.Add(ref MemoryMarshal.GetReference(page), size - acc);
		}
	}
#if NET8_0_OR_GREATER
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="instance">A <see cref="MetadataStorage{T}"/> instance.</param>
	public BinaryMap(MetadataStorage<T> instance) : this(
		MemoryMarshal.CreateSpan(ref instance.MetadataReference, instance.Capacity),
		instance is G2047<T> g2047 ? g2047.Slots.AsSpan() : []) { }
#endif
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="initial">Initial 2^N-1 span.</param>
	/// <param name="slots">Slots 2^N..2^15 span.</param>
	public BinaryMap(Span<BufferTypeMetadata<T>?> initial, Span<BufferTypeMetadata<T>?[]?> slots)
	{
		this._initial = initial;
		this._slots = slots;
	}

	/// <summary>
	/// Indicates whether the current instance allows binary buffers with <see langword="count"/> size.
	/// </summary>
	/// <param name="count">Requested size.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="count"/> is allowed on the current instance; otherwise;
	/// <see langword="false"/>.
	/// </returns>
	public Boolean IsAllowed(UInt16 count) => BinaryMap<T>.IsAllowed(count, (UInt16)this._initial.Length, this._slots);
	/// <summary>
	/// Retrieves the minimal buffer metadata registered to hold at least <paramref name="count"/> items.
	/// </summary>
	/// <param name="count">Minimal number of items in buffer.</param>
	/// <returns>A <see cref="BufferTypeMetadata"/> instance.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS907)]
#endif
	public BufferTypeMetadata<T>? GetMinimal(UInt16 count)
	{
		Debug.Assert(count > 1);
		if (count >= this._initial.Length && this._slots.IsEmpty)
			return default;

		Int32 pageIndex = -1;
		Int32 relativeIndex = count;
		ReadOnlySpan<BufferTypeMetadata<T>?> span = this._initial;
		// Get the initial page. Initial span is the -1 page.
		while (relativeIndex >= span.Length)
		{
			relativeIndex -= span.Length;
			pageIndex++;
			if (!this.TryGetPage(pageIndex, out span))
				// Initial page unavailable.
				return default;
		}
		// Total elements to check in the range: (count, 2 * count).
		Int32 remaining = count - 1;
		while (remaining > 0)
		{
			// Search at the current page.
			Int32 length = Math.Min(span.Length - relativeIndex, remaining);
			if (BinaryMap<T>.Search(span, relativeIndex, length) is { } result)
				// Minimal metadata found.
				return result;
			// Exclude from total elements the current search length.
			if ((remaining -= length) <= 0) continue;
			// Get the next page.
			pageIndex++;
			if (!this.TryGetPage(pageIndex, out span))
				// Next page unavailable.
				return default;
			relativeIndex = 0;
		}
		return default;
	}

	/// <summary>
	/// Attempts to retrieve a metadata storage page.
	/// </summary>
	/// <param name="pageIndex">
	/// Requested page index. A value lower than zero resolves to the initial page stored in
	/// <see cref="_initial"/> (page <c>-1</c>). Non-negative values resolve to pages stored in <see cref="_slots"/>.
	/// </param>
	/// <param name="span">
	/// Receives the span associated with the requested page when available; otherwise, an empty span.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the requested page exists and is available; otherwise, <see langword="false"/>.
	/// </returns>
	private Boolean TryGetPage(Int32 pageIndex, out ReadOnlySpan<BufferTypeMetadata<T>?> span)
	{
		if (pageIndex < 0)
		{
			span = this._initial;
			return true;
		}
		if ((UInt32)pageIndex >= (UInt32)this._slots.Length || this._slots[pageIndex] is not { } page)
		{
			span = default;
			return false;
		}
		span = page;
		return true;
	}

	/// <summary>
	/// Indicates whether the current instance allows binary buffers with <see langword="count"/> size.
	/// </summary>
	/// <param name="count">Requested size.</param>
	/// <param name="capacity">Current capacity.</param>
	/// <param name="slots">Slots span.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="count"/> is allowed on the current instance; otherwise;
	/// <see langword="false"/>.
	/// </returns>
	private static Boolean IsAllowed(UInt16 count, UInt16 capacity, Span<BufferTypeMetadata<T>?[]?> slots)
	{
		Debug.Assert(count > 0);
		if (slots.IsEmpty || slots[0] is null) return count <= capacity;
		UInt32 limit = capacity;
		foreach (BufferTypeMetadata<T>?[]? page in slots)
		{
			if (page is null) break;
			limit += (UInt32)page.Length;
			if (limit >= UInt16.MaxValue) return true;
		}
		return count <= limit;
	}
	/// <summary>
	/// Searches for the first available metadata entry in a page segment.
	/// </summary>
	/// <param name="span">Metadata page to search.</param>
	/// <param name="start">Zero-based index of the first entry to inspect.</param>
	/// <param name="count">Number of entries to inspect.</param>
	/// <returns>
	/// The first available metadata entry within the specified range; otherwise, <see langword="null"/>.
	/// </returns>
	private static BufferTypeMetadata<T>? Search(ReadOnlySpan<BufferTypeMetadata<T>?> span, Int32 start, Int32 count)
	{
		foreach (BufferTypeMetadata<T>? val in span.Slice(start, count))
		{
			if (val is not null)
				return val;
		}
		return default;
	}
}