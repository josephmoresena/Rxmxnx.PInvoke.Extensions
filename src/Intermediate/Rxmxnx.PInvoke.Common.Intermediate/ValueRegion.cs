namespace Rxmxnx.PInvoke;

/// <summary>
/// This class represents a region of memory that contains a sequence of
/// <typeparamref name="T"/> values.
/// </summary>
/// <remarks>
/// This is not a general-purpose class; it should only be used with <see langword="unmanaged"/> types.
/// </remarks>
/// <typeparam name="T">The <see langword="unmanaged"/> type of the items in the sequence.</typeparam>
public abstract partial class ValueRegion<T> where T : unmanaged
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
	public T[] ToArray() => this.AsSpan().ToArray();

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
	protected virtual T[]? AsArray() => default;

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
	/// Creates a new <see cref="ValueRegion{T}"/> instance from a pointer to a memory region.
	/// </summary>
	/// <param name="ptr">Pointer to memory region.</param>
	/// <param name="length">Amount of values in sequence.</param>
	/// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
	/// <remarks>
	/// If the provided pointer is <see langword="null"/>, the method returns an empty <see cref="ValueRegion{T}"/>
	/// instance.
	/// </remarks>
	public static ValueRegion<T> Create(IntPtr ptr, Int32 length)
		=> ptr != IntPtr.Zero && length != default ? new(ptr, length) : NativeRegion.Empty;
	/// <summary>
	/// Creates a new <see cref="ValueRegion{T}"/> instance from a <see cref="ReadOnlySpanFunc{T}"/>
	/// delegate.
	/// </summary>
	/// <param name="func"><see cref="ReadOnlySpanFunc{T}"/> delegate.</param>
	/// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
	public static ValueRegion<T> Create(ReadOnlySpanFunc<T> func) => new FuncRegion(func);
}