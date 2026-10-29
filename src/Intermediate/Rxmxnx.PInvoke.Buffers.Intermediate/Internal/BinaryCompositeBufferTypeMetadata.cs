namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Metadata of a binary <see cref="Composite{TBufferA,TBufferB,T}"/>.
/// </summary>
/// <typeparam name="TBufferA">The type of low buffer.</typeparam>
/// <typeparam name="TBufferB">The type of high buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal sealed class BinaryCompositeBufferTypeMetadata<
	[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBufferA,
	[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBufferB, T>
	: BufferTypeMetadata<Composite<TBufferA, TBufferB, T>, T>
	where TBufferA : struct, IManagedBinaryBuffer<TBufferA, T>
	where TBufferB : struct, IManagedBinaryBuffer<TBufferB, T>
{
	/// <inheritdoc/>
	public override Int32 ComponentCount => 2;
	/// <inheritdoc/>
	public override BufferTypeMetadata this[Int32 index] => index switch
	{
		0 => BufferTypeMetadata<T>.GetMetadata<TBufferA>(),
		1 => BufferTypeMetadata<T>.GetMetadata<TBufferB>(),
		_ => throw new IndexOutOfRangeException(),
	};

	/// <summary>
	/// Initializes a binary composite metadata instance.
	/// </summary>
	/// <param name="capacity">Buffer's capacity.</param>
	public BinaryCompositeBufferTypeMetadata(Int32 capacity) : base(capacity) { }

	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal override void AppendComponent(IMetadataStorage storage)
	{
		ManagedBuffer<T>.AppendComponent(BufferTypeMetadata<T>.GetMetadata<TBufferA>(), storage);
		ManagedBuffer<T>.AppendComponent(BufferTypeMetadata<T>.GetMetadata<TBufferB>(), storage);
	}
}

/// <summary>
/// Metadata of a binary <see cref="Composite{TBufferA,TBufferB,T}"/>.
/// </summary>
/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal sealed class BinaryCompositeBufferTypeMetadata<
	[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer, T> : BufferTypeMetadata<TBuffer, T>
	where TBuffer : struct, IManagedBuffer<T>
{
	/// <summary>
	/// Low component.
	/// </summary>
	internal BufferTypeMetadata<T> Low { get; init; }
	/// <summary>
	/// High component.
	/// </summary>
	internal BufferTypeMetadata<T> High { get; init; }

	/// <inheritdoc/>
	public override Int32 ComponentCount => 2;
	/// <inheritdoc/>
	public override BufferTypeMetadata this[Int32 index] => index switch
	{
		0 => this.Low,
		1 => this.High,
		_ => throw new IndexOutOfRangeException(),
	};

	/// <summary>
	/// Initializes a binary composite metadata instance.
	/// </summary>
	/// <param name="capacity">Buffer's capacity.</param>
	/// <param name="low">Low component.</param>
	/// <param name="high">High component.</param>
	/// <param name="isBinary">Indicates whether the composition is binary.</param>
	public BinaryCompositeBufferTypeMetadata(Int32 capacity, BufferTypeMetadata<T> low, BufferTypeMetadata<T> high,
		Boolean isBinary) : base(capacity, isBinary)
	{
		this.Low = low;
		this.High = high;
	}

	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal override void AppendComponent(IMetadataStorage storage)
	{
		if (!this.IsBinary) return;
		ManagedBuffer<T>.AppendComponent(this.Low, storage);
		ManagedBuffer<T>.AppendComponent(this.High, storage);
	}
}
