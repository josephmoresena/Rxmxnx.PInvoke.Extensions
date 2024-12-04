namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// Non-binary buffer space.
/// </summary>
/// <typeparam name="TArray">The type inline array.</typeparam>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
/// <remarks><typeparamref name="TArray"/> must ensure that the <typeparamref name="T"/> elements are safely preserved.</remarks>
[StructLayout(LayoutKind.Sequential)]
#pragma warning disable CA2252
public unsafe struct NonBinarySpace<TArray, T> : IManagedBuffer<T> where TArray : struct
{
	/// <summary>
	/// Buffer type metadata.
	/// </summary>
#pragma warning disable CS8500
	private static readonly BufferTypeMetadata<NonBinarySpace<TArray, T>, T> typeMetadata =
		new(sizeof(TArray) / sizeof(T), false);
#pragma warning disable CS8500

	/// <summary>
	/// Internal value.
	/// </summary>
	private TArray _value;

	static BufferTypeMetadata<T>[] IManagedBuffer<T>.Components => [];
	static BufferTypeMetadata<T> IManagedBuffer<T>.TypeMetadata => NonBinarySpace<TArray, T>.typeMetadata;

	static void IManagedBuffer<T>.AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components) { }
}
#pragma warning disable CA2252