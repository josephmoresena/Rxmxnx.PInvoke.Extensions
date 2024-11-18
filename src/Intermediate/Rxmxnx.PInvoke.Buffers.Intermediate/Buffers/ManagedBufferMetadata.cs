namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a buffer type metadata.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#pragma warning disable CA2252
public abstract class ManagedBufferMetadata<T>
{
	/// <summary>
	/// Current buffer components.
	/// </summary>
	public abstract ManagedBufferMetadata<T>[] Components { get; }
	/// <summary>
	/// Buffer capacity.
	/// </summary>
	public abstract UInt16 Size { get; }
	/// <summary>
	/// Composes a new buffer using current buffer type and <paramref name="otherBuffer"/>.
	/// </summary>
	/// <param name="otherBuffer">A <see cref="ManagedBufferMetadata{T}"/> instance.</param>
	/// <returns>A composed <see cref="ManagedBufferMetadata{T}"/>.</returns>
	public abstract ManagedBufferMetadata<T>? Compose(ManagedBufferMetadata<T> otherBuffer);
	/// <summary>
	/// Composes a new buffer using current buffer type and <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Other buffer type.</typeparam>
	/// <returns>A composed <see cref="ManagedBufferMetadata{T}"/>.</returns>
	public abstract ManagedBufferMetadata<T>? Compose<TBuffer>() where TBuffer : struct, IManagedBuffer<T>;
	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type.
	/// </summary>
	/// <param name="action">A <see cref="AllocatedBufferAction{T}"/> delegate.</param>
	/// <param name="spanLength">Required span length.</param>
	public abstract void Execute(AllocatedBufferAction<T> action, Int32 spanLength);
	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type and given state object.
	/// </summary>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <param name="state">State object.</param>
	/// <param name="action">A <see cref="AllocatedBufferAction{T, TState}"/> delegate.</param>
	/// <param name="spanLength">Required span length.</param>
	public abstract void Execute<TState>(in TState state, AllocatedBufferAction<T, TState> action, Int32 spanLength);
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">A <see cref="AllocatedBufferFunc{T, TResult}"/> delegate.</param>
	/// <param name="spanLength">Required span length.</param>
	public abstract TResult Execute<TResult>(AllocatedBufferFunc<T, TResult> func, Int32 spanLength);
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type and given state object.
	/// </summary>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="state">State object.</param>
	/// <param name="func">A <see cref="AllocatedBufferFunc{T, TState, TResult}"/> delegate.</param>
	/// <param name="spanLength">Required span length.</param>
	public abstract TResult Execute<TState, TResult>(in TState state, AllocatedBufferFunc<T, TState, TResult> func,
		Int32 spanLength);
	/// <summary>
	/// Composes a new buffer using twice the current buffer type.
	/// </summary>
	/// <returns>A composed <see cref="ManagedBufferMetadata{T}"/>.</returns>
	internal virtual ManagedBufferMetadata<T>? Double() => this.Compose(this);
	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type and given state object.
	/// </summary>
	/// <typeparam name="TU">Type of transformation state object.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <param name="state">State object.</param>
	/// <param name="action">A <see cref="AllocatedBufferAction{T, TState}"/> delegate.</param>
	/// <param name="spanLength">Required span length.</param>
	internal abstract void Execute<TU, TState>(in TState state, AllocatedBufferAction<TU, TState> action,
		Int32 spanLength);
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type and given state object.
	/// </summary>
	/// <typeparam name="TU">Type of transformation state object.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="state">State object.</param>
	/// <param name="func">A <see cref="AllocatedBufferFunc{T, TState, TResult}"/> delegate.</param>
	/// <param name="spanLength">Required span length.</param>
	internal abstract TResult Execute<TU, TState, TResult>(in TState state,
		AllocatedBufferFunc<TU, TState, TResult> func, Int32 spanLength);
}
#pragma warning restore CA2252