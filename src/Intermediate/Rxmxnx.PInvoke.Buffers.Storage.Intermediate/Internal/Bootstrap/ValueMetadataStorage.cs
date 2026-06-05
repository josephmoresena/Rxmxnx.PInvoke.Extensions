namespace Rxmxnx.PInvoke.Internal.Bootstrap;

/// <summary>
/// Internal value store with 2^5-1 binary space.
/// </summary>
internal sealed class ValueMetadataStorage31<T> : G31<T>;

/// <summary>
/// Internal value store with 2^7-1 binary space.
/// </summary>
internal sealed class ValueMetadataStorage127<T> : G127<T>;

/// <summary>
/// Internal value store with 2^11-1 binary space.
/// </summary>
internal sealed class ValueMetadataStorage2047<T>(Boolean withSlots) : G2047<T>
{
	/// <inheritdoc/>
	public override BufferTypeMetadata<T>?[]?[] Slots { get; } = withSlots ? new BufferTypeMetadata<T>?[]?[5] : [];
}