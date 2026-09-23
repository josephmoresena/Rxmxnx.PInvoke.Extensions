namespace Rxmxnx.PInvoke;

public static partial class BufferManager
{
	/// <summary>
	/// Creates a <see cref="VbScopedBuffer{T}"/> instance from <paramref name="buffer"/>.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="buffer">A <see cref="ScopedBuffer{T}"/> instance.</param>
	/// <returns>A new <see cref="VbScopedBuffer{T}"/> instance from <paramref name="buffer"/>.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private static VbScopedBuffer<T> ToVbScopedBuffer<T>(this ScopedBuffer<T> buffer)
		=> new(ref MemoryMarshal.GetReference(buffer.Span), (UInt16)buffer.Span.Length, buffer.BufferMetadata);

	// ReSharper disable once UnusedType.Global
	public static partial class VisualBasic
	{
		/// <summary>
		/// Internal <see cref="VbScopedBufferAction{T}"/> action.
		/// </summary>
		/// <typeparam name="T">Type of items in the buffer.</typeparam>
		/// <param name="action">A <see cref="VbScopedBufferAction{T}"/> delegate.</param>
		private readonly struct VbActionValue<T>(VbScopedBufferAction<T> action) : IScopedBufferAction<T>
		{
			/// <summary>
			/// Internal delegate.
			/// </summary>
			private readonly VbScopedBufferAction<T> _action = action;

			/// <inheritdoc/>
			public Boolean IsMinimalCount { get; init; }
			/// <inheritdoc/>
			public UInt16 Count { get; init; }

			/// <inheritdoc/>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Accept(ScopedBuffer<T> buffer)
			{
				VbScopedBuffer<T> vbBuffer = buffer.ToVbScopedBuffer();
				try
				{
					this._action(vbBuffer);
				}
				finally
				{
					vbBuffer.Unload();
				}
			}
		}

		/// <summary>
		/// Internal <see cref="VbScopedBufferAction{T, TState}"/> action.
		/// </summary>
		/// <typeparam name="T">Type of items in the buffer.</typeparam>
		/// <typeparam name="TState">The type of the state object.</typeparam>
		/// <param name="action">A <see cref="VbScopedBufferAction{T}"/> delegate.</param>
		/// <param name="state">State object.</param>
		private readonly struct VbActionValue<T, TState>(VbScopedBufferAction<T, TState> action, TState state)
			: IScopedBufferAction<T>
		{
			/// <summary>
			/// Internal delegate.
			/// </summary>
			private readonly VbScopedBufferAction<T, TState> _action = action;
			/// <summary>
			/// Internal state.
			/// </summary>
			private readonly TState _state = state;

			/// <inheritdoc/>
			public Boolean IsMinimalCount { get; init; }
			/// <inheritdoc/>
			public UInt16 Count { get; init; }

			/// <inheritdoc/>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public void Accept(ScopedBuffer<T> buffer)
			{
				VbScopedBuffer<T> vbBuffer = buffer.ToVbScopedBuffer();
				try
				{
					this._action(vbBuffer, this._state);
				}
				finally
				{
					vbBuffer.Unload();
				}
			}
		}

		/// <summary>
		/// Internal <see cref="VbScopedBufferFunc{T, TResult}"/> action.
		/// </summary>
		/// <typeparam name="T">Type of items in the buffer.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="func">A <see cref="VbScopedBufferFunc{T, TResult}"/> delegate.</param>
		private readonly struct VbFunctionValue<T, TResult>(VbScopedBufferFunc<T, TResult> func)
			: IScopedBufferFunction<T, TResult>
		{
			/// <summary>
			/// Internal delegate.
			/// </summary>
			private readonly VbScopedBufferFunc<T, TResult> _func = func;

			/// <inheritdoc/>
			public Boolean IsMinimalCount { get; init; }
			/// <inheritdoc/>
			public UInt16 Count { get; init; }

			/// <inheritdoc/>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public TResult Apply(ScopedBuffer<T> buffer)
			{
				VbScopedBuffer<T> vbBuffer = buffer.ToVbScopedBuffer();
				try
				{
					return this._func(buffer.ToVbScopedBuffer());
				}
				finally
				{
					vbBuffer.Unload();
				}
			}
		}

		/// <summary>
		/// Internal <see cref="VbScopedBufferFunc{T, TState, TResult}"/> action.
		/// </summary>
		/// <typeparam name="T">Type of items in the buffer.</typeparam>
		/// <typeparam name="TState">The type of the state object.</typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="func">A <see cref="VbScopedBufferFunc{T, TState, TResult}"/> delegate.</param>
		/// <param name="state">State object.</param>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
#endif
		private readonly struct VbFunctionValue<T, TState, TResult>(
			VbScopedBufferFunc<T, TState, TResult> func,
			TState state) : IScopedBufferFunction<T, TResult>
		{
			/// <summary>
			/// Internal delegate.
			/// </summary>
			private readonly VbScopedBufferFunc<T, TState, TResult> _func = func;
			/// <summary>
			/// Internal state.
			/// </summary>
			private readonly TState _state = state;

			/// <inheritdoc/>
			public Boolean IsMinimalCount { get; init; }
			/// <inheritdoc/>
			public UInt16 Count { get; init; }

			/// <inheritdoc/>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			public TResult Apply(ScopedBuffer<T> buffer)
			{
				VbScopedBuffer<T> vbBuffer = buffer.ToVbScopedBuffer();
				try
				{
					return this._func(buffer.ToVbScopedBuffer(), this._state);
				}
				finally
				{
					vbBuffer.Unload();
				}
			}
		}
	}
}