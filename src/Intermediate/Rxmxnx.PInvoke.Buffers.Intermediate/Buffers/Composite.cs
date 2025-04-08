namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// Composite binary buffer.
/// </summary>
/// <typeparam name="TBufferA">The type of low buffer.</typeparam>
/// <typeparam name="TBufferB">The type of high buffer.</typeparam>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
/// <remarks>
/// Use this type to perform the binary combinations required to create any binary buffer type. <br/>
/// <typeparamref name="TBufferA"/> must have a smaller capacity than <typeparamref name="TBufferB"/>.<br/>
/// <typeparamref name="TBufferB"/> must have a capacity that is a power of two.<br/>
/// If the current type has a capacity that is a power of two, <typeparamref name="TBufferA"/> and
/// <typeparamref name="TBufferB"/> must be the same.
/// </remarks>
#pragma warning disable CA2252
[StructLayout(LayoutKind.Sequential)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
#endif
public partial struct Composite<TBufferA, TBufferB, T> : IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>
	where TBufferA : struct, IManagedBinaryBuffer<TBufferA, T>
	where TBufferB : struct, IManagedBinaryBuffer<TBufferB, T>
{
	/// <summary>
	/// Internal metadata.
	/// </summary>
	private static readonly BufferTypeMetadata<T> typeMetadata =
		new BufferTypeMetadata<Composite<TBufferA, TBufferB, T>, T>(
			TBufferA.TypeMetadata.Size + TBufferB.TypeMetadata.Size, Composite<TBufferA, TBufferB, T>.IsBinary());

	/// <summary>
	/// Low buffer.
	/// </summary>
	private TBufferA _buff0;
	/// <summary>
	/// High buffer.
	/// </summary>
	private TBufferB _buff1;

	static BufferTypeMetadata<T> IManagedBuffer<T>.TypeMetadata => Composite<TBufferA, TBufferB, T>.typeMetadata;
	static BufferTypeMetadata<T>[] IManagedBuffer<T>.Components => [TBufferA.TypeMetadata, TBufferB.TypeMetadata,];

	static void IManagedBuffer<T>.AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components)
	{
		IManagedBuffer<T>.AppendComponent<TBufferA>(components);
		IManagedBuffer<T>.AppendComponent<TBufferB>(components);
	}

	/// <summary>
	/// Determines if current buffer type is binary.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if current buffer type is binary; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Boolean IsBinary()
	{
		if (!TBufferA.TypeMetadata.IsBinary || !TBufferB.TypeMetadata.IsBinary)
			return false;
		if (TBufferB.TypeMetadata.Components.Length == 2 &&
		    TBufferB.TypeMetadata.Components[0] != TBufferB.TypeMetadata.Components[^1])
			return false;
		UInt16 sizeB = TBufferB.TypeMetadata.Size;
		Int32 diff = sizeB - TBufferA.TypeMetadata.Size;
		return diff >= 0 && diff <= sizeB;
	}
}
#pragma warning restore CA2252