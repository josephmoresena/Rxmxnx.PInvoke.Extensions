namespace Rxmxnx.PInvoke;

public partial class BufferTypeMetadata
{
	/// <summary>
	/// Internal <see cref="ScopedBufferAction{T}"/> action.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="action">A <see cref="ScopedBufferAction{T}"/> delegate.</param>
	protected readonly struct ActionValue<T>(ScopedBufferAction<T> action) : IScopedBufferAction<T>
	{
		/// <summary>
		/// Internal delegate.
		/// </summary>
		private readonly ScopedBufferAction<T> _action = action;

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Invoke(ScopedBuffer<T> buffer) => this._action(buffer);
	}

	/// <summary>
	/// Internal <see cref="ScopedBufferAction{T, TState}"/> action.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TState">The type of the state object.</typeparam>
	/// <param name="action">A <see cref="ScopedBufferAction{T}"/> delegate.</param>
	/// <param name="state">State object.</param>
#if !NET9_0_OR_GREATER
	protected readonly struct ActionValue<T, TState>(ScopedBufferAction<T, TState> action, TState state)
		: IScopedBufferAction<T>
#else
	protected readonly ref struct ActionValue<T, TState>(ScopedBufferAction<T, TState> action, TState state)
		: IScopedBufferAction<T> where TState : allows ref struct
#endif
	{
		/// <summary>
		/// Internal delegate.
		/// </summary>
		private readonly ScopedBufferAction<T, TState> _action = action;
		/// <summary>
		/// Internal state.
		/// </summary>
		private readonly TState _state = state;

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Invoke(ScopedBuffer<T> buffer) => this._action(buffer, this._state);
	}

	/// <summary>
	/// Internal <see cref="ScopedBufferFunc{T, TResult}"/> action.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="func">A <see cref="ScopedBufferFunc{T, TResult}"/> delegate.</param>
	protected readonly struct FunctionValue<T, TResult>(ScopedBufferFunc<T, TResult> func)
		: IScopedBufferFunction<T, TResult>
	{
		/// <summary>
		/// Internal delegate.
		/// </summary>
		private readonly ScopedBufferFunc<T, TResult> _func = func;

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TResult Invoke(ScopedBuffer<T> buffer) => this._func(buffer);
	}

	/// <summary>
	/// Internal <see cref="ScopedBufferFunc{T, TState, TResult}"/> action.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TState">The type of the state object.</typeparam>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="func">A <see cref="ScopedBufferFunc{T, TState, TResult}"/> delegate.</param>
	/// <param name="state">State object.</param>
#if !NET9_0_OR_GREATER
	protected readonly struct FunctionValue<T, TState, TResult>(ScopedBufferFunc<T, TState, TResult> func, TState state)
		: IScopedBufferFunction<T, TResult>
#else
	protected readonly ref struct FunctionValue<T, TState, TResult>(ScopedBufferFunc<T, TState, TResult> func, TState state)
		: IScopedBufferFunction<T, TResult> where TState : allows ref struct
#endif
	{
		/// <summary>
		/// Internal delegate.
		/// </summary>
		private readonly ScopedBufferFunc<T, TState, TResult> _func = func;
		/// <summary>
		/// Internal state.
		/// </summary>
		private readonly TState _state = state;

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TResult Invoke(ScopedBuffer<T> buffer) => this._func(buffer, this._state);
	}
}