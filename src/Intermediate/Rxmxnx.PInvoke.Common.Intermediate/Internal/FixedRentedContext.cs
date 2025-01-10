namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Contains a rented fixed array instance
/// </summary>
internal sealed class FixedRentedContext<T> : IFixedContext<T>.IDisposable
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
	/// Internal fixed context.
	/// </summary>
	private readonly FixedContext<T> _ctx;
	/// <summary>
	/// <see cref="MemoryHandle"/> from array.
	/// </summary>
	private MemoryHandle _handle;

	/// <summary>
	/// Rented array;
	/// </summary>
	public T[] Array { get; }

	/// <summary>
	/// Private constructor.
	/// </summary>
	/// <param name="arrayPool">A <see cref="ArrayPool{T}"/> instance.</param>
	/// <param name="length">Required length.</param>
	/// <param name="clearArray">Indicates whether the contents of the buffer should be cleared before reuse.</param>
	public unsafe FixedRentedContext(ArrayPool<T> arrayPool, Int32 length, Boolean clearArray)
	{
		this._arrayPool = arrayPool;
		this.Array = arrayPool.Rent(length);
		this._clearArray = clearArray;
		this._handle = this.Array.AsMemory().Pin();
		this._ctx = new(this._handle.Pointer, length);
	}

	[ExcludeFromCodeCoverage]
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IFixedMemory residual)
		=> this.GetContext().Transformation<TDestination>(out residual);
	[ExcludeFromCodeCoverage]
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		=> this.GetContext().Transformation<TDestination>(out residual);
	[ExcludeFromCodeCoverage]
	IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.
		Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		=> this.GetContext().Transformation<TDestination>(out residual);
	[ExcludeFromCodeCoverage]
	IntPtr IFixedPointer.Pointer => this.GetContext().Pointer;
	[ExcludeFromCodeCoverage]
	Boolean IReadOnlyFixedMemory.IsNullOrEmpty => this.GetContext().IsNullOrEmpty;
	[ExcludeFromCodeCoverage]
	Span<Byte> IFixedMemory.Bytes => this.GetContext().Bytes;
	[ExcludeFromCodeCoverage]
	Span<Object> IFixedMemory.Objects => this.GetContext().Objects;
	[ExcludeFromCodeCoverage]
	ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => this.GetReadOnlyContext().Bytes;
	[ExcludeFromCodeCoverage]
	ReadOnlySpan<Object> IReadOnlyFixedMemory.Objects => this.GetReadOnlyContext().Objects;
	[ExcludeFromCodeCoverage]
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.GetReadOnlyContext().AsBinaryContext();
	[ExcludeFromCodeCoverage]
	IFixedContext<Object> IFixedMemory.AsObjectContext() => this.GetContext().AsObjectContext();
	[ExcludeFromCodeCoverage]
	IFixedContext<Byte> IFixedMemory.AsBinaryContext() => this.GetContext().AsBinaryContext();
	[ExcludeFromCodeCoverage]
	IReadOnlyFixedContext<Object> IReadOnlyFixedMemory.AsObjectContext() => this.GetReadOnlyContext().AsObjectContext();
	[ExcludeFromCodeCoverage]
	ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => this.GetContext().Values;
	[ExcludeFromCodeCoverage]
	Span<T> IFixedMemory<T>.Values => this.GetContext().Values;

	/// <inheritdoc/>
	public void Dispose()
	{
		this.Release();
		GC.SuppressFinalize(this);
	}

	[ExcludeFromCodeCoverage]
	~FixedRentedContext() => this.Release();

	/// <summary>
	/// Release all resources.
	/// </summary>
	private void Release()
	{
		this._ctx.Unload();
		this._handle.Dispose();
		this._arrayPool.Return(this.Array, this._clearArray);
	}

	/// <summary>
	/// Fixed context.
	/// </summary>
	[ExcludeFromCodeCoverage]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IFixedContext<T> GetContext() => this._ctx;
	/// <summary>
	/// Read-only fixed context.
	/// </summary>
	[ExcludeFromCodeCoverage]
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private IFixedContext<T> GetReadOnlyContext() => this._ctx;
}