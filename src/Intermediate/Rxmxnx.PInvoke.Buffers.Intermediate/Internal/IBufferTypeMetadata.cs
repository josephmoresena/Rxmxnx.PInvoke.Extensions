namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// This interface exposes a buffer type metadata.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
internal interface IBufferTypeMetadata<T>
{
	/// <summary>
	/// Buffer capacity.
	/// </summary>
	UInt16 Size { get; }

	/// <summary>
	/// Composes a new buffer using current buffer type and <paramref name="otherBuffer"/>.
	/// </summary>
	/// <param name="otherBuffer">A <see cref="IBufferTypeMetadata{T}"/> instance.</param>
	/// <returns>A composed <see cref="IBufferTypeMetadata{T}"/>.</returns>
	IBufferTypeMetadata<T>? Compose(IBufferTypeMetadata<T> otherBuffer);
	/// <summary>
	/// Composes a new buffer using current buffer type and <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Other buffer type.</typeparam>
	/// <returns>A composed <see cref="IBufferTypeMetadata{T}"/>.</returns>
#if NET6_0
	[RequiresPreviewFeatures]
#endif
	IBufferTypeMetadata<T>? Compose<TBuffer>() where TBuffer : struct, IAllocatedBuffer<T>;
	/// <summary>
	/// Composes a new buffer using twice the current buffer type.
	/// </summary>
	/// <returns>A composed <see cref="IBufferTypeMetadata{T}"/>.</returns>
	IBufferTypeMetadata<T>? Double() => this.Compose(this);

	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type.
	/// </summary>
	/// <param name="action">A <see cref="AllocatedBufferAction{T}"/> delegate.</param>
	void Execute(AllocatedBufferAction<T> action);
	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type and given state object.
	/// </summary>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <param name="state">State object.</param>
	/// <param name="action">A <see cref="AllocatedBufferAction{T, TState}"/> delegate.</param>
	void Execute<TState>(TState state, AllocatedBufferAction<T, TState> action);
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">A <see cref="AllocatedBufferFunc{T, TResult}"/> delegate.</param>
	TResult Execute<TResult>(AllocatedBufferFunc<T, TResult> func);
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type and given state object.
	/// </summary>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="state">State object.</param>
	/// <param name="func">A <see cref="AllocatedBufferFunc{T, TState, TResult}"/> delegate.</param>
	TResult Execute<TState, TResult>(TState state, AllocatedBufferFunc<T, TState, TResult> func);
}