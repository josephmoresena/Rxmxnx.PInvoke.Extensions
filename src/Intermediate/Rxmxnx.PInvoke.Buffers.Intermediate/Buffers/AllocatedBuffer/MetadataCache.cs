namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public static partial class AllocatedBuffer
{
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	private static readonly Type typeofComposed = typeof(Composed<,,>);
	[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
	private static readonly Type typeofMetadata = typeof(BufferTypeMetadata<,>);

	internal static class MetadataCache<T>
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
				MetadataCache<T>.cache.TryAdd(
					3,
					new BufferTypeMetadata<Composed<Primordial<T>, Composed<Primordial<T>, Primordial<T>, T>, T>, T>());
			}
		}

		/// <summary>
		/// Retrieves metadata required for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <returns>A <see cref="IBufferTypeMetadata{T}"/> instance.</returns>
		public static IBufferTypeMetadata<T>? GetMetadata(UInt16 count)
		{
			IBufferTypeMetadata<T>? result = MetadataCache<T>.GetFundamental(count);
			if (result is null) return result;
			while (count - result.Size > 0)
			{
				IBufferTypeMetadata<T>? aux = MetadataCache<T>.GetMetadata((UInt16)(count - result.Size));
				if (aux is null) return null;
				lock (MetadataCache<T>.lockObject)
				{
					result = result.Compose(aux);
					if (result is null) break;
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
		private static IBufferTypeMetadata<T>? GetFundamental(UInt16 count)
		{
			lock (MetadataCache<T>.lockObject)
			{
				if (MetadataCache<T>.cache.TryGetValue(count, out IBufferTypeMetadata<T>? metadata)) return metadata;
				UInt16 space = MetadataCache<T>.maxSpace;
				while (count < space) space /= 2;
				IBufferTypeMetadata<T>? result = MetadataCache<T>.cache[space];
				while (AllocatedBuffer.GetMaxValue(result.Size) < count)
				{
					result = result.Double();
					if (result is null) break;
					MetadataCache<T>.cache.TryAdd(result.Size, result);
					MetadataCache<T>.maxSpace = result.Size;
				}
				return result;
			}
		}

		[UnconditionalSuppressMessage("AOT", "IL2055",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		[UnconditionalSuppressMessage("AOT", "IL3050",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		public static IBufferTypeMetadata<T>? Get<TBufferA, TBufferB>() where TBufferA : struct, IAllocatedBuffer<T>
			where TBufferB : struct, IAllocatedBuffer<T>
		{
			UInt16 key = (UInt16)(TBufferA.Capacity + TBufferB.Capacity);
			lock (MetadataCache<T>.lockObject)
			{
				if (MetadataCache<T>.cache.TryGetValue(key, out IBufferTypeMetadata<T>? result))
					return result;
				try
				{
					Type genericType =
						AllocatedBuffer.typeofComposed.MakeGenericType(typeof(TBufferA), typeof(TBufferB), typeof(T));
					Type genericMetadataType = AllocatedBuffer.typeofMetadata.MakeGenericType(genericType, typeof(T));
					result = (IBufferTypeMetadata<T>)Activator.CreateInstance(genericMetadataType)!;
					MetadataCache<T>.cache.TryAdd(key, result);
					while (AllocatedBuffer.GetMaxValue(MetadataCache<T>.maxSpace) < result.Size)
						MetadataCache<T>.maxSpace *= 2;
					return result;
				}
				catch (Exception)
				{
					return default;
				}
			}
		}
		/// <summary>
		/// Registers buffer type.
		/// </summary>
		/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
		public static void RegisterBuffer<TBuffer>() where TBuffer : struct, IAllocatedBuffer<T>
		{
			lock (MetadataCache<T>.lockObject)
			{
				if (MetadataCache<T>.cache.ContainsKey((UInt16)TBuffer.Capacity)) return;
				MetadataCache<T>.cache[(UInt16)TBuffer.Capacity] = new BufferTypeMetadata<TBuffer, T>();
				while (AllocatedBuffer.GetMaxValue(MetadataCache<T>.maxSpace) < TBuffer.Capacity)
					MetadataCache<T>.maxSpace *= 2;
				TBuffer.AppendComponent(MetadataCache<T>.cache);
			}
		}
	}
}