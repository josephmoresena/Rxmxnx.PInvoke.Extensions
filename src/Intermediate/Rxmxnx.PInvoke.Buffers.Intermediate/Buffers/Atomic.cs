namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// Atomic binary buffer.
/// </summary>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
/// <remarks>Use this type as the basic unit of binary buffers.</remarks>
[StructLayout(LayoutKind.Sequential)]
public struct Atomic<T> : IManagedBinaryBuffer<Atomic<T>, T>
{
	/// <summary>
	/// Internal metadata.
	/// </summary>
	internal static readonly BufferTypeMetadata<T> TypeMetadata = new BufferTypeMetadata<Atomic<T>, T>(1);

	/// <summary>
	/// Internal value.
	/// </summary>
	private T _val0;

#if NET7_0_OR_GREATER
	static Boolean IManagedBinaryBuffer<Atomic<T>, T>.IsStaticBinary => true;
	static BufferTypeMetadata<T> IManagedBuffer<T>.TypeMetadata => Atomic<T>.TypeMetadata;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBuffer<T>.AppendComponent(IMetadataStorage storage) { }
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	BufferTypeMetadata<T> IManagedBinaryBuffer<T>.Metadata => Atomic<T>.TypeMetadata;
#if !PACKAGE && NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	BufferTypeMetadata<T> IManagedBuffer<T>.GetStaticTypeMetadata() => Atomic<T>.TypeMetadata;
}