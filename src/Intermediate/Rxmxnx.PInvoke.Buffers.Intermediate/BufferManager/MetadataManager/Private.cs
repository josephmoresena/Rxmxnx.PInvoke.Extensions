namespace Rxmxnx.PInvoke;

public static partial class BufferManager
{
	internal static partial class MetadataManager<T>
	{
		/// <summary>
		/// Internal cache.
		/// </summary>
#pragma warning disable S2743
		private static readonly MetadataStore store = new();
#pragma warning restore S2743

		/// <summary>
		/// Retrieves the fundamental metadata for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
		private static BufferTypeMetadata<T>? GetFundamental(UInt16 count)
		{
			lock (MetadataManager<T>.store.LockObject)
			{
				if (MetadataManager<T>.store.Buffers.TryGetValue(count, out BufferTypeMetadata<T>? metadata))
					return metadata;
				UInt16 space = MetadataManager<T>.store.MaxSpace;
				while (count < space) space /= 2;
				BufferTypeMetadata<T>? result = MetadataManager<T>.store.Buffers[space];
				while (BufferManager.GetMaxValue(result.Size) < count)
				{
					result = result.Double();
					if (result is null) break;
					MetadataManager<T>.store.Add(result);
					MetadataManager<T>.store.MaxSpace = result.Size;
				}
				return result;
			}
		}
	}
}