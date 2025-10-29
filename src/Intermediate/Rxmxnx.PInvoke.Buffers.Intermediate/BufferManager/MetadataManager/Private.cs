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
		/// Retrieves binary metadata required for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <param name="allowMinimal">Allow to return minimal buffer.</param>
		/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
		private static BufferTypeMetadata<T>? GetBinaryMetadata(UInt16 count, Boolean allowMinimal)
		{
			BufferTypeMetadata<T>? result = MetadataManager<T>.GetFundamental(count);
			if (result is null) return result;
			while (count - result.Size > 0)
			{
				UInt16 diff = (UInt16)(count - result.Size);
				BufferTypeMetadata<T>? aux = MetadataManager<T>.GetBinaryMetadata(diff, false);
#if NET9_0_OR_GREATER
				using (MetadataManager<T>.store.LockObject.EnterScope())
#else
				lock (MetadataManager<T>.store.LockObject)
#endif
				{
					// Auxiliary metadata not found. Use minimal.
					if (aux is null)
						return allowMinimal ? MetadataManager<T>.store.GetMinimal(count) : default;
					result = result.Compose(aux);
					if (result is null)
						// Unable to create composed metadata. Use minimal.
						return allowMinimal ? MetadataManager<T>.store.GetMinimal(count) : default;
					MetadataManager<T>.store.Add(result);
				}
			}
			return result;
		}
		/// <summary>
		/// Retrieves the fundamental metadata for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
		private static BufferTypeMetadata<T>? GetFundamental(UInt16 count)
		{
#if NET9_0_OR_GREATER
			using (MetadataManager<T>.store.LockObject.EnterScope())
#else
			lock (MetadataManager<T>.store.LockObject)
#endif
			{
				if (MetadataManager<T>.store.BinaryBuffers.TryGetValue(count, out BufferTypeMetadata<T>? metadata))
					return metadata;
				UInt16 space = MetadataManager<T>.store.MaxSpace;
				while (count < space) space /= 2;
				BufferTypeMetadata<T>? result = MetadataManager<T>.store.BinaryBuffers[space];
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
#if NET7_0_OR_GREATER && BINARY_SPACES
		/// <summary>
		/// Writes on <paramref name="sizes"/> the sizes of <paramref name="metadata"/>.
		/// </summary>
		/// <param name="metadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
		/// <param name="sizes">Buffer to write.</param>
		/// <returns>Written buffer.</returns>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		private static Span<UInt16> WriteSizes(BufferTypeMetadata<T> metadata, Span<UInt16> sizes)
		{
			sizes[0] = metadata.Size;
			if (metadata.Components.Length != 2) return sizes;
			sizes[1] = metadata.Components[1].Size;
			sizes[2] = metadata.Components[0].Size;
			return sizes;
		}
#endif
		/// <summary>
		/// Adds <paramref name="typeMetadata"/> to binary cache.
		/// </summary>
		/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
		/// <returns><paramref name="typeMetadata"/>.</returns>
		[return: NotNullIfNotNull(nameof(typeMetadata))]
		private static BufferTypeMetadata<T>? AddBinaryMetadata(BufferTypeMetadata<T>? typeMetadata)
		{
			if (typeMetadata is null) return typeMetadata;
			MetadataManager<T>.store.Add(typeMetadata);
			while (BufferManager.GetMaxValue(MetadataManager<T>.store.MaxSpace) < typeMetadata.Size)
				MetadataManager<T>.store.MaxSpace *= 2;
			return typeMetadata;
		}
	}
}