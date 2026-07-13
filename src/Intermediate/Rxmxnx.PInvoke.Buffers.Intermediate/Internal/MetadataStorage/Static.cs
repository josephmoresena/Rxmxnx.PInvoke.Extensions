#if NET8_0_OR_GREATER
using Rxmxnx.PInvoke.Buffers.Storage.Bootstrap;
#endif

namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Metadata storage class.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal abstract partial class MetadataStorage
{
	/// <summary>
	/// Singleton instance.
	/// </summary>
#if !NET8_0_OR_GREATER
	public static readonly IMetadataStorage Instance = new MetadataStorage<StandardBackend>();
#else
	public static readonly IMetadataStorage Instance = MetadataStorage.MaxCapacity switch
	{
		0 => new MetadataStorage<StandardBackend>(),
		31 => new MetadataStorage<BootstrapBackend31>(),
		127 => new MetadataStorage<BootstrapBackend127>(),
		2047 => new MetadataStorage<BootstrapBackend<Space11>>(),
		_ => new MetadataStorage<BootstrapBackend<Space16>>(),
	};
#endif
	/// <summary>
	/// Maximum capacity of the buffers.
	/// </summary>
	public static Int32 MaxCapacity => 0; // Standard storage.

#if NET8_0_OR_GREATER
	/// <summary>
	/// Initialize bootstrap object storage.
	/// </summary>
	/// <param name="span">A <see cref="BufferTypeMetadata{Object}"/> span.</param>
	/// <param name="bufferTypeMetadata">Initial <see cref="BufferTypeMetadata{Object}"/> instance.</param>
	internal static void Initialize(Span<BufferTypeMetadata<Object>?> span,
		BufferTypeMetadata<Object> bufferTypeMetadata)
	{
		ref BufferTypeMetadata<Object>? refComponent = ref span[bufferTypeMetadata.Size - 1];
		if (refComponent is not null) return;

		refComponent = bufferTypeMetadata;
		foreach (BufferTypeMetadata<Object> metadataComponent in bufferTypeMetadata.Components.Span)
			MetadataStorage.Initialize(span, metadataComponent);
	}
#endif
}