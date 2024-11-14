namespace Rxmxnx.PInvoke.Internal;

#if NET6_0
[RequiresPreviewFeatures]
#endif
internal class BufferTypeMetadata<TBuffer, T>(Int32 capacity)
	: IBufferTypeMetadata<T> where TBuffer : struct, IAllocatedBuffer<T>
{
	/// <inheritdoc/>
	public UInt16 Size { get; } = (UInt16)capacity;

	/// <inheritdoc/>
	public IBufferTypeMetadata<T>? Compose(IBufferTypeMetadata<T> otherBuffer) => otherBuffer.Compose<TBuffer>();

	/// <inheritdoc/>
	public IBufferTypeMetadata<T>? Compose<TOther>() where TOther : struct, IAllocatedBuffer<T>
		=> AllocatedBuffer.MetadataCache<T>.Get<TBuffer, TOther>();

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