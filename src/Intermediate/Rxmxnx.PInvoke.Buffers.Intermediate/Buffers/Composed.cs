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
	/// Low buffer.
	/// </summary>
	private TBufferA _buff0;
	/// <summary>
	/// High buffer.
	/// </summary>
	private TBufferB _buff1;

	/// <inheritdoc/>
	public static Int32 Capacity => TBufferA.Capacity + TBufferB.Capacity;

	static void IAllocatedBuffer<T>.AppendComponent(IDictionary<UInt16, IBufferTypeMetadata<T>> component)
	{
		IAllocatedBuffer<T>.AppendComponent<TBufferA>(component);
		IAllocatedBuffer<T>.AppendComponent<TBufferB>(component);
	}
}
#pragma warning restore CA2252