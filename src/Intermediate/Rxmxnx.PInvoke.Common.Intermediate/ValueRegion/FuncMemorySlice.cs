namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
	/// <summary>
	/// This class represents a memory slice provided by a function.
	/// </summary>
	private sealed class FuncMemorySlice : MemorySlice
	{
		/// <summary>
		/// The internal function that provides the memory region.
		/// </summary>
		private readonly ReadOnlySpanFunc<T> _func;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.FuncMemorySlice"/> class from a sub-range of an
		/// existing <see cref="ValueRegion{T}.FuncRegion"/>.
		/// </summary>
		/// <param name="region">A <see cref="ValueRegion{T}.FuncRegion"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		public FuncMemorySlice(FuncRegion region, Int32 offset, Int32 length) : base(region.GetLength(), offset, length)
			=> this._func = region.AsReadOnlySpanFunc();

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.FuncMemorySlice"/> class from a sub-range of an
		/// existing <see cref="ValueRegion{T}.FuncMemorySlice"/>.
		/// </summary>
		/// <param name="region">A <see cref="ValueRegion{T}.FuncRegion"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		private FuncMemorySlice(FuncMemorySlice region, Int32 offset, Int32 length) : base(
			region._func().Length, offset, length, region.Offset)
			=> this._func = region._func;

		/// <inheritdoc/>
		internal override ReadOnlySpan<T> AsSpan() => this._func()[this.Offset..this.End];
		/// <inheritdoc/>
		internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> new FuncMemorySlice(this, startIndex, length);
	}

	/// <summary>
	/// This class represents a memory slice provided by a stateful function.
	/// </summary>
	private sealed class FuncMemorySlice<TState> : MemorySlice, IWrapper<TState>
	{
		/// <summary>
		/// Internal function that allocates the memory region.
		/// </summary>
		private readonly Func<TState, GCHandleType, GCHandle>? _alloc;
		/// <summary>
		/// The internal function that provides the memory region.
		/// </summary>
		private readonly ReadOnlySpanFunc<T, TState> _func;
		/// <summary>
		/// Function state.
		/// </summary>
		private readonly TState _state;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.FuncMemorySlice"/> class from a sub-range of an
		/// existing <see cref="ValueRegion{T}.FuncRegion"/>.
		/// </summary>
		/// <param name="region">A <see cref="ValueRegion{T}.FuncRegion"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		public FuncMemorySlice(FuncRegion<TState> region, Int32 offset, Int32 length) : base(
			region.GetLength(), offset, length)
			=> this._func = region.AsReadOnlySpanFunc(out this._state, out this._alloc);

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.FuncMemorySlice"/> class from a sub-range of an
		/// existing <see cref="ValueRegion{T}.FuncMemorySlice"/>.
		/// </summary>
		/// <param name="region">A <see cref="ValueRegion{T}.FuncRegion"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		private FuncMemorySlice(FuncMemorySlice<TState> region, Int32 offset, Int32 length) : base(
			region._func(region._state).Length, offset, length, region.Offset)
		{
			this._func = region._func;
			this._state = region._state;
		}

#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		TState IWrapper<TState>.Value => this._state;

		/// <inheritdoc/>
		public override Boolean TryAlloc(GCHandleType type, out GCHandle handle)
			=> FuncRegion<TState>.TryAlloc(this._alloc, this._state, type, out handle);
		/// <inheritdoc/>
		/// <param name="offset">Internal offset.</param>
		public override IPinnable? GetPinnable(out Int32 offset)
		{
			offset = this.Offset;
			return this._state as IPinnable;
		}

		/// <inheritdoc/>
		internal override ReadOnlySpan<T> AsSpan() => this._func(this._state)[this.Offset..this.End];
		/// <inheritdoc/>
		internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> new FuncMemorySlice<TState>(this, startIndex, length);
	}
}