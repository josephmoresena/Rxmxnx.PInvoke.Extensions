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
[StructLayout(LayoutKind.Sequential)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2436)]
#endif
public struct Composite<[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBufferA,
	[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBufferB,
	T> : IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>
	where TBufferA : struct, IManagedBinaryBuffer<TBufferA, T>
	where TBufferB : struct, IManagedBinaryBuffer<TBufferB, T>
{
	/// <summary>
	/// Internal metadata.
	/// </summary>
	internal static readonly BufferTypeMetadata<T> TypeMetadata =
#if NET7_0_OR_GREATER
		new BufferTypeMetadata<Composite<TBufferA, TBufferB, T>, T>(
			BuffersHelper.GetCapacity(TBufferA.TypeMetadata, TBufferB.TypeMetadata, out Boolean isBinary), isBinary);
#else
		Composite<TBufferA, TBufferB, T>.CreateBufferMetadata();
#endif

	/// <summary>
	/// Low buffer.
	/// </summary>
	private TBufferA _buff0;
	/// <summary>
	/// High buffer.
	/// </summary>
	private TBufferB _buff1;

#if NET7_0_OR_GREATER
	static BufferTypeMetadata<T> IManagedBuffer<T>.TypeMetadata => Composite<TBufferA, TBufferB, T>.TypeMetadata;
	static BufferTypeMetadata<T>[] IManagedBuffer<T>.Components => [TBufferA.TypeMetadata, TBufferB.TypeMetadata,];

	static void IManagedBuffer<T>.AppendComponent(IMetadataStorage storage)
	{
		IManagedBuffer<T>.AppendComponent<TBufferA>(storage);
		IManagedBuffer<T>.AppendComponent<TBufferB>(storage);
	}
#else
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	BufferTypeMetadata<T> IManagedBinaryBuffer<T>.Metadata => Composite<TBufferA, TBufferB, T>.TypeMetadata;
	BufferTypeMetadata<T> IManagedBuffer<T>.GetStaticTypeMetadata() => Composite<TBufferA, TBufferB, T>.TypeMetadata;

	/// <summary>
	/// Creates the <see cref="BufferTypeMetadata{T}"/> instance for current type.
	/// </summary>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	private static BufferTypeMetadata<T> CreateBufferMetadata()
	{
		BufferTypeMetadata<T>[] components = BuffersHelper.GetComponents<T, TBufferA, TBufferB>();
		Int32 capacity = BuffersHelper.GetCapacity(components[0], components[1], out Boolean isBinary);
		return new BufferTypeMetadata<Composite<TBufferA, TBufferB, T>, T>(
			capacity, components, isBinary, Composite<TBufferA, TBufferB, T>.AppendComponent);
	}
	/// <summary>
	/// Appends all components from current type.
	/// </summary>
	/// <param name="storage">A <see cref="IMetadataStorage"/> instance.</param>
	private static void AppendComponent(IMetadataStorage storage)
	{
		BufferTypeMetadata<T> currentMetadata = Composite<TBufferA, TBufferB, T>.TypeMetadata;
		if (!currentMetadata.IsBinary) return;
		foreach (BufferTypeMetadata<T> component in currentMetadata.Components.Span)
			ManagedBuffer<T>.AppendComponent(component, storage);
	}
#endif
}