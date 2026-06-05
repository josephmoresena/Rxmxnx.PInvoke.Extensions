namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Bootstrap storage class.
/// </summary>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal abstract partial class MetadataStorage<T>
{
	/// <summary>
	/// Current capacity.
	/// </summary>
	public abstract UInt16 Capacity { get; }
	/// <summary>
	/// </summary>
	public abstract ref BufferTypeMetadata<T>? MetadataReference { get; }

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="source">Output. Current buffers type metadata instance.</param>
	protected MetadataStorage(out ReadOnlySpan<BufferTypeMetadata<T>?> source)
		=> source = Interlocked.CompareExchange(ref MetadataStorage<T>.instance, this, null) is { } original ?
			MemoryMarshal.CreateReadOnlySpan(ref original.MetadataReference, original.Capacity) :
			default;
}

/// <summary>
/// Bootstrap storage class.
/// </summary>
/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal abstract class MetadataStorage<[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer,
	T> : MetadataStorage<T> where TBuffer : struct, IManagedBinaryBuffer<TBuffer, Object>
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private TBuffer _value;

	/// <inheritdoc/>
	public sealed override UInt16 Capacity => this._value.Metadata.Size;
	/// <inheritdoc/>
	public sealed override ref BufferTypeMetadata<T>? MetadataReference
		=> ref Unsafe.As<TBuffer, BufferTypeMetadata<T>?>(ref this._value);

	/// <summary>
	/// Internal object type metadata.
	/// </summary>
	protected BufferTypeMetadata<Object> TypeMetadata => this._value.Metadata;

	/// <summary>
	/// Parameterless constructor.
	/// </summary>
	protected MetadataStorage() : base(out ReadOnlySpan<BufferTypeMetadata<T>?> source)
	{
		if (source.IsEmpty) return;
		Span<BufferTypeMetadata<T>?> binaryMap = MemoryMarshal.CreateSpan(ref this.MetadataReference, this.Capacity);
		source.CopyTo(binaryMap);
	}
}