namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of <see cref="IBufferTypeMetadata{T}"/>.
/// </summary>
/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
/// <param name="capacity">Buffer's capacity.</param>
#pragma warning disable CA2252
internal sealed class BufferTypeMetadata<TBuffer, T>(Int32 capacity)
	: IBufferTypeMetadata<T> where TBuffer : struct, IAllocatedBuffer<T>
{
	/// <inheritdoc/>
	public UInt16 Size { get; } = (UInt16)capacity;
	/// <inheritdoc/>
	public IBufferTypeMetadata<T>? Compose(IBufferTypeMetadata<T> otherBuffer) => otherBuffer.Compose<TBuffer>();
	/// <inheritdoc/>
	public IBufferTypeMetadata<T>? Compose<TOther>() where TOther : struct, IAllocatedBuffer<T>
		=> AllocatedBuffer.MetadataCache<T>.CreateComposedWithReflection<TBuffer, TOther>();
	/// <inheritdoc/>
	public void Execute(AllocatedBufferAction<T> action)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, this.Size);
		AllocatedBuffer<T> allocated = new(memMarshal);
		action(allocated);
	}
	/// <inheritdoc/>
	public void Execute<TState>(TState state, AllocatedBufferAction<T, TState> action)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, this.Size);
		AllocatedBuffer<T> allocated = new(memMarshal);
		action(allocated, state);
	}
	/// <inheritdoc/>
	public TResult Execute<TResult>(AllocatedBufferFunc<T, TResult> func)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, this.Size);
		AllocatedBuffer<T> allocated = new(memMarshal);
		return func(allocated);
	}
	/// <inheritdoc/>
	public TResult Execute<TState, TResult>(TState state, AllocatedBufferFunc<T, TState, TResult> func)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, this.Size);
		AllocatedBuffer<T> allocated = new(memMarshal);
		return func(allocated, state);
	}
}
#pragma warning restore CA2252