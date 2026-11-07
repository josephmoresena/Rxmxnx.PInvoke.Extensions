#if NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class ValueRegion<T>
{
	/// <summary>
	/// This class represents a memory region provided by a function.
	/// </summary>
	private sealed class UnsafeFuncRegion : ValueRegion<T>
	{
		/// <summary>
		/// The internal function that provides the memory region.
		/// </summary>
		private readonly delegate*<ReadOnlySpan<T>> _func;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="func">The internal function that provides the memory region.</param>
		public UnsafeFuncRegion(delegate*<ReadOnlySpan<T>> func) => this._func = func;

		/// <summary>
		/// Invokes internal function and retrieves span length.
		/// </summary>
		/// <returns>Current span length.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int32 GetLength() => this._func().Length;

		/// <inheritdoc/>
		public override ValueRegion<T> Slice(Int32 startIndex)
		{
			Int32 regionLength = this._func().Length;
			Int32 length = regionLength - startIndex;
			ValidationUtilities.ThrowIfInvalidSubregion(regionLength, startIndex, length);
			return this.InternalSlice(startIndex, length);
		}
		/// <inheritdoc/>
		public override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
		{
			Int32 regionLength = this._func().Length;
			ValidationUtilities.ThrowIfInvalidSubregion(regionLength, startIndex, length);
			return this.InternalSlice(startIndex, length);
		}

		/// <inheritdoc/>
		internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> new UnsafeFuncMemorySlice(this, startIndex, length);
		/// <inheritdoc/>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		internal override ReadOnlySpan<T> AsSpan() => this._func();

		/// <summary>
		/// Defines an implicit conversion of a given <see cref="ValueRegion{T}.UnsafeFuncRegion"/> to a function pointer.
		/// </summary>
		/// <param name="region">A <see cref="ValueRegion{T}.UnsafeFuncRegion"/> to implicitly convert.</param>
		public static implicit operator delegate*<ReadOnlySpan<T>>(UnsafeFuncRegion region) => region._func;
	}

	/// <summary>
	/// This class represents a memory region provided by a stateful function.
	/// </summary>
	private sealed class UnsafeFuncRegion<TState> : ValueRegion<T>, IWrapper<TState>
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
		/// Constructor.
		/// </summary>
		/// <param name="state">Function state.</param>
		/// <param name="func">The internal function that provides the memory region.</param>
		/// <param name="tryAlloc">Function for state allocation.</param>
		public UnsafeFuncRegion(TState state, delegate*<TState, ReadOnlySpan<T>> func,
			delegate*<TState, GCHandleType, out GCHandle, Boolean> tryAlloc)
		{
			this._state = state;
			this._func = func;
			this._tryAlloc = tryAlloc;
		}

#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		TState IWrapper<TState>.Value => this._state;
		/// <summary>
		/// Gets the memory region provided by the function.
		/// </summary>
		/// <param name="state">Function state.</param>
		/// <param name="alloc">Function for state allocation.</param>
		/// <returns>The memory region provided by the function.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public delegate*<TState, ReadOnlySpan<T>> AsFunctionPointer(out TState state,
			out delegate*<TState, GCHandleType, out GCHandle, Boolean> tryAlloc)
		{
			state = this._state;
			tryAlloc = this._tryAlloc;
			return this._func;
		}
		/// <summary>
		/// Invokes internal function and retrieves span length.
		/// </summary>
		/// <returns>Current span length.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Int32 GetLength() => this._func(this._state).Length;

		/// <inheritdoc/>
		public override ValueRegion<T> Slice(Int32 startIndex)
		{
			Int32 regionLength = this._func(this._state).Length;
			Int32 length = regionLength - startIndex;
			ValidationUtilities.ThrowIfInvalidSubregion(regionLength, startIndex, length);
			return this.InternalSlice(startIndex, length);
		}
		/// <inheritdoc/>
		public override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
		{
			Int32 regionLength = this._func(this._state).Length;
			ValidationUtilities.ThrowIfInvalidSubregion(regionLength, startIndex, length);
			return this.InternalSlice(startIndex, length);
		}
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
			offset = default;
			return this._state as IPinnable;
		}

		/// <inheritdoc/>
		internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> new UnsafeFuncMemorySlice<TState>(this, startIndex, length);
		/// <inheritdoc/>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		internal override ReadOnlySpan<T> AsSpan() => this._func(this._state);
	}
}
#endif