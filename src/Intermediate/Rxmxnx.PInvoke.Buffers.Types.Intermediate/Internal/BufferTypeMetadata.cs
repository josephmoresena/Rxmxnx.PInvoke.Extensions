namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of <see cref="BufferTypeMetadata{T}"/>.
/// </summary>
/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal sealed class BufferTypeMetadata<[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer,
	T> : BufferTypeMetadata<T> where TBuffer : struct, IManagedBuffer<T>
{
#if !NET7_0_OR_GREATER
	private readonly Action<IMetadataStorage>? _appendComponents;
#endif

	/// <inheritdoc/>
	public override Type BufferType => typeof(TBuffer);

#if NET7_0_OR_GREATER
	/// <summary>
	/// Internal implementation of <see cref="BufferTypeMetadata{T}"/>.
	/// </summary>
	/// <param name="capacity">Buffer's capacity.</param>
	/// <param name="isBinary">Indicates if current buffer is binary.</param>
	public BufferTypeMetadata(Int32 capacity, Boolean isBinary = true) : base(
		isBinary, TBuffer.Components, (UInt16)capacity) { }
#endif

	/// <summary>
	/// Internal implementation of <see cref="BufferTypeMetadata{T}"/>.
	/// </summary>
	/// <param name="capacity">Buffer's capacity.</param>
	/// <param name="components">Buffers components.</param>
	/// <param name="isBinary">Indicates if current buffer is binary.</param>
	/// <param name="appendComponents">Append components delegate.</param>
#if !PACKAGE && NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public BufferTypeMetadata(Int32 capacity, BufferTypeMetadata<T>[] components, Boolean isBinary = true,
#if !PACKAGE && NET7_0_OR_GREATER
		[SuppressMessage("ReSharper", "UnusedParameter.Local")]
#endif
		Action<IMetadataStorage>? appendComponents = default) : base(isBinary, components, (UInt16)capacity)
	{
#if !NET7_0_OR_GREATER
		this._appendComponents = appendComponents;
#endif
	}

	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal override void AppendComponent(IMetadataStorage storage)
	{
#if !NET7_0_OR_GREATER
		Debug.Assert(this._appendComponents is not null);
		this._appendComponents(storage);
#else
		TBuffer.AppendComponent(storage);
#endif
	}
	/// <inheritdoc/>
	internal override BufferTypeMetadata<T>? Compose(IMetadataStorage storage, BufferTypeMetadata<T> otherMetadata)
		=> otherMetadata.Compose<TBuffer>(storage);
	/// <inheritdoc/>
	internal override BufferTypeMetadata<T>? Compose<
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TOther>(IMetadataStorage storage)
	{
		if (!BuffersHelper.BufferAutoCompositionEnabled || !this.IsBinary
#if NET7_0_OR_GREATER
		    || !IManagedBuffer<T>.GetMetadata<TOther>().IsBinary
#endif
		   ) return default;
		return BuffersHelper.ComposeWithReflection<T>(storage, typeof(TBuffer), typeof(TOther));
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override void Execute(ScopedBufferAction<T> action, Int32 spanLength)
	{
		TBuffer buffer = default;
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size, this);
		action(scoped);
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override void Execute<TState>(TState state, ScopedBufferAction<T, TState> action, Int32 spanLength)
	{
		TBuffer buffer = default;
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size, this);
		action(scoped, state);
	}
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override void Execute<TState>(TState state, VbScopedBufferAction<T, TState> action, Int32 spanLength)
	{
		TBuffer buffer = default;
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		VbScopedBuffer<T> bufferT = new(ref valRef, (UInt16)spanLength, this);
		try
		{
			action(bufferT, state);
		}
		finally
		{
			bufferT.Unload();
		}
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override TResult Execute<TResult>(ScopedBufferFunc<T, TResult> func, Int32 spanLength)
	{
		TBuffer buffer = default;
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size, this);
		return func(scoped);
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override TResult Execute<TState, TResult>(TState state, ScopedBufferFunc<T, TState, TResult> func,
		Int32 spanLength)
	{
		TBuffer buffer = default;
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		Span<T> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<T> scoped = new(memMarshal, false, this.Size, this);
		return func(scoped, state);
	}
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override TResult Execute<TState, TResult>(TState state, VbScopedBufferFunc<T, TState, TResult> func,
		Int32 spanLength)
	{
		TBuffer buffer = default;
		ref T valRef = ref Unsafe.As<TBuffer, T>(ref buffer);
		VbScopedBuffer<T> bufferT = new(ref valRef, (UInt16)spanLength, this);
		try
		{
			return func(bufferT, state);
		}
		finally
		{
			bufferT.Unload();
		}
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override void Execute<TU, TState>(TState state, ScopedBufferAction<TU, TState> action, Int32 spanLength)
	{
		TBuffer buffer = default;
		ref TU valRef = ref Unsafe.As<TBuffer, TU>(ref buffer);
		Span<TU> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<TU> scoped = new(memMarshal, false, this.Size, this);
		action(scoped, state);
	}
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override void Execute<TU, TState>(TState state, VbScopedBufferAction<TU, TState> action, Int32 spanLength)
	{
		TBuffer buffer = default;
		ref TU valRef = ref Unsafe.As<TBuffer, TU>(ref buffer);
		VbScopedBuffer<TU> bufferT = new(ref valRef, (UInt16)spanLength, this);
		try
		{
			action(bufferT, state);
		}
		finally
		{
			bufferT.Unload();
		}
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override TResult Execute<TU, TState, TResult>(TState state, ScopedBufferFunc<TU, TState, TResult> func,
		Int32 spanLength)
	{
		TBuffer buffer = default;
		ref TU valRef = ref Unsafe.As<TBuffer, TU>(ref buffer);
		Span<TU> memMarshal = MemoryMarshal.CreateSpan(ref valRef, spanLength);
		ScopedBuffer<TU> scoped = new(memMarshal, false, this.Size, this);
		return func(scoped, state);
	}
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal override TResult Execute<TU, TState, TResult>(TState state, VbScopedBufferFunc<TU, TState, TResult> func,
		Int32 spanLength)
	{
		TBuffer buffer = default;
		ref TU valRef = ref Unsafe.As<TBuffer, TU>(ref buffer);
		VbScopedBuffer<TU> bufferT = new(ref valRef, (UInt16)spanLength, this);
		try
		{
			return func(bufferT, state);
		}
		finally
		{
			bufferT.Unload();
		}
	}
}