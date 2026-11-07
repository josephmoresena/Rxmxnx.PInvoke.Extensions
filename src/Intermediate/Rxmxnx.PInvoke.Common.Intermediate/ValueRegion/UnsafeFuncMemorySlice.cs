#if NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class ValueRegion<T>
{
	/// <summary>
	/// This class represents a memory slice provided by a function.
	/// </summary>
	private sealed class UnsafeFuncMemorySlice : MemorySlice
	{
		/// <summary>
		/// The internal function that provides the memory region.
		/// </summary>
		private readonly delegate*<ReadOnlySpan<T>> _func;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.UnsafeFuncMemorySlice"/> class from a
		/// sub-range of an existing <see cref="ValueRegion{T}.UnsafeFuncRegion"/>.
		/// </summary>
		/// <param name="region">A <see cref="ValueRegion{T}.UnsafeFuncRegion"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		public UnsafeFuncMemorySlice(UnsafeFuncRegion region, Int32 offset, Int32 length) : base(
			region.GetLength(), offset, length)
			=> this._func = region;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.UnsafeFuncMemorySlice"/> class from a
		/// sub-range of an existing <see cref="ValueRegion{T}.UnsafeFuncMemorySlice"/>.
		/// </summary>
		/// <param name="region">A <see cref="ValueRegion{T}.UnsafeFuncMemorySlice"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		private UnsafeFuncMemorySlice(UnsafeFuncMemorySlice region, Int32 offset, Int32 length) : base(
			region._func().Length, offset, length, region.Offset)
			=> this._func = region._func;

		/// <inheritdoc/>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		internal override ReadOnlySpan<T> AsSpan() => this._func()[this.Offset..this.End];
		/// <inheritdoc/>
		internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> new UnsafeFuncMemorySlice(this, startIndex, length);
	}

	/// <summary>
	/// This class represents a memory slice provided by a stateful function.
	/// </summary>
	private sealed class UnsafeFuncMemorySlice<TState> : MemorySlice, IWrapper<TState>
	{
		/// <summary>
		/// The internal function that provides the memory region.
		/// </summary>
		private readonly delegate*<TState, ReadOnlySpan<T>> _func;
		/// <summary>
		/// Function state.
		/// </summary>
		private readonly TState _state;
		/// <summary>
		/// Internal function that allocates the memory region.
		/// </summary>
		private readonly delegate*<TState, GCHandleType, out GCHandle, Boolean> _tryAlloc;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.UnsafeFuncMemorySlice{TState}"/> class from a
		/// sub-range of an existing <see cref="ValueRegion{T}.FuncRegion"/>.
		/// </summary>
		/// <param name="region">A <see cref="ValueRegion{T}.UnsafeFuncRegion{TState}"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		public UnsafeFuncMemorySlice(UnsafeFuncRegion<TState> region, Int32 offset, Int32 length) : base(
			region.GetLength(), offset, length)
			=> this._func = region.AsFunctionPointer(out this._state, out this._tryAlloc);

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueRegion{T}.UnsafeFuncMemorySlice{TState}"/> class from a
		/// sub-range of an existing <see cref="ValueRegion{T}.UnsafeFuncMemorySlice{TState}"/>.
		/// </summary>
		/// <param name="region">A <see cref="ValueRegion{T}.FuncRegion"/> instance.</param>
		/// <param name="offset">The offset for the range.</param>
		/// <param name="length">The length of the range.</param>
		private UnsafeFuncMemorySlice(UnsafeFuncMemorySlice<TState> region, Int32 offset, Int32 length) : base(
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
		{
			if (this._tryAlloc != default)
				return this._tryAlloc(this._state, type, out handle);
			handle = default;
			return false;
		}
		/// <inheritdoc/>
		/// <param name="offset">Internal offset.</param>
		public override IPinnable? GetPinnable(out Int32 offset)
		{
			offset = this.Offset;
			return this._state as IPinnable;
		}

		/// <inheritdoc/>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		internal override ReadOnlySpan<T> AsSpan() => this._func(this._state)[this.Offset..this.End];
		/// <inheritdoc/>
		internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> new UnsafeFuncMemorySlice<TState>(this, startIndex, length);
	}
}
#endif