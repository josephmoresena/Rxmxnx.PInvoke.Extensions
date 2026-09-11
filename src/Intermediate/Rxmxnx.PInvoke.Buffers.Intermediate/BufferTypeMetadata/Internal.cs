namespace Rxmxnx.PInvoke;

public abstract partial class BufferTypeMetadata
{
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
}

public abstract partial class BufferTypeMetadata<T>
{
	/// <summary>
	/// Current buffer components.
	/// </summary>
	internal ReadOnlyMemory<BufferTypeMetadata<T>> Components { get; }

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="isBinary">Indicates if the current buffer is binary.</param>
	/// <param name="components">Buffer's components.</param>
	/// <param name="capacity">Buffer's capacity.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private protected BufferTypeMetadata(Boolean isBinary, BufferTypeMetadata<T>[] components, UInt16 capacity) :
		base(isBinary, capacity)
		=> this.Components = components;

	/// <summary>
	/// Appends all components from current buffer type.
	/// </summary>
	/// <param name="storage">A <see cref="IMetadataStorage"/> instance.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal virtual void AppendComponent(IMetadataStorage storage) { }
	/// <summary>
	/// Composes a new buffer using twice the current buffer type.
	/// </summary>
	/// <param name="storage">A <see cref="IMetadataStorage"/> instance.</param>
	/// <returns>A composed <see cref="BufferTypeMetadata{T}"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal BufferTypeMetadata<T>? Double(IMetadataStorage storage) => this.Compose(storage, this);
	/// <summary>
	/// Composes a new buffer using current buffer type and <paramref name="otherMetadata"/>.
	/// </summary>
	/// <param name="storage">A <see cref="IMetadataStorage"/> instance.</param>
	/// <param name="otherMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	/// <returns>A composed <see cref="BufferTypeMetadata{T}"/>.</returns>
	internal abstract BufferTypeMetadata<T>? Compose(IMetadataStorage storage, BufferTypeMetadata<T> otherMetadata);
	/// <summary>
	/// Composes a new buffer using current buffer type and <typeparamref name="TBuffer"/>.
	/// </summary>
	/// <typeparam name="TBuffer">Other buffer type.</typeparam>
	/// <param name="storage">A <see cref="IMetadataStorage"/> instance.</param>
	/// <returns>A composed <see cref="BufferTypeMetadata{T}"/>.</returns>
	internal abstract BufferTypeMetadata<T>? Compose<
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer>(IMetadataStorage storage)
		where TBuffer : struct, IManagedBuffer<T>;
	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/> interface.</typeparam>
	/// <param name="action">A <see cref="IScopedBufferAction{T}"/> instance.</param>
	/// <param name="spanLength">Required span length.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal abstract void Execute<TAction>(ref TAction action, Int32 spanLength)
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<T>;
#else
		where TAction : IScopedBufferAction<T>, allows ref struct;
#endif
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, Result}"/> interface.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">A <see cref="IScopedBufferFunction{T,TResult}"/> instance.</param>
	/// <param name="spanLength">Required span length.</param>
	/// <returns><paramref name="func"/> result.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal abstract TResult Execute<TFunction, TResult>(ref TFunction func, Int32 spanLength)
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<T, TResult>;
#else
		where TFunction : IScopedBufferFunction<T, TResult>, allows ref struct;
#endif
	/// <summary>
	/// Executes <paramref name="action"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="TU">Type of elements exposed to the operation.</typeparam>
	/// <typeparam name="TAction">Type of <see cref="IScopedBufferAction{T}"/> interface.</typeparam>
	/// <param name="action">A <see cref="IScopedBufferAction{T}"/> instance.</param>
	/// <param name="spanLength">Required span length.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal abstract void Execute<TU, TAction>(ref TAction action, Int32 spanLength)
#if !NET9_0_OR_GREATER
		where TAction : IScopedBufferAction<TU>;
#else
		where TAction : IScopedBufferAction<TU>, allows ref struct;
#endif
	/// <summary>
	/// Executes <paramref name="func"/> using a buffer of current type.
	/// </summary>
	/// <typeparam name="TU">Type of elements exposed to the operation.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IScopedBufferFunction{T, Result}"/> interface.</typeparam>
	/// <typeparam name="TResult">Type of <paramref name="func"/> result.</typeparam>
	/// <param name="func">A <see cref="IScopedBufferFunction{T,TResult}"/> instance.</param>
	/// <param name="spanLength">Required span length.</param>
	/// <returns><paramref name="func"/> result.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal abstract TResult Execute<TU, TFunction, TResult>(ref TFunction func, Int32 spanLength)
#if !NET9_0_OR_GREATER
		where TFunction : IScopedBufferFunction<TU, TResult>;
#else
		where TFunction : IScopedBufferFunction<TU, TResult>, allows ref struct;
#endif
}