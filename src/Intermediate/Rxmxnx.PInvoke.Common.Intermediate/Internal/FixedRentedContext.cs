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
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	public unsafe FixedRentedContext(ArrayPool<T> arrayPool, Int32 length, Boolean clearArray)
	{
		this._arrayPool = arrayPool;
		this.Array = arrayPool.Rent(length);
		this._clearArray = clearArray;
		this._handle = new ReadOnlyMemory<T>(this.Array).Pin();
		this._ctx = new(this._handle.Pointer, length);
	}

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IFixedMemory residual)
		=> this.GetContext().Transformation<TDestination>(out residual);
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		=> this.GetContext().Transformation<TDestination>(out residual);
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.
		Transformation<TDestination>(out IReadOnlyFixedMemory residual)
		=> this.GetContext().Transformation<TDestination>(out residual);
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IntPtr IFixedPointer.Pointer => this.GetContext().Pointer;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	Boolean IReadOnlyFixedMemory.IsNullOrEmpty => this.GetContext().IsNullOrEmpty;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	Span<Byte> IFixedMemory.Bytes => this.GetContext().Bytes;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	Span<Object> IFixedMemory.Objects => this.GetContext().Objects;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => this.GetReadOnlyContext().Bytes;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ReadOnlySpan<Object> IReadOnlyFixedMemory.Objects => this.GetReadOnlyContext().Objects;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => this.GetReadOnlyContext().AsBinaryContext();
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IFixedContext<Object> IFixedMemory.AsObjectContext() => this.GetContext().AsObjectContext();
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IFixedContext<Byte> IFixedMemory.AsBinaryContext() => this.GetContext().AsBinaryContext();
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	IReadOnlyFixedContext<Object> IReadOnlyFixedMemory.AsObjectContext() => this.GetReadOnlyContext().AsObjectContext();
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => this.GetContext().Values;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	Span<T> IFixedMemory<T>.Values => this.GetContext().Values;

	/// <inheritdoc/>
	public void Dispose()
	{
		this.Release();
		GC.SuppressFinalize(this);
	}

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	~FixedRentedContext() => this.Release();

	/// <summary>
	/// Release all resources.
	/// </summary>
	private void Release()
	{
		if (this._ctx is not { IsValid: true, } || (this._ctx as IFixedPointer).Pointer != IntPtr.Zero) return;
		this._ctx.Unload();
		this._handle.Dispose();
		this._arrayPool.Return(this.Array, this._clearArray);
	}

	/// <summary>
	/// Fixed context.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CA1859
	private IFixedContext<T> GetContext() => this._ctx;
#pragma warning restore CA1859
	/// <summary>
	/// Read-only fixed context.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#pragma warning disable CA1859
	private IReadOnlyFixedContext<T> GetReadOnlyContext() => this._ctx;
#pragma warning restore CA1859
}