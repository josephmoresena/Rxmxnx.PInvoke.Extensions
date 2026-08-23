#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static partial class BufferManager
{
	/// <summary>
	/// Internal <see cref="ScopedBufferAction{T}"/> action.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="action">A <see cref="ScopedBufferAction{T}"/> delegate.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
	[Obsolete]
#endif
	private readonly struct ActionValue<T>(ScopedBufferAction<T> action) : IScopedBufferAction<T>
	{
		/// <summary>
		/// Internal delegate.
		/// </summary>
		private readonly ScopedBufferAction<T> _action = action;

		/// <inheritdoc/>
		public Boolean IsMinimalCount { get; init; }
		/// <inheritdoc/>
		public UInt16 Count { get; init; }

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Accept(ScopedBuffer<T> buffer) => this._action(buffer);
	}

	/// <summary>
	/// Internal <see cref="ScopedBufferAction{T, TState}"/> action.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TState">The type of the state object.</typeparam>
	/// <param name="action">A <see cref="ScopedBufferAction{T}"/> delegate.</param>
	/// <param name="state">State object.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
	[Obsolete]
#endif
#if !NET9_0_OR_GREATER
	private readonly struct ActionValue<T, TState>(ScopedBufferAction<T, TState> action, TState state)
		: IScopedBufferAction<T>
#else
	private readonly ref struct ActionValue<T, TState>(ScopedBufferAction<T, TState> action, TState state)
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
		public Boolean IsMinimalCount { get; init; }
		/// <inheritdoc/>
		public UInt16 Count { get; init; }

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Accept(ScopedBuffer<T> buffer) => this._action(buffer, this._state);
	}

	/// <summary>
	/// Internal <see cref="ScopedBufferAction{T}"/> action.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="func">A <see cref="ScopedBufferFunc{T, TResult}"/> delegate.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
	[Obsolete]
#endif
	private readonly struct FunctionValue<T, TResult>(ScopedBufferFunc<T, TResult> func)
		: IScopedBufferFunction<T, TResult>
	{
		/// <summary>
		/// Internal delegate.
		/// </summary>
		private readonly ScopedBufferFunc<T, TResult> _func = func;

		/// <inheritdoc/>
		public Boolean IsMinimalCount { get; init; }
		/// <inheritdoc/>
		public UInt16 Count { get; init; }

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TResult Apply(ScopedBuffer<T> buffer) => this._func(buffer);
	}

	/// <summary>
	/// Internal <see cref="ScopedBufferAction{T, TState}"/> action.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <typeparam name="TState">The type of the state object.</typeparam>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="func">A <see cref="ScopedBufferFunc{T, TState, TResult}"/> delegate.</param>
	/// <param name="state">State object.</param>
#if OBSOLTE_DELEGATES && !GITHUB_ACTIONS
	[Obsolete]
#endif
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
#endif
#if !NET9_0_OR_GREATER
	private readonly struct FunctionValue<T, TState, TResult>(ScopedBufferFunc<T, TState, TResult> func, TState state)
		: IScopedBufferFunction<T, TResult>
#else
	private readonly ref struct FunctionValue<T, TState, TResult>(ScopedBufferFunc<T, TState, TResult> func, TState state)
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
		public Boolean IsMinimalCount { get; init; }
		/// <inheritdoc/>
		public UInt16 Count { get; init; }

		/// <inheritdoc/>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TResult Apply(ScopedBuffer<T> buffer) => this._func(buffer, this._state);
	}
}
#endif