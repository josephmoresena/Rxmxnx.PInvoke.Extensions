namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public static partial class AllocatedBuffer
{
	private static class MetadataCache<T>
	{
		/// <summary>
		/// Lock object.
		/// </summary>
		private static readonly Object lockObject = new();
		/// <summary>
		/// Dictionary.
		/// </summary>
		private static readonly Dictionary<UInt16, IBufferTypeMetadata<T>> cache = new();
		/// <summary>
		/// Maximum double space.
		/// </summary>
		private static UInt16 maxSpace = 2;

		/// <summary>
		/// Static constructor.
		/// </summary>
		static MetadataCache()
		{
			lock (MetadataCache<T>.lockObject)
			{
				MetadataCache<T>.cache.TryAdd(1, new BufferTypeMetadata<Primordial<T>, T>());
				MetadataCache<T>.cache.TryAdd(
					2, new BufferTypeMetadata<Composed<Primordial<T>, Primordial<T>, T>, T>());
			}
		}

		/// <summary>
		/// Retrieves metadata required for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <returns>A <see cref="IBufferTypeMetadata{T}"/> instance.</returns>
		public static IBufferTypeMetadata<T> GetMetadata(UInt16 count)
		{
			IBufferTypeMetadata<T> result = MetadataCache<T>.GetFundamental(count);
			while (count - result.Size > 0)
			{
				IBufferTypeMetadata<T> aux = MetadataCache<T>.GetMetadata((UInt16)(count - result.Size));
				lock (MetadataCache<T>.lockObject)
				{
					result = result.Compose(aux);
					MetadataCache<T>.cache.TryAdd(result.Size, result);
				}
			}
			return result;
		}

		/// <summary>
		/// Retrieves the fundamental metadata for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <returns>A <see cref="IBufferTypeMetadata{T}"/> instance.</returns>
		private static IBufferTypeMetadata<T> GetFundamental(UInt16 count)
		{
			lock (MetadataCache<T>.lockObject)
			{
				if (MetadataCache<T>.cache.TryGetValue(count, out IBufferTypeMetadata<T>? metadata)) return metadata;
				UInt16 space = MetadataCache<T>.maxSpace;
				while (count < space) space /= 2;
				IBufferTypeMetadata<T> result = MetadataCache<T>.cache[space];
				while (result.Size * 2 + 1 < count)
				{
					result = result.Double();
					MetadataCache<T>.cache.TryAdd(result.Size, result);
					MetadataCache<T>.maxSpace = result.Size;
				}
				return result;
			}
		}
	}
}