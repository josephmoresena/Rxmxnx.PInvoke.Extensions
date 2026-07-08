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
		if (this._appendComponents is null)
		{
			// Non-Binary buffer.
			base.AppendComponent(storage);
			return;
		}
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal override void Execute<TAction>(in TAction action, Int32 spanLength)
		=> BufferTypeMetadata.Execute<T, TBuffer, TAction>(in action, this, spanLength);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal override TResult Execute<TFunction, TResult>(in TFunction func, Int32 spanLength)
		=> BufferTypeMetadata.Execute<T, TBuffer, TFunction, TResult>(in func, this, spanLength);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal override void Execute<TU, TAction>(in TAction action, Int32 spanLength)
		=> BufferTypeMetadata.Execute<TU, TBuffer, TAction>(in action, this, spanLength);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal override TResult Execute<TU, TFunction, TResult>(in TFunction func, Int32 spanLength)
		=> BufferTypeMetadata.Execute<TU, TBuffer, TFunction, TResult>(in func, this, spanLength);
}