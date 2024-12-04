namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// Atomic binary buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
/// <remarks>Use this type as the basic unit of binary buffers.</remarks>
#pragma warning disable CA2252
[StructLayout(LayoutKind.Sequential)]
public partial struct Atomic<T> : IManagedBinaryBuffer<Atomic<T>, T>
{
	/// <summary>
	/// Internal metadata.
	/// </summary>
	private static readonly BufferTypeMetadata<T> typeMetadata = new BufferTypeMetadata<Atomic<T>, T>(1);

	/// <summary>
	/// Internal value.
	/// </summary>
	private T _val0;

	static BufferTypeMetadata<T> IManagedBuffer<T>.TypeMetadata => Atomic<T>.typeMetadata;
	static BufferTypeMetadata<T>[] IManagedBuffer<T>.Components => [];

	static void IManagedBuffer<T>.AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components) { }
}
#pragma warning restore CA2252