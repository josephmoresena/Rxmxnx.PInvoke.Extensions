namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents the metadata of a managed buffer type.
/// </summary>
public abstract class BufferTypeMetadata : IEnumerableSequence<BufferTypeMetadata>
{
	/// <summary>
	/// Indicates whether current type is binary space.
	/// </summary>
	public Boolean IsBinary { get; }
	/// <summary>
	/// Buffer capacity.
	/// </summary>
	public UInt16 Size { get; }
	/// <summary>
	/// Number of components.
	/// </summary>
	public abstract Int32 ComponentCount { get; }
	/// <summary>
	/// Buffer type.
	/// </summary>
	public abstract Type BufferType { get; }
	/// <summary>
	/// Retrieves a component from current metadata at the specified zero-based <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The zero-based index of the component to retrieve.</param>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when <paramref name="index"/> is less than zero or greater than or equal to
	/// the count of the components.
	/// </exception>
	/// <returns>The component at the specified index within the buffer metadata.</returns>
	[ExcludeFromCodeCoverage]
	[IndexerName("Item")]
	public abstract BufferTypeMetadata this[Int32 index] { get; }

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="isBinary">Indicates if current buffer is binary.</param>
	/// <param name="capacity">Buffer's capacity.</param>
	private protected BufferTypeMetadata(Boolean isBinary, UInt16 capacity)
	{
		this.IsBinary = isBinary;
		this.Size = capacity;
	}

	[ExcludeFromCodeCoverage]
	BufferTypeMetadata IEnumerableSequence<BufferTypeMetadata>.GetItem(Int32 index) => this[index];
	[ExcludeFromCodeCoverage]
	Int32 IEnumerableSequence<BufferTypeMetadata>.GetSize() => this.ComponentCount;
}

/// <summary>
/// Represents the metadata of a managed buffer type.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
public abstract class BufferTypeMetadata<T> : BufferTypeMetadata
{
	/// <summary>
	/// Current buffer components.
	/// </summary>
	internal BufferTypeMetadata<T>[] Components { get; }

	/// <inheritdoc/>
	public override BufferTypeMetadata this[Int32 index] => this.Components[index];
	/// <inheritdoc/>
	public override Int32 ComponentCount => this.Components.Length;

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="isBinary">Indicates if current buffer is binary.</param>
	/// <param name="components">Buffer's components.</param>
	/// <param name="capacity">Buffer's capacity.</param>
	private protected BufferTypeMetadata(Boolean isBinary, BufferTypeMetadata<T>[] components, UInt16 capacity) :
		base(isBinary, capacity)
		=> this.Components = components;

	/// <summary>
	/// Composes a new buffer using twice the current buffer type.
	/// </summary>
	/// <returns>A composed <see cref="BufferTypeMetadata{T}"/>.</returns>
	internal BufferTypeMetadata<T>? Double() => this.Compose(this);
	/// <summary>
	/// Composes a new buffer using current buffer type and <paramref name="otherMetadata"/>.
	/// </summary>
	/// <param name="otherMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	/// <returns>A composed <see cref="BufferTypeMetadata{T}"/>.</returns>
	internal abstract BufferTypeMetadata<T>? Compose(BufferTypeMetadata<T> otherMetadata);
	/// <summary>
	/// Composes a new buffer using current buffer type and <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Other buffer type.</typeparam>
	/// <returns>A composed <see cref="BufferTypeMetadata{T}"/>.</returns>
	internal abstract BufferTypeMetadata<T>? Compose<TBuffer>() where TBuffer : struct, IManagedBuffer<T>;
	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type.
	/// </summary>
	/// <param name="action">A <see cref="ScopedBufferAction{T}"/> delegate.</param>
	/// <param name="spanLength">Required span length.</param>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal abstract void Execute(ScopedBufferAction<T> action, Int32 spanLength);
	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type and given state object.
	/// </summary>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <param name="state">State object.</param>
	/// <param name="action">A <see cref="ScopedBufferAction{T,TArg}"/> delegate.</param>
	/// <param name="spanLength">Required span length.</param>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal abstract void Execute<TState>(TState state, ScopedBufferAction<T, TState> action, Int32 spanLength)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
		;
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">A <see cref="ScopedBufferFunc{T,TResult}"/> delegate.</param>
	/// <param name="spanLength">Required span length.</param>
	/// <returns><paramref name="func"/> result.</returns>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal abstract TResult Execute<TResult>(ScopedBufferFunc<T, TResult> func, Int32 spanLength);
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type and given state object.
	/// </summary>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="state">State object.</param>
	/// <param name="func">A <see cref="ScopedBufferFunc{T,TArg,TResult}"/> delegate.</param>
	/// <param name="spanLength">Required span length.</param>
	/// <returns><paramref name="func"/> result.</returns>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal abstract TResult Execute<TState, TResult>(TState state, ScopedBufferFunc<T, TState, TResult> func,
			Int32 spanLength)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
		;
	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type and given state object.
	/// </summary>
	/// <typeparam name="TU">Type of transformation state object.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <param name="state">State object.</param>
	/// <param name="action">A <see cref="ScopedBufferAction{T,TArg}"/> delegate.</param>
	/// <param name="spanLength">Required span length.</param>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal abstract void Execute<TU, TState>(TState state, ScopedBufferAction<TU, TState> action, Int32 spanLength)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
		;
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type and given state object.
	/// </summary>
	/// <typeparam name="TU">Type of transformation state object.</typeparam>
	/// <typeparam name="TState">Type of state object.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="state">State object.</param>
	/// <param name="func">A <see cref="ScopedBufferFunc{T,TArg,TResult}"/> delegate.</param>
	/// <param name="spanLength">Required span length.</param>
	/// <returns><paramref name="func"/> result.</returns>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal abstract TResult Execute<TU, TState, TResult>(TState state, ScopedBufferFunc<TU, TState, TResult> func,
			Int32 spanLength)
#if NET9_0_OR_GREATER
		where TState : allows ref struct
#endif
		;
}