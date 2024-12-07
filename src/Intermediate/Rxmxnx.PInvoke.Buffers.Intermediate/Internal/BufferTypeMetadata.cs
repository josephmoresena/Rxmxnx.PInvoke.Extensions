namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of <see cref="BufferTypeMetadata{T}"/>.
/// </summary>
/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal sealed class BufferTypeMetadata<TBuffer, T> : BufferTypeMetadata<T> where TBuffer : struct, IManagedBuffer<T>
{
	/// <summary>
	/// Internal implementation of <see cref="BufferTypeMetadata{T}"/>.
	/// </summary>
	/// <param name="capacity">Buffer's capacity.</param>
	/// <param name="isBinary">Indicates if current buffer is binary.</param>
#pragma warning disable CA2252
	public BufferTypeMetadata(Int32 capacity, Boolean isBinary = true) : base(
		isBinary, TBuffer.Components, (UInt16)capacity)
#pragma warning restore CA2252
	{ }

	/// <inheritdoc/>
	public override BufferTypeMetadata<T>? Compose(BufferTypeMetadata<T> otherMetadata)
		=> otherMetadata.Compose<TBuffer>();
	/// <inheritdoc/>
	public override BufferTypeMetadata<T>? Compose<TOther>()
	{
		if (!BufferManager.BufferAutoCompositionEnabled || !this.IsBinary ||
		    !IManagedBuffer<T>.GetMetadata<TOther>().IsBinary) return default;
		return BufferManager.MetadataManager<T>.ComposeWithReflection(typeof(TBuffer), typeof(TOther));
	}
	/// <inheritdoc/>
	public override void Execute(ScopedBufferAction<T> action, Int32 spanLength, out Boolean allocated)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size);
		allocated = true;
		action(scoped);
	}
	/// <inheritdoc/>
	public override void Execute<TState>(in TState state, ScopedBufferAction<T, TState> action, Int32 spanLength,
		out Boolean allocated)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size);
		allocated = true;
		action(scoped, state);
	}
	/// <inheritdoc/>
	public override TResult Execute<TResult>(ScopedBufferFunc<T, TResult> func, Int32 spanLength, out Boolean allocated)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size);
		allocated = true;
		return func(scoped);
	}
	/// <inheritdoc/>
	public override TResult Execute<TState, TResult>(in TState state, ScopedBufferFunc<T, TState, TResult> func,
		Int32 spanLength, out Boolean allocated)
	{
		TBuffer buffer = new();
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size);
		allocated = true;
		return func(scoped, state);
	}
	/// <inheritdoc/>
	internal override void Execute<TU, TState>(in TState state, ScopedBufferAction<TU, TState> action, Int32 spanLength,
		out Boolean allocated)
	{
		TBuffer buffer = new();
		ref TU valRef = ref Unsafe.As<TBuffer, TU>(ref buffer);
		Span<TU> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<TU> scoped = new(memMarshal, false, this.Size);
		allocated = true;
		action(scoped, state);
	}
	/// <inheritdoc/>
	internal override TResult Execute<TU, TState, TResult>(in TState state, ScopedBufferFunc<TU, TState, TResult> func,
		Int32 spanLength, out Boolean allocated)
	{
		TBuffer buffer = new();
		ref TU valRef = ref Unsafe.As<TBuffer, TU>(ref buffer);
		Span<TU> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<TU> scoped = new(memMarshal, false, this.Size);
		allocated = true;
		return func(scoped, state);
	}
}