#if !PACKAGE
namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Exposes binary type metadata slots.
/// </summary>
internal interface IBinarySlotsOwner<T>
{
	/// <summary>
	/// Additional slots.
	/// </summary>
	BufferTypeMetadata<T>?[]?[] Slots { get; }
}
#endif