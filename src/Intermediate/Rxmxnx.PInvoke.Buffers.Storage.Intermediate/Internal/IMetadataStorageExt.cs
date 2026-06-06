namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Exposes a buffer metadata store.
/// </summary>
internal interface IMetadataStorageExt : IMetadataStorage
{
	void IMetadataStorage.RegisterBuffer<T,
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer>()
		=> MetadataStorage<T>.RegisterBuffer<TBuffer>();
	void IMetadataStorage.PrepareBinaryMetadata<T>(UInt16 count) => MetadataStorage<T>.PrepareBinaryMetadata(count);
	BufferTypeMetadata<T>? IMetadataStorage.GetMetadata<T>(UInt16 count) => MetadataStorage<T>.GetMetadata(count);
	Boolean IMetadataStorage.TryAdd<T>(BufferTypeMetadata<T> component) => MetadataStorage<T>.TryAdd(component);
	BufferTypeMetadata<T>? IMetadataStorage.AddBinaryMetadata<T>(BufferTypeMetadata<T>? typeMetadata)
	{
		if (typeMetadata is null) return default;
		MetadataStorage<T>.AddBinaryMetadata(typeMetadata);
		return typeMetadata;
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6670)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3973)]
	void IMetadataStorage.PrintMetadata<T>(Boolean trace)
	{
		if (!trace) return;
		Int32 count = 0;
		foreach (BufferTypeMetadata<T>? m in MetadataStorage<T>.GetBinaryMap())
		{
			if (m is null) continue;
			// ReSharper disable once HeapView.BoxingAllocation
			Trace.WriteLine($"{typeof(T)} {m.Size}({String.Join(", ", m.Components.ToArray().Select(k => k.Size))})");
			count++;
		}
		foreach (BufferTypeMetadata<T>?[] a in MetadataStorage<T>.GetBinarySlots())
		foreach (BufferTypeMetadata<T>? m in a)
		{
			if (m is null) continue;
			// ReSharper disable once HeapView.BoxingAllocation
			Trace.WriteLine($"{typeof(T)} {m.Size}({String.Join(", ", m.Components.ToArray().Select(k => k.Size))})");
			count++;
		}
		// ReSharper disable once HeapView.BoxingAllocation
		Trace.WriteLine($"{typeof(T)}: {count}");
	}
#endif

	/// <summary>
	/// Retrieves a <see cref="BinaryMap{T}"/> instance.
	/// </summary>
	/// <param name="capacity">Output. Current capacity.</param>
	/// <param name="instance">Reference. Current <see cref="MetadataStorage{T}"/> instance.</param>
	/// <param name="prepareSlots">Indicates whether the slots should be prepared.</param>
	/// <returns>A <see cref="BinaryMap{T}"/> instance.</returns>
	BinaryMap<T> GetBinaryMap<T>(UInt16 capacity, ref MetadataStorage<T>? instance, Boolean prepareSlots);
}