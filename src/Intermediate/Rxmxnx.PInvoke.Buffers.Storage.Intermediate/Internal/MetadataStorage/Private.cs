namespace Rxmxnx.PInvoke.Internal;

internal abstract partial class MetadataStorage<T>
{
	/// <summary>
	/// Current instance.
	/// </summary>
	private static MetadataStorage<T>? instance;

	/// <summary>
	/// Retrieves the current instance binary buffer map.
	/// </summary>
	/// <returns>A span of <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	private static BinaryMap<T> GetBinaryMap(UInt16 count)
	{
		UInt16 capacity = BuffersHelper.GetCapacityFor(count);
		// Slots are prepared only if reflection is enabled.
		return MetadataStorage.Instance.GetBinaryMap(capacity, ref MetadataStorage<T>.instance,
		                                             BuffersHelper.BufferAutoCompositionEnabled);
	}
	/// <summary>
	/// Retrieves binary metadata required for a buffer with <paramref name="count"/> items.
	/// </summary>
	/// <param name="binaryMap">Map of binary buffers type metadata.</param>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <param name="allowMinimal">Allow to return minimal buffer.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1199)]
#endif
	private static BufferTypeMetadata<T>? GetBinaryMetadata(BinaryMap<T> binaryMap, UInt16 count, Boolean allowMinimal)
	{
		if (binaryMap[count] is { } result)
			// Exact type metadata is found.
			return result;
		if (!binaryMap.IsAllowed(count))
			// There is no minimal.
			return default;

		result = MetadataStorage<T>.GetFundamental(binaryMap, BuffersHelper.GetSpaceFor(count));
		// Fundamental metadata not found. Use minimal.
		if (result is null)
			return allowMinimal ? binaryMap.GetMinimal(count) : result;
		while (count - result.Size > 0)
		{
			UInt16 diff = (UInt16)(count - result.Size);
			BufferTypeMetadata<T>? aux = MetadataStorage<T>.GetBinaryMetadata(binaryMap, diff, false);
			{
				// Auxiliary metadata not found. Use minimal.
				if (aux is null)
					return allowMinimal ? binaryMap.GetMinimal(count) : default;
				result = result.Compose(MetadataStorage.Instance, aux);
				if (result is null)
					// Unable to create composed metadata. Use minimal.
					return allowMinimal ? binaryMap.GetMinimal(count) : default;
				binaryMap[result.Size] = result;
			}
		}
		return result;
	}
	/// <summary>
	/// Retrieves the fundamental component of size <paramref name="space"/>.
	/// </summary>
	/// <param name="binaryMap">Map of binary buffers type metadata.</param>
	/// <param name="space">Size of fundamental component.</param>
	/// <returns>A <see cref="BufferTypeMetadata"/> instance.</returns>
	private static BufferTypeMetadata<T>? GetFundamental(BinaryMap<T> binaryMap, UInt16 space)
	{
		if (binaryMap[space] is { } metadata)
			return metadata;
		if (space == 1)
			return binaryMap[space] = Atomic<T>.TypeMetadata; // Atomic missing.
		BufferTypeMetadata<T>? result = MetadataStorage<T>.GetMaxBinarySpace(binaryMap, (UInt16)(space / 2));
		while (result.Size < space)
		{
			result = result.Double(MetadataStorage.Instance);
			if (result is null) break;
			binaryMap[result.Size] = result;
		}
		return result;
	}
	/// <summary>
	/// Retrieves the nearest fundamental component to <paramref name="space"/> size.
	/// </summary>
	/// <param name="binaryMap">Map of binary buffers type metadata.</param>
	/// <param name="space">Size of fundamental component.</param>
	/// <returns>A <see cref="BufferTypeMetadata"/> instance.</returns>
	private static BufferTypeMetadata<T> GetMaxBinarySpace(BinaryMap<T> binaryMap, UInt16 space)
	{
		ref BufferTypeMetadata<T>? result = ref binaryMap[space];
		while (result is null)
		{
			if (space == 1)
			{
				result = Atomic<T>.TypeMetadata;
				break;
			}
			space /= 2;
		}
		return result;
	}
	/// <summary>
	/// Indicates whether the current instance is prepared for <paramref name="count"/>
	/// </summary>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <returns>
	/// <see langword="true"/> if the current instance is prepared for <paramref name="count"/>; otherwise
	/// <see langword="false"/>.
	/// </returns>
	private static Boolean IsBinaryPrepared(UInt16 count) => MetadataStorage<T>.instance?.Capacity >= count;
}