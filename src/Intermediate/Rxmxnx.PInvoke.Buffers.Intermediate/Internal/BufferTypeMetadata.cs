namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of <see cref="BufferTypeMetadata{T}"/>.
/// </summary>
/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal class BufferTypeMetadata<[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer,
	T> : BufferTypeMetadata<T> where TBuffer : struct, IManagedBuffer<T>
{
	/// <inheritdoc/>
	public sealed override Type BufferType => typeof(TBuffer);
	/// <inheritdoc/>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#pragma warning disable CS8500
	public sealed override unsafe Int32 SizeOf => sizeof(TBuffer);
#pragma warning restore CS8500
	/// <inheritdoc/>
	public sealed override Int32 SizeOfElement => base.SizeOfElement;

	/// <summary>
	/// Internal implementation of <see cref="BufferTypeMetadata{T}"/>.
	/// </summary>
	/// <param name="capacity">Buffer's capacity.</param>
	/// <param name="isBinary">Indicates if current buffer is binary.</param>
	public BufferTypeMetadata(Int32 capacity, Boolean isBinary = true) : base(isBinary, (UInt16)capacity) { }

	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal override void AppendComponent(IMetadataStorage storage)
	{
		if (!this.IsBinary) return;
#if NET7_0_OR_GREATER
		TBuffer.AppendComponent(storage);
#endif
	}
	/// <inheritdoc/>
	internal sealed override BufferTypeMetadata<T>? Compose(IMetadataStorage storage,
		BufferTypeMetadata<T> otherMetadata)
	{
		Composition composition = new(typeof(T), (UInt16)(this.Size + otherMetadata.Size));
		if (BufferTypeMetadata.HasError(composition)) return default;
		BufferTypeMetadata<T>? result = otherMetadata.Compose<TBuffer>(storage);
		if (result is null && BuffersHelper.BufferAutoCompositionEnabled) BufferTypeMetadata.SetError(composition);
		return result;
	}
	/// <inheritdoc/>
	internal sealed override BufferTypeMetadata<T>? Compose<
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
	internal sealed override void Execute<TAction>(ref TAction action, Int32 spanLength)
		=> BufferTypeMetadata.Execute<T, TBuffer, TAction>(ref action, this, spanLength);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal sealed override TResult Execute<TFunction, TResult>(ref TFunction func, Int32 spanLength)
		=> BufferTypeMetadata.Execute<T, TBuffer, TFunction, TResult>(ref func, this, spanLength);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal sealed override void Execute<TU, TAction>(ref TAction action, Int32 spanLength)
		=> BufferTypeMetadata.Execute<TU, TBuffer, TAction>(ref action, this, spanLength);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal sealed override TResult Execute<TU, TFunction, TResult>(ref TFunction func, Int32 spanLength)
		=> BufferTypeMetadata.Execute<TU, TBuffer, TFunction, TResult>(ref func, this, spanLength);
}