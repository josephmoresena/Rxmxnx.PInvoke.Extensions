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
public partial struct Composite<
#if NET5_0_OR_GREATER
	[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)]
#endif
	TBufferA,
#if NET5_0_OR_GREATER
	[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)]
#endif
	TBufferB, T> : IManagedBinaryBuffer<Composite<TBufferA, TBufferB, T>, T>
	where TBufferA : struct, IManagedBinaryBuffer<TBufferA, T>
	where TBufferB : struct, IManagedBinaryBuffer<TBufferB, T>
{
	/// <summary>
	/// Internal metadata.
	/// </summary>
	internal static readonly BufferTypeMetadata<T> TypeMetadata =
#if !PACKAGE && NET6_0 || NET7_0_OR_GREATER
		new BufferTypeMetadata<Composite<TBufferA, TBufferB, T>, T>(
			BufferManager.MetadataManager<T>.GetCapacity(TBufferA.TypeMetadata, TBufferB.TypeMetadata,
			                                             out Boolean isBinary), isBinary);
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

#if !PACKAGE && NET6_0 || NET7_0_OR_GREATER
	static BufferTypeMetadata<T> IManagedBuffer<T>.TypeMetadata => Composite<TBufferA, TBufferB, T>.TypeMetadata;
	static BufferTypeMetadata<T>[] IManagedBuffer<T>.Components => [TBufferA.TypeMetadata, TBufferB.TypeMetadata,];

	static void IManagedBuffer<T>.AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components)
	{
		IManagedBuffer<T>.AppendComponent<TBufferA>(components);
		IManagedBuffer<T>.AppendComponent<TBufferB>(components);
	}
#else
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	void IManagedBuffer<T>.DoNotImplement() { }

	/// <summary>
	/// Creates the <see cref="BufferTypeMetadata{T}"/> instance for current type.
	/// </summary>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	private static BufferTypeMetadata<T> CreateBufferMetadata()
	{
		BufferTypeMetadata<T>[] components =
			BufferManager.MetadataManager<T>.GetComponents(typeof(TBufferA), typeof(TBufferB));
		Int32 capacity =
			BufferManager.MetadataManager<T>.GetCapacity(components[0], components[1], out Boolean isBinary);
		return new BufferTypeMetadata<Composite<TBufferA, TBufferB, T>, T>(
			capacity, components, isBinary, Composite<TBufferA, TBufferB, T>.AppendComponent);
	}
	/// <summary>
	/// Appends all components from current type.
	/// </summary>
	/// <param name="components">A dictionary of components.</param>
	private static void AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components)
	{
		BufferTypeMetadata<T> currentMetadata = Composite<TBufferA, TBufferB, T>.TypeMetadata;
		if (!currentMetadata.IsBinary) return;
		foreach (BufferTypeMetadata<T> component in currentMetadata.Components.AsSpan())
			IManagedBuffer<T>.AppendComponent(component, components);
	}
#endif
}
#pragma warning restore CA2252