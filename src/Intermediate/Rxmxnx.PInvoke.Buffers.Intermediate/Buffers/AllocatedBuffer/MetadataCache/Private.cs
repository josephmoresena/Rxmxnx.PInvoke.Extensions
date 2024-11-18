namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public static partial class AllocatedBuffer
{
	internal static partial class MetadataCache<T>
	{
		/// <summary>
		/// Internal cache.
		/// </summary>
		private static readonly Cache cache = new();

		/// <summary>
		/// Retrieves the fundamental metadata for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <returns>A <see cref="ManagedBufferMetadata{T}"/> instance.</returns>
		private static ManagedBufferMetadata<T>? GetFundamental(UInt16 count)
		{
			lock (MetadataCache<T>.cache.LockObject)
			{
				if (MetadataCache<T>.cache.Buffers.TryGetValue(count, out ManagedBufferMetadata<T>? metadata))
					return metadata;
				UInt16 space = MetadataCache<T>.cache.MaxSpace;
				while (count < space) space /= 2;
				ManagedBufferMetadata<T>? result = MetadataCache<T>.cache.Buffers[space];
				while (AllocatedBuffer.GetMaxValue(result.Size) < count)
				{
					result = result.Double();
					if (result is null) break;
					MetadataCache<T>.cache.Add(result);
					MetadataCache<T>.cache.MaxSpace = result.Size;
				}
				return result;
			}
		}
	}
}
#pragma warning restore CA2252