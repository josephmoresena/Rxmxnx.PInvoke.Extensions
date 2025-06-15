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
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#pragma warning disable CA2252
public unsafe struct NonBinarySpace<TArray, T> : IManagedBuffer<T> where TArray : struct
{
	/// <summary>
	/// Buffer type metadata.
	/// </summary>
	internal static readonly BufferTypeMetadata<T> TypeMetadata = NonBinarySpace<TArray, T>.GetMetadata();

	/// <summary>
	/// Internal value.
	/// </summary>
	private TArray _value;

#if NET6_0_OR_GREATER
	static BufferTypeMetadata<T>[] IManagedBuffer<T>.Components => [];
	static BufferTypeMetadata<T> IManagedBuffer<T>.TypeMetadata => NonBinarySpace<TArray, T>.TypeMetadata;

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	static void IManagedBuffer<T>.AppendComponent(IDictionary<UInt16, BufferTypeMetadata<T>> components) { }
#else
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	void IManagedBuffer<T>.DoNotImplement() { }
#endif

	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance for current type.
	/// </summary>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance for current type.</returns>
	private static BufferTypeMetadata<NonBinarySpace<TArray, T>, T> GetMetadata()
	{
		Boolean isItemUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>();
		Boolean isArrayUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<TArray>();
		ValidationUtilities.ThrowIfInvalidBuffer(typeof(T), isItemUnmanaged, typeof(TArray), isArrayUnmanaged);
		
#pragma warning disable CS8500
		Int32 spaceCapacity = sizeof(TArray) / sizeof(T);
#pragma warning restore CS8500
		
#if NET6_0_OR_GREATER
		return new(spaceCapacity, false);
#else
		return new(spaceCapacity, [], false);
#endif
	}
}
#pragma warning disable CA2252