namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Exposes the maximum capacity of a metadata store.
/// </summary>
internal interface IStorageCapacity
{
	/// <summary>
	/// Maximum storage capacity.
	/// </summary>
	Int32 MaxStorageCapacity { get; }
}