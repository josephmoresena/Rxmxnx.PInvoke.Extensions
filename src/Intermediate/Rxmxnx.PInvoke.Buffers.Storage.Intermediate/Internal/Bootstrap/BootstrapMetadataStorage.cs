#if !NET8_0_OR_GREATER
// ReSharper disable EmptyNamespace
#endif
namespace Rxmxnx.PInvoke.Internal.Bootstrap;

#if NET8_0_OR_GREATER
/// <summary>
/// Bootstrap storage class.
/// </summary>
/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal abstract class BootstrapMetadataStorage<
	[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer, T> : MetadataStorage<T>
	where TBuffer : struct, IManagedBinaryBuffer<TBuffer, Object>
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
	protected BootstrapMetadataStorage() : base(out ReadOnlySpan<BufferTypeMetadata<T>?> source)
	{
		if (source.IsEmpty) return;
		Span<BufferTypeMetadata<T>?> binaryMap = MemoryMarshal.CreateSpan(ref this.MetadataReference, this.Capacity);
		source.CopyTo(binaryMap);
	}
}
#endif