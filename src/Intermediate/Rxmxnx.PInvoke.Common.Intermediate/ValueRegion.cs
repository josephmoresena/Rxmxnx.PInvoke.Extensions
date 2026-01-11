namespace Rxmxnx.PInvoke;

/// <summary>
/// This class represents a region of memory that contains a sequence of
/// <typeparamref name="T"/> values.
/// </summary>
/// <typeparam name="T">The type of the items in the sequence.</typeparam>
public abstract partial class ValueRegion<T>
{
	/// <summary>
	/// Retrieves an item from the memory region at the specified zero-based <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The zero-based index of the element to retrieve.</param>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when <paramref name="index"/> is less than zero or greater than or equal to
	/// the length of the memory region.
	/// </exception>
	/// <returns>The element at the specified index within the memory region.</returns>
	[IndexerName("Item")]
	public virtual T this[Int32 index] => this.AsSpan()[index];
	/// <summary>
	/// Indicates whether the current instance represents a subregion of a memory region.
	/// </summary>
	public virtual Boolean IsMemorySlice => false;

	/// <summary>
	/// Copies the contents of this memory region into a new array.
	/// </summary>
	/// <returns>An array containing the copied data from the current memory region.</returns>
	public virtual T[] ToArray() => this.AsSpan().ToArray();
	/// <summary>
	/// Tries to create a new <see cref="GCHandle"/> for current value region.
	/// </summary>
	/// <param name="type">The type of <see cref="GCHandle"/> to create.</param>
	/// <param name="handle">Output. Created <see cref="GCHandle"/> that protects the value region.</param>
	/// <returns>
	/// <see langword="true"/> if a <paramref name="handle"/> was successfully created; otherwise, <see langword="false"/>.
	/// </returns>
	public virtual Boolean TryAlloc(GCHandleType type, out GCHandle handle)
	{
		handle = default;
		return false;
	}
	/// <summary>
	/// Retrieves the <see cref="IPinnable"/> instance for current region.
	/// </summary>
	/// <param name="offset">Memory region offset.</param>
	/// <returns>The <see cref="IPinnable"/> instance for current region.</returns>
	public virtual IPinnable? GetPinnable(out Int32 offset)
	{
		offset = default;
		return default;
	}

	/// <summary>
	/// Retrieves a subsequence from this instance, starting from a specified item position and extending to the end of
	/// the region.
	/// </summary>
	/// <param name="startIndex">
	/// The zero-based starting item position of the subregion in this instance.
	/// </param>
	/// <returns>
	/// A <see cref="ValueRegion{T}"/> equivalent to the subregion starting at <paramref name="startIndex"/> in this instance.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when <paramref name="startIndex"/> is less than zero or greater than or equal to the length of the memory
	/// region.
	/// </exception>
	public abstract ValueRegion<T> Slice(Int32 startIndex);
	/// <summary>
	/// Retrieves a subregion from this instance. The subregion starts at a specified item position and has a specified length.
	/// </summary>
	/// <param name="startIndex">
	/// The zero-based starting item position of the subregion in this instance.
	/// </param>
	/// <param name="length">The number of items in the subregion.</param>
	/// <returns>
	/// A <see cref="ValueRegion{T}"/> equivalent to the subregion of length <paramref name="length"/>, starting at
	/// <paramref name="startIndex"/> in this instance.
	/// </returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown when <paramref name="startIndex"/> is less than zero, greater than or equal to the length of the memory region,
	/// or when the specified <paramref name="length"/> is greater than the remaining length from the
	/// <paramref name="startIndex"/>.
	/// </exception>
	public abstract ValueRegion<T> Slice(Int32 startIndex, Int32 length);

