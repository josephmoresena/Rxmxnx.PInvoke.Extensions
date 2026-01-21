namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
	/// <summary>
	/// This class represents a memory region provided by a function.
	/// </summary>
	private sealed class FuncRegion : ValueRegion<T>
	{
		/// <summary>
		/// The internal function that provides the memory region.
		/// </summary>
		private readonly ReadOnlySpanFunc<T> _func;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="func">The internal function that provides the memory region.</param>
		public FuncRegion(ReadOnlySpanFunc<T> func) => this._func = func;

		/// <summary>
		/// Gets the memory region provided by the function.
		/// </summary>
		/// <returns>The memory region provided by the function.</returns>
		public ReadOnlySpanFunc<T> AsReadOnlySpanFunc() => this._func;
		/// <summary>
		/// Invokes internal function and retrieves span length.
		/// </summary>
		/// <returns>Current span length.</returns>
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
			=> new FuncMemorySlice(this, startIndex, length);
		/// <inheritdoc/>
		internal override ReadOnlySpan<T> AsSpan() => this._func();
	}

	/// <summary>
	/// This class represents a memory region provided by a stateful function.
	/// </summary>
	private sealed class FuncRegion<TState> : ValueRegion<T>, IWrapper<TState>
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
		/// Constructor.
		/// </summary>
		/// <param name="state">Function state.</param>
		/// <param name="func">The internal function that provides the memory region.</param>
		/// <param name="alloc">Optional. Function for state allocation.</param>
		public FuncRegion(TState state, ReadOnlySpanFunc<T, TState> func,
			Func<TState, GCHandleType, GCHandle>? alloc = default)
		{
			this._state = state;
			this._func = func;
			this._alloc = alloc;
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
		public ReadOnlySpanFunc<T, TState> AsReadOnlySpanFunc(out TState state,
			out Func<TState, GCHandleType, GCHandle>? alloc)
		{
			state = this._state;
			alloc = this._alloc;
			return this._func;
		}
		/// <summary>
		/// Invokes internal function and retrieves span length.
		/// </summary>
		/// <returns>Current span length.</returns>
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
			=> FuncRegion<TState>.TryAlloc(this._alloc, this._state, type, out handle);
		/// <inheritdoc/>
		/// <param name="offset">Internal offset.</param>
		public override IPinnable? GetPinnable(out Int32 offset)
		{
			offset = default;
			return this._state as IPinnable;
		}

		/// <inheritdoc/>
		internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
			=> new FuncMemorySlice<TState>(this, startIndex, length);
		/// <inheritdoc/>
		internal override ReadOnlySpan<T> AsSpan() => this._func(this._state);

		/// <summary>
		/// Tries to create a new <see cref="GCHandle"/> from <paramref name="state"/> instance.
		/// </summary>
		/// <param name="alloc">Function to create the new <see cref="GCHandle"/>.</param>
		/// <param name="state">The object that uses the <see cref="GCHandle"/>.</param>
		/// <param name="type">The type of <see cref="GCHandle"/> to create.</param>
		/// <param name="handle">Output. Created <see cref="GCHandle"/> that protects the value region.</param>
		/// <returns>
		/// <see langword="true"/> if a <paramref name="handle"/> was successfully created; otherwise, <see langword="false"/>.
		/// </returns>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean TryAlloc(Func<TState, GCHandleType, GCHandle>? alloc, TState state, GCHandleType type,
			out GCHandle handle)
		{
			if (alloc is not null)
				try
				{
					handle = alloc.Invoke(state, type);
					return true;
				}
				catch (Exception)
				{
					// ignored
				}

			handle = default;
			return false;
		}
	}
}