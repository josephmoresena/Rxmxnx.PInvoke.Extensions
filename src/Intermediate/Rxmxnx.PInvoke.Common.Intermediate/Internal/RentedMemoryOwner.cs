namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Owns a rented and pinned array allocation.
/// </summary>
/// <typeparam name="T">The array item type.</typeparam>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal sealed unsafe class RentedMemoryOwner<T> : IDisposable
{
	/// <summary>
	/// Array pool.
	/// </summary>
	private readonly ArrayPool<T> _arrayPool;
	/// <summary>
	/// Indicates whether the contents of the buffer should be cleared before reuse.
	/// </summary>
	private readonly Boolean _clearArray;
	/// <summary>
	/// Rented array.
	/// </summary>
	private T[]? _array;
	/// <summary>
	/// <see cref="MemoryHandle"/> from array.
	/// </summary>
	private MemoryHandle _handle;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="arrayPool">A <see cref="ArrayPool{T}"/> instance.</param>
	/// <param name="length">Required length.</param>
	/// <param name="clearArray">Indicates whether the contents of the buffer should be cleared before reuse.</param>
	/// <param name="arrayLength">Output. Rented array length.</param>
	private RentedMemoryOwner(ArrayPool<T> arrayPool, Int32 length, Boolean clearArray, out Int32 arrayLength)
	{
		this._arrayPool = arrayPool;
		this._array = arrayPool.Rent(length);
		this._clearArray = clearArray;
		this._handle = new ReadOnlyMemory<T>(this._array).Pin();

		arrayLength = this._array.Length;
	}

	/// <inheritdoc/>
	public void Dispose()
	{
		this.Release();
		GC.SuppressFinalize(this);
	}

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	~RentedMemoryOwner() { this.Release(); }

	/// <summary>
	/// Releases the rented array allocation.
	/// </summary>
	private void Release()
	{
		T[]? array = this._array;
		if (array is null) return;
		this._array = default;
		this._handle.Dispose();
		this._arrayPool.Return(array, this._clearArray);
	}

	/// <summary>
	/// Rents and pins an array of minimum <paramref name="count"/> elements from <paramref name="arrayPool"/>,
	/// ensuring a safe context for accessing the fixed memory.
	/// </summary>
	/// <typeparam name="T">
	/// The unmanaged type from which the contiguous region of memory will be fixed.
	/// </typeparam>
	/// <param name="arrayPool">A <see cref="ArrayPool{T}"/> instance.</param>
	/// <param name="count">Minimum size of rented array.</param>
	/// <param name="clearArray">Indicates whether the contents of the buffer should be cleared before reuse.</param>
	/// <param name="arrayLength">Output. Rented array length.</param>
	/// <returns>An <see cref="IFixedContext{T}.IDisposable"/> instance representing the pinned memory.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IFixedContext<T>.IDisposable CreateContext(ArrayPool<T> arrayPool, Int32 count, Boolean clearArray,
		out Int32 arrayLength)
	{
		if (count == 0)
		{
			arrayLength = default;
			return FixedContext<T>.EmptyDisposable;
		}
		RentedMemoryOwner<T> owner = new(arrayPool, count, clearArray, out arrayLength);
		return new FixedContext<T>(owner._handle.Pointer, count).ToDisposable(owner);
	}
}