	/// <summary>
	/// Creates a new read-only span over this memory region.
	/// </summary>
	/// <returns>A read-only span representation of the memory region.</returns>
	internal abstract ReadOnlySpan<T> AsSpan();
	/// <summary>
	/// Retrieves a subregion from this instance.
	/// The subregion starts at a specified item position and has a specified length.
	/// </summary>
	/// <param name="startIndex">
	/// The zero-based starting item position of a subregion in this instance.
	/// </param>
	/// <param name="length">The number of items in the subregion.</param>
	/// <returns>
	/// A <see cref="ValueRegion{T}"/> that is equivalent to the subregion of length
	/// <paramref name="length"/> that begins at <paramref name="startIndex"/> in this
	/// instance.
	/// </returns>
	/// <remarks>This method does not perform any validation.</remarks>
	internal abstract ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length);

	/// <summary>
	/// Retrieves an array from this memory region if the region is contained in an array.
	/// </summary>
	/// <returns>
	/// An array containing the data from the current memory region if the region is contained in an array;
	/// otherwise, returns <see langword="null"/>.
	/// </returns>
	private protected virtual T[]? AsArray() => default;

	/// <summary>
	/// Attempts to retrieve a <see cref="ReadOnlyMemory{T}"/> instance representing the current instance.
	/// </summary>
	/// <param name="memory">Output. A <see cref="ReadOnlyMemory{T}"/> instance representing the current instance.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="memory"/> instance represents the current instance; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	internal virtual Boolean TryGetMemory(out ReadOnlyMemory<T> memory)
	{
		Unsafe.SkipInit(out memory);
		return false;
	}
	/// <summary>
	/// Converts the value of the current <see cref="ValueRegion{T}"/> to its equivalent read-only span representation.
	/// </summary>
	/// <param name="region">The <see cref="ValueRegion{T}"/> to convert.</param>
	/// <returns>A read-only span representation of the <see cref="ValueRegion{T}"/>.</returns>
	public static implicit operator ReadOnlySpan<T>(ValueRegion<T> region) => region.AsSpan();
	/// <summary>
	/// Converts the value of the current <see cref="ValueRegion{T}"/> to its equivalent array representation.
	/// </summary>
	/// <param name="region">The <see cref="ValueRegion{T}"/> to convert.</param>
	/// <returns>
	/// An array representation of the memory region. Returns <see langword="null"/> if the region is not contained in
	/// an array.
	/// </returns>
	public static explicit operator T[]?(ValueRegion<T> region) => region.AsArray();

	/// <summary>
	/// Creates a new <see cref="ValueRegion{T}"/> instance from an array of
	/// <typeparamref name="T"/> values.
	/// </summary>
	/// <param name="array">Array of <typeparamref name="T"/> values.</param>
	/// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
	public static ValueRegion<T> Create(T[] array) => new ManagedRegion(array);
	/// <summary>
	/// Creates a new <see cref="ValueRegion{T}"/> instance from a pointer to a native memory region.
	/// </summary>
	/// <param name="ptr">Pointer to memory region.</param>
	/// <param name="length">Amount of values in sequence.</param>
	/// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
	/// <remarks>
	/// If the provided pointer is <see langword="null"/>, the method returns an empty <see cref="ValueRegion{T}"/>
	/// instance. This method might be intended for use only with memory regions of unmanaged types.
	/// </remarks>
	public static ValueRegion<T> Create(IntPtr ptr, Int32 length)
		=> ptr != IntPtr.Zero && length != default ? new(ptr, length) : NativeRegion.Empty;
	/// <summary>
	/// Creates a new <see cref="ValueRegion{T}"/> instance from a <see cref="ReadOnlySpanFunc{T}"/>
	/// function.
	/// </summary>
	/// <param name="func"><see cref="ReadOnlySpanFunc{T}"/> delegate.</param>
	/// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
	public static ValueRegion<T> Create(ReadOnlySpanFunc<T> func) => new FuncRegion(func);
	/// <summary>
	/// Creates a new <see cref="ValueRegion{T}"/> instance from a <see cref="ReadOnlySpanFunc{T, TState}"/>
	/// function and a <typeparamref name="TState"/> instance.
	/// </summary>
	/// <param name="state">Function state.</param>
	/// <param name="func"><see cref="ReadOnlySpanFunc{T, TState}"/> delegate.</param>
	/// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
	public static ValueRegion<T> Create<TState>(TState state, ReadOnlySpanFunc<T, TState> func)
		=> new FuncRegion<TState>(state, func);
	/// <summary>
	/// Creates a new <see cref="ValueRegion{T}"/> instance from a <see cref="ReadOnlySpanFunc{T, TState}"/>
	/// function and a <typeparamref name="TState"/> instance.
	/// </summary>
	/// <param name="state">Function state.</param>
	/// <param name="func"><see cref="ReadOnlySpanFunc{T, TState}"/> delegate.</param>
	/// <param name="alloc">Allocation state delegate.</param>
	/// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
	public static ValueRegion<T> Create<TState>(TState state, ReadOnlySpanFunc<T, TState> func,
		Func<TState, GCHandleType, GCHandle>? alloc)
		=> new FuncRegion<TState>(state, func, alloc);
}