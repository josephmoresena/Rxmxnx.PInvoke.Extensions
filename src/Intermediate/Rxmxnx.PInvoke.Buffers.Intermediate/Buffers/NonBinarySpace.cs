namespace Rxmxnx.PInvoke.Buffers;

/// <summary>
/// Non-binary buffer space.
/// </summary>
/// <typeparam name="TArray">The type inline array.</typeparam>
/// <typeparam name="T">The type of items in the buffer.</typeparam>
/// <remarks>
/// <typeparamref name="TArray"/> must ensure that the <typeparamref name="T"/> elements are safely preserved. <br/>
/// It is not guaranteed that there is no other type of binary or non-binary buffer capable of storing this amount;
/// therefore, its declaration might generate metadata that will never be used.
/// </remarks>
[StructLayout(LayoutKind.Sequential)]
#pragma warning disable CA2252
public unsafe struct NonBinarySpace<TArray, T> : IManagedBuffer<T> where TArray : struct
{
	/// <summary>
	/// Buffer type metadata.
	/// </summary>
	private static readonly BufferTypeMetadata<T> typeMetadata = NonBinarySpace<TArray, T>.GetMetadata();

	/// <summary>
	/// Internal value.
	/// </summary>
	private TArray _value;

	static BufferTypeMetadata<T>[] IManagedBuffer<T>.Components => [];
	static BufferTypeMetadata<T> IManagedBuffer<T>.TypeMetadata => NonBinarySpace<TArray, T>.typeMetadata;

	static void IManagedBuffer<T>.AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components) { }

	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance for current type.
	/// </summary>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance for current type.</returns>
	private static BufferTypeMetadata<NonBinarySpace<TArray, T>, T> GetMetadata()
	{
		Boolean isItemUnmanaged = ReadOnlyValPtr<T>.IsUnmanaged;
		Boolean isArrayUnmanaged = ReadOnlyValPtr<TArray>.IsUnmanaged;
		ValidationUtilities.ThrowIfNotUnmanagedBuffer(typeof(T), isItemUnmanaged, typeof(TArray), isArrayUnmanaged);
#pragma warning disable CS8500
		return new(sizeof(TArray) / sizeof(T), false);
#pragma warning restore CS8500
	}
}
#pragma warning disable CA2252