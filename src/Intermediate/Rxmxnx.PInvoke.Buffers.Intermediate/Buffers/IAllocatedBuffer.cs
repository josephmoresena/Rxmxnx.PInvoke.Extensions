namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// This interfaces exposes an allocated buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#if NET6_0
[RequiresPreviewFeatures]
#endif
public interface IAllocatedBuffer<T>
{
	/// <summary>
	/// Buffer capacity.
	/// </summary>
	internal static abstract Int32 Capacity { get; }
}