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

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="instance">A <see cref="MetadataStorage{T}"/> instance.</param>
	public BinaryMap(MetadataStorage<T> instance)
	{
		this._initial = MemoryMarshal.CreateSpan(ref instance.MetadataReference, instance.Capacity);
		this._slots = instance is G2047<T> g2047 ? g2047.Slots.AsSpan() : [];
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
	public BufferTypeMetadata<T>? GetMinimal(UInt16 count)
	{
		if (count >= this._initial.Length && this._slots.IsEmpty)
			return default;
		ReadOnlySpan<BufferTypeMetadata<T>?> binarySpan;
		switch (count)
		{
			case >= 32768 when !this._slots.IsEmpty && this._slots[4] is { } page:
				binarySpan = page.AsSpan();
				count -= 32768;
				break;
			case >= 16384 when !this._slots.IsEmpty && this._slots[3] is { } page:
				binarySpan = page.AsSpan();
				count -= 16384;
				break;
			case >= 8192 when !this._slots.IsEmpty && this._slots[2] is { } page:
				binarySpan = page.AsSpan();
				count -= 8192;
				break;
			case >= 4096 when !this._slots.IsEmpty && this._slots[1] is { } page:
				binarySpan = page.AsSpan();
				count -= 4096;
				break;
			case >= 2048 when !this._slots.IsEmpty && this._slots[0] is { } page:
				binarySpan = page.AsSpan();
				count -= 2048;
				break;
			case < 2048:
				binarySpan = this._initial;
				break;
			default:
				return default;
		}
		foreach (BufferTypeMetadata<T>? val in binarySpan.Slice(count, Math.Min(binarySpan.Length - count, count)))
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
		if (slots.IsEmpty || slots[0] is null)
			return count <= capacity;
		if (slots[4] is not null)
			return true;
		if (slots[3] is not null)
			return count <= 32767;
		if (slots[2] is not null)
			return count < 16383;
		if (slots[1] is not null)
			return count < 8191;
		return count < 4095;
	}
}