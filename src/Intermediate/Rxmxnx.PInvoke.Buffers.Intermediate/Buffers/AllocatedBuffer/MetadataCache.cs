namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public static partial class AllocatedBuffer
{
	/// <summary>
	/// Name of <see cref="IAllocatedBuffer{Object}.GetMetadata{TBuffer}()"/> method.
	/// </summary>
	private const String getMetadataName = nameof(IAllocatedBuffer<Object>.GetMetadata);
	/// <summary>
	/// Flags of <see cref="IAllocatedBuffer{Object}.GetMetadata{TBuffer}"/> method.
	/// </summary>
	private const BindingFlags getMetadataFlags = BindingFlags.NonPublic | BindingFlags.Static;

	/// <summary>
	/// Type of <see cref="Composed{TBufferA,TBufferB,T}"/>.
	/// </summary>
	private static readonly Type typeofComposed = typeof(Composed<,,>);
	/// <summary>
	/// Flag to check if reflection is disabled.
	/// </summary>
	private static readonly Boolean disabledReflection = !typeof(String).ToString().Contains(nameof(String));

	/// <summary>
	/// Metadata cache.
	/// </summary>
	/// <typeparam name="T"></typeparam>
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
		/// <see cref="MethodInfo"/> of buffer metadata.
		/// </summary>
		private static readonly MethodInfo? getMetadataInfo;
		/// <summary>
		/// Maximum double space.
		/// </summary>
		private static UInt16 maxSpace = 2;

		/// <summary>
		/// Static constructor.
		/// </summary>
		[ExcludeFromCodeCoverage]
		static MetadataCache()
		{
			try
			{
				if (!AllocatedBuffer.disabledReflection)
					MetadataCache<T>.getMetadataInfo = MetadataCache<T>.ReflectGetMetadataMethod();
			}
			catch (Exception)
			{
				// ignored
			}
			lock (MetadataCache<T>.lockObject)
			{
				MetadataCache<T>.cache.TryAdd(1, IAllocatedBuffer<T>.GetMetadata<Primordial<T>>());
				MetadataCache<T>.cache.TryAdd(
					2, IAllocatedBuffer<T>.GetMetadata<Composed<Primordial<T>, Primordial<T>, T>>());
				MetadataCache<T>.cache.TryAdd(
					3,
					IAllocatedBuffer<T>
						.GetMetadata<Composed<Primordial<T>, Composed<Primordial<T>, Primordial<T>, T>, T>>());
			}
		}
		private static MethodInfo? ReflectGetMetadataMethod()
		{
			Type typeofT = typeof(IAllocatedBuffer<T>);
			return typeofT.GetMethod(AllocatedBuffer.getMetadataName, AllocatedBuffer.getMetadataFlags)!;
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
		[UnconditionalSuppressMessage("AOT", "IL2060",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		[UnconditionalSuppressMessage("AOT", "IL3050",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		public static IBufferTypeMetadata<T>? Get<TBufferA, TBufferB>() where TBufferA : struct, IAllocatedBuffer<T>
			where TBufferB : struct, IAllocatedBuffer<T>
		{
			if (MetadataCache<T>.getMetadataInfo is null) return default;
			try
			{
				Type genericType =
					AllocatedBuffer.typeofComposed.MakeGenericType(typeof(TBufferA), typeof(TBufferB), typeof(T));
				MethodInfo getGenericMetadataInfo = MetadataCache<T>.getMetadataInfo.MakeGenericMethod(genericType);
				Func<IBufferTypeMetadata<T>> getGenericMetadata =
					getGenericMetadataInfo.CreateDelegate<Func<IBufferTypeMetadata<T>>>();
				IBufferTypeMetadata<T> result = getGenericMetadata();
				MetadataCache<T>.cache.TryAdd(result.Size, result);
				while (AllocatedBuffer.GetMaxValue(MetadataCache<T>.maxSpace) < result.Size)
					MetadataCache<T>.maxSpace *= 2;
				return result;
			}
			catch (Exception)
			{
				return default;
			}
		}
		/// <summary>
		/// Registers buffer type.
		/// </summary>
		/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
		public static void RegisterBuffer<TBuffer>() where TBuffer : struct, IAllocatedBuffer<T>
		{
			IBufferTypeMetadata<T> metadata = IAllocatedBuffer<T>.GetMetadata<TBuffer>();
			lock (MetadataCache<T>.lockObject)
			{
				if (!MetadataCache<T>.cache.TryAdd(metadata.Size, metadata)) return;
				while (AllocatedBuffer.GetMaxValue(MetadataCache<T>.maxSpace) < metadata.Size)
					MetadataCache<T>.maxSpace *= 2;
				TBuffer.AppendComponent(MetadataCache<T>.cache);
			}
		}
	}
}