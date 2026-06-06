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
				if (Unsafe.IsNullRef(ref page) || page is null) return ref Unsafe.NullRef<BufferTypeMetadata<T>?>();
				if (size < page.Length + acc) break;
				page = ref Unsafe.Add(ref page, 1)!;
				acc *= 2;
			} while (size > acc);
			return ref page.AsSpan()[size - acc];
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
		Debug.Assert(count > 0);
		if (count >= this._initial.Length && this._slots.IsEmpty)
			return default;

		ReadOnlySpan<BufferTypeMetadata<T>?> binarySpan;
		Int32 relativeIndex = count;

		if (count < this._initial.Length)
		{
			binarySpan = this._initial;
			goto Search;
		}

		Int32 acc = this._initial.Length + 1;
		foreach (BufferTypeMetadata<T>?[]? page in this._slots)
		{
			if (page is null)
				return default;

			if (count < acc + page.Length)
			{
				binarySpan = page;
				relativeIndex = count - acc;
				goto Search;
			}

			acc <<= 1;
		}
		return default;

		Search:
		Int32 length = Math.Min(binarySpan.Length - relativeIndex, relativeIndex);
		foreach (BufferTypeMetadata<T>? val in binarySpan.Slice(relativeIndex, length))
		{
			if (val is not null)
				return val;
		}
		return default;
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
}