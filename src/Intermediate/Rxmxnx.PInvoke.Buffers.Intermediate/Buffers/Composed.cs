namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// Primordial buffer.
/// </summary>
/// <typeparam name="TBufferA">The type of low buffer.</typeparam>
/// <typeparam name="TBufferB">The type of high buffer.</typeparam>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
#pragma warning disable CA2252
[StructLayout(LayoutKind.Sequential)]
public struct Composed<TBufferA, TBufferB, T> : IAllocatedBuffer<T> where TBufferA : struct, IAllocatedBuffer<T>
	where TBufferB : struct, IAllocatedBuffer<T>
{
	/// <summary>
	/// Internal metadata.
	/// </summary>
	private static readonly IBufferTypeMetadata<T> metadata =
		new BufferTypeMetadata<Composed<TBufferA, TBufferB, T>, T>(TBufferA.Metadata.Size + TBufferB.Metadata.Size);

	/// <summary>
	/// Low buffer.
	/// </summary>
	private TBufferA _buff0;
	/// <summary>
	/// High buffer.
	/// </summary>
	private TBufferB _buff1;

	static Boolean IAllocatedBuffer<T>.IsBinary => true;
	static IBufferTypeMetadata<T> IAllocatedBuffer<T>.Metadata => Composed<TBufferA, TBufferB, T>.metadata;
	static void IAllocatedBuffer<T>.AppendComponent(IDictionary<UInt16, IBufferTypeMetadata<T>> components)
	{
		IAllocatedBuffer<T>.AppendComponent<TBufferA>(components);
		IAllocatedBuffer<T>.AppendComponent<TBufferB>(components);
	}
}
#pragma warning restore CA2252