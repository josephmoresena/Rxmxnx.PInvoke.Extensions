namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of <see cref="ManagedBufferMetadata{T}"/>.
/// </summary>
/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
/// <param name="capacity">Buffer's capacity.</param>
#pragma warning disable CA2252
internal sealed class BufferTypeMetadata<TBuffer, T>(Int32 capacity)
	: ManagedBufferMetadata<T> where TBuffer : struct, IManagedBuffer<T>
{
	/// <inheritdoc/>
	public override ManagedBufferMetadata<T>[] Components { get; } = TBuffer.Components;
	/// <inheritdoc/>
	public override UInt16 Size { get; } = (UInt16)capacity;
	/// <inheritdoc/>
	public override ManagedBufferMetadata<T>? Compose(ManagedBufferMetadata<T> otherBuffer)
		=> otherBuffer.Compose<TBuffer>();
	/// <inheritdoc/>
	public override ManagedBufferMetadata<T>? Compose<TOther>()
		=> AllocatedBuffer.BufferAutoCompositionEnabled ?
			AllocatedBuffer.MetadataCache<T>.CreateComposedWithReflection(typeof(TBuffer), typeof(TOther)) :
			default;
	/// <inheritdoc/>
	public override void Execute(AllocatedBufferAction<T> action, Int32 spanLength)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		AllocatedBuffer<T> allocated = new(memMarshal, false, this.Size);
		action(allocated);
	}
	/// <inheritdoc/>
	public override void Execute<TState>(in TState state, AllocatedBufferAction<T, TState> action, Int32 spanLength)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		AllocatedBuffer<T> allocated = new(memMarshal, false, this.Size);
		action(allocated, state);
	}
	/// <inheritdoc/>
	public override TResult Execute<TResult>(AllocatedBufferFunc<T, TResult> func, Int32 spanLength)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		AllocatedBuffer<T> allocated = new(memMarshal, false, this.Size);
		return func(allocated);
	}
	/// <inheritdoc/>
	public override TResult Execute<TState, TResult>(in TState state, AllocatedBufferFunc<T, TState, TResult> func,
		Int32 spanLength)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		AllocatedBuffer<T> allocated = new(memMarshal, false, this.Size);
		return func(allocated, state);
	}
	/// <inheritdoc/>
	internal override void Execute<TU, TState>(in TState state, AllocatedBufferAction<TU, TState> action,
		Int32 spanLength)
	{
		TBuffer buffer = new();
		ref TU valRef = ref Unsafe.As<TBuffer, TU>(ref buffer);
		Span<TU> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		AllocatedBuffer<TU> allocated = new(memMarshal, false, this.Size);
		action(allocated, state);
	}
	/// <inheritdoc/>
	internal override TResult Execute<TU, TState, TResult>(in TState state,
		AllocatedBufferFunc<TU, TState, TResult> func, Int32 spanLength)
	{
		TBuffer buffer = new();
		ref TU valRef = ref Unsafe.As<TBuffer, TU>(ref buffer);
		Span<TU> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		AllocatedBuffer<TU> allocated = new(memMarshal, false, this.Size);
		return func(allocated, state);
	}
}
#pragma warning restore CA2252