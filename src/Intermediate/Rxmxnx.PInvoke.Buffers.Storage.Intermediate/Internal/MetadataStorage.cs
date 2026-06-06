namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Generic metadata buffer type storage class.
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
	/// Parameterless constructor.
	/// </summary>
#if !PACKAGE && NET8_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	protected MetadataStorage() => Interlocked.CompareExchange(ref MetadataStorage<T>.instance, this, null);
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="source">Output. Current buffers type metadata instance.</param>
	protected MetadataStorage(out ReadOnlySpan<BufferTypeMetadata<T>?> source)
		=> source = Interlocked.CompareExchange(ref MetadataStorage<T>.instance, this, null) is { } original ?
			MemoryMarshal.CreateReadOnlySpan(ref original.MetadataReference, original.Capacity) :
			default;
}