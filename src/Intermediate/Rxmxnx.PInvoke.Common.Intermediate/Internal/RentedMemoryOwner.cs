namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Owns a rented and pinned array allocation.
/// </summary>
/// <typeparam name="T">The array item type.</typeparam>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal sealed unsafe class RentedMemoryOwner<T> : FixedValueHandle.Memory
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
	/// Constructor.
	/// </summary>
	/// <param name="arrayPool">A <see cref="ArrayPool{T}"/> instance.</param>
	/// <param name="array">Rented array..</param>
	/// <param name="clearArray">Indicates whether the contents of the buffer should be cleared before reuse.</param>
	/// <param name="arrayLength">Output. Rented array length.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private RentedMemoryOwner(ArrayPool<T> arrayPool, T[] array, Boolean clearArray, out Int32 arrayLength) : base(
		new ReadOnlyMemory<T>(array).Pin())
	{
		this._arrayPool = arrayPool;
		this._array = array;
		this._clearArray = clearArray;

		arrayLength = this._array.Length;
	}

	/// <inheritdoc/>
	protected override Boolean Dispose(Boolean disposing)
	{
		if (!base.Dispose(disposing)) return false;
		this.Release();
		return true;
	}

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	~RentedMemoryOwner() => this.Release();

	/// <summary>
	/// Releases the rented array allocation.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private void Release()
	{
		if (this._array is not { } array) return;
		this._array = default;
		this._arrayPool.Return(array, this._clearArray);
	}

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Rents and pins an array of minimum <paramref name="count"/> elements from <paramref name="arrayPool"/>,
	/// ensuring a safe context for accessing the fixed memory.
	/// </summary>
	/// <param name="arrayPool">A <see cref="ArrayPool{T}"/> instance.</param>
	/// <param name="count">Minimum size of rented array.</param>
	/// <param name="clearArray">Indicates whether the contents of the buffer should be cleared before reuse.</param>
	/// <param name="arrayLength">Output. Rented array length.</param>
	/// <returns>An <see cref="IFixedContext{T}.IDisposable"/> instance representing the pinned memory.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if OBSOLETE_FIXED_INTERFACES && !GITHUB_ACTIONS
	[Obsolete]
#endif
	public static IFixedContext<T>.IDisposable CreateContext(ArrayPool<T> arrayPool, Int32 count, Boolean clearArray,
		out Int32 arrayLength)
	{
		if (count == 0)
		{
			arrayLength = default;
#pragma warning disable CS0612
			return FixedContext<T>.EmptyDisposable;
#pragma warning restore CS0612
		}
		RentedMemoryOwner<T> owner = new(arrayPool, arrayPool.Rent(count), clearArray, out arrayLength);
		return new FixedContext<T>(owner.Pointer, count).ToDisposable(owner);
	}
#endif
	/// <summary>
	/// Rents and pins an array of minimum <paramref name="count"/> elements from <paramref name="arrayPool"/>,
	/// ensuring a safe context for accessing the fixed memory.
	/// </summary>
	/// <param name="arrayPool">A <see cref="ArrayPool{T}"/> instance.</param>
	/// <param name="count">Minimum size of rented array.</param>
	/// <param name="clearArray">Indicates whether the contents of the buffer should be cleared before reuse.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="FixedContextValue{T}"/> instance representing the fixed memory.
	/// </param>
	/// <param name="arrayLength">Output. Rented array length.</param>
	/// <returns>An <see cref="IDisposable"/> instance representing the pinned memory.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IDisposable CreateContext(ArrayPool<T> arrayPool, Int32 count, Boolean clearArray,
		out FixedContextValue<T> fixedContext, out Int32 arrayLength)
	{
		if (count == 0)
		{
			fixedContext = default;
			arrayLength = default;
			return FixedValueHandle.EmptyDisposable;
		}
		RentedMemoryOwner<T> owner = new(arrayPool, arrayPool.Rent(count), clearArray, out arrayLength);
		return FixedContextValue<T>.CreateDisposable((ValPtr<T>)owner.Pointer, count, owner, out fixedContext);
	}
}