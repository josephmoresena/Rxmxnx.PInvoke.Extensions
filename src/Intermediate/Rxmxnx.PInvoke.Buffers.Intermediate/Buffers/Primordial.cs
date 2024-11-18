namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// Primordial buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#pragma warning disable CA2252
[StructLayout(LayoutKind.Sequential)]
public struct Primordial<T> : IManagedBuffer<T>
{
	/// <summary>
	/// Internal metadata.
	/// </summary>
	private static readonly ManagedBufferMetadata<T> metadata = new BufferTypeMetadata<Primordial<T>, T>(1);

	/// <summary>
	/// Internal value.
	/// </summary>
	private T _val0;

	static Boolean IManagedBuffer<T>.IsPure => true;
	static Boolean IManagedBuffer<T>.IsBinary => true;
	static ManagedBufferMetadata<T> IManagedBuffer<T>.Metadata => Primordial<T>.metadata;
	static ManagedBufferMetadata<T>[] IManagedBuffer<T>.Components => [];

	static void IManagedBuffer<T>.AppendComponent(IDictionary<UInt16, ManagedBufferMetadata<T>> components) { }
	static void IManagedBuffer<T>.Append<TBuffer>(IDictionary<UInt16, ManagedBufferMetadata<T>> components)
	{
		UInt16 initialSize = TBuffer.Metadata.Size;
		components.TryAdd(initialSize, TBuffer.Metadata);
	}
}
#pragma warning restore CA2252