namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public static partial class AllocatedBuffer
{
	internal static partial class MetadataCache<T>
	{
		private static readonly Cache cache = new();

		/// <summary>
		/// Static constructor.
		/// </summary>
		[ExcludeFromCodeCoverage]
		static MetadataCache()
		{
			MetadataCache<T>.cache.Add(IAllocatedBuffer<T>.GetMetadata<Primordial<T>>());
			MetadataCache<T>.cache.Add(IAllocatedBuffer<T>.GetMetadata<Composed<Primordial<T>, Primordial<T>, T>>());
			MetadataCache<T>.cache.Add(IAllocatedBuffer<T>
				                           .GetMetadata<
					                           Composed<Primordial<T>, Composed<Primordial<T>, Primordial<T>, T>,
						                           T>>());
		}

		/// <summary>
		/// Retrieves the fundamental metadata for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <returns>A <see cref="IBufferTypeMetadata{T}"/> instance.</returns>
		private static IBufferTypeMetadata<T>? GetFundamental(UInt16 count)
		{
			lock (MetadataCache<T>.cache.LockObject)
			{
				if (MetadataCache<T>.cache.Metadatas.TryGetValue(count, out IBufferTypeMetadata<T>? metadata))
					return metadata;
				UInt16 space = MetadataCache<T>.cache.MaxSpace;
				while (count < space) space /= 2;
				IBufferTypeMetadata<T>? result = MetadataCache<T>.cache.Metadatas[space];
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