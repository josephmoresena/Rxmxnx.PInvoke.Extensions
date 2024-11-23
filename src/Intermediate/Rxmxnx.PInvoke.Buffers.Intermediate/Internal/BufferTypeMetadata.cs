namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of <see cref="BufferTypeMetadata{T}"/>.
/// </summary>
/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
/// <param name="capacity">Buffer's capacity.</param>
/// <param name="isBinary">Indicates if current buffer is binary.</param>
internal sealed class BufferTypeMetadata<TBuffer, T>(Int32 capacity, Boolean isBinary = true)
	: BufferTypeMetadata<T> where TBuffer : struct, IManagedBuffer<T>
{
	/// <inheritdoc/>
	public override Boolean IsBinary { get; } = isBinary;
	/// <inheritdoc/>
#pragma warning disable CA2252
	public override BufferTypeMetadata<T>[] Components { get; } = TBuffer.Components;
#pragma warning restore CA2252
	/// <inheritdoc/>
	public override UInt16 Size { get; } = (UInt16)capacity;
	/// <inheritdoc/>
	public override BufferTypeMetadata<T>? Compose(BufferTypeMetadata<T> otherMetadata)
		=> otherMetadata.Compose<TBuffer>();
	/// <inheritdoc/>
	public override BufferTypeMetadata<T>? Compose<TOther>()
		=> BufferManager.BufferAutoCompositionEnabled ?
			BufferManager.MetadataManager<T>.ComposeWithReflection(typeof(TBuffer), typeof(TOther)) :
			default;
	/// <inheritdoc/>
	public override void Execute(AllocatedBufferAction<T> action, Int32 spanLength)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size);
		action(scoped);
	}
	/// <inheritdoc/>
	public override void Execute<TState>(in TState state, AllocatedBufferAction<T, TState> action, Int32 spanLength)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size);
		action(scoped, state);
	}
	/// <inheritdoc/>
	public override TResult Execute<TResult>(AllocatedBufferFunc<T, TResult> func, Int32 spanLength)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size);
		return func(scoped);
	}
	/// <inheritdoc/>
	public override TResult Execute<TState, TResult>(in TState state, AllocatedBufferFunc<T, TState, TResult> func,
		Int32 spanLength)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size);
		return func(scoped, state);
	}
	/// <inheritdoc/>
	internal override void Execute<TU, TState>(in TState state, AllocatedBufferAction<TU, TState> action,
		Int32 spanLength)
	{
		TBuffer buffer = new();
		ref TU valRef = ref Unsafe.As<TBuffer, TU>(ref buffer);
		Span<TU> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<TU> scoped = new(memMarshal, false, this.Size);
		action(scoped, state);
	}
	/// <inheritdoc/>
	internal override TResult Execute<TU, TState, TResult>(in TState state,
		AllocatedBufferFunc<TU, TState, TResult> func, Int32 spanLength)
	{
		TBuffer buffer = new();
		ref TU valRef = ref Unsafe.As<TBuffer, TU>(ref buffer);
		Span<TU> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<TU> scoped = new(memMarshal, false, this.Size);
		return func(scoped, state);
	}
}