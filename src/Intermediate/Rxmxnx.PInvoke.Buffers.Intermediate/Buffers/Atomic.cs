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
	internal static readonly BufferTypeMetadata<T> TypeMetadata =
#if NET6_0_OR_GREATER
		new BufferTypeMetadata<Atomic<T>, T>(1);
#else
		new BufferTypeMetadata<Atomic<T>, T>(1, []);
#endif

	/// <summary>
	/// Internal value.
	/// </summary>
	private T _val0;

#if NET6_0_OR_GREATER
	static BufferTypeMetadata<T> IManagedBuffer<T>.TypeMetadata => Atomic<T>.TypeMetadata;
	static BufferTypeMetadata<T>[] IManagedBuffer<T>.Components => [];
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBuffer<T>.AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components) { }
#endif
}
#pragma warning restore CA2252