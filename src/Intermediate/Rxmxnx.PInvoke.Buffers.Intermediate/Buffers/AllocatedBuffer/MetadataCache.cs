namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public static partial class AllocatedBuffer
{
	/// <summary>
	/// Static class to store metadata cache for <typeparamref name="T"/> type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	internal static partial class MetadataCache<T>
	{
		/// <summary>
		/// Retrieves metadata required for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <returns>A <see cref="ManagedBufferMetadata{T}"/> instance.</returns>
		public static ManagedBufferMetadata<T>? GetMetadata(UInt16 count)
		{
			ManagedBufferMetadata<T>? result = MetadataCache<T>.GetFundamental(count);
			if (result is null) return result;
			while (count - result.Size > 0)
			{
				ManagedBufferMetadata<T>? aux = MetadataCache<T>.GetMetadata((UInt16)(count - result.Size));
				lock (MetadataCache<T>.cache.LockObject)
				{
					// Auxiliary metadata not found. Use minimal.
					if (aux is null)
						return MetadataCache<T>.cache.GetMinimal(count);
					result = result.Compose(aux);
					if (result is null)
						// Unable to create composed metadata. Use minimal.
						return MetadataCache<T>.cache.GetMinimal(count);
					MetadataCache<T>.cache.Add(result);
				}
			}
			return result;
		}
		/// <summary>
		/// Creates <see cref="ManagedBufferMetadata{T}"/> for <see cref="Composed{TBufferA,TBufferB,T}"/>.
		/// </summary>
		/// <param name="typeofA">The type of low buffer.</param>
		/// <param name="typeofB">The type of high buffer.</param>
		/// <returns>
		/// The <see cref="ManagedBufferMetadata{T}"/> for <see cref="Composed{TBufferA,TBufferB,T}"/> buffer.
		/// </returns>
		[UnconditionalSuppressMessage("AOT", "IL2055",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		[UnconditionalSuppressMessage("AOT", "IL2060",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		[UnconditionalSuppressMessage("AOT", "IL3050",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		public static ManagedBufferMetadata<T>? CreateComposedWithReflection(Type typeofA, Type typeofB)
		{
			if (MetadataCache<T>.cache.GetMetadataInfo is null) return default;
			try
			{
				Type genericType = AllocatedBuffer.typeofComposed.MakeGenericType(typeofA, typeofB, typeof(T));
				MethodInfo getGenericMetadataInfo =
					MetadataCache<T>.cache.GetMetadataInfo.MakeGenericMethod(genericType);
				Func<ManagedBufferMetadata<T>> getGenericMetadata =
					getGenericMetadataInfo.CreateDelegate<Func<ManagedBufferMetadata<T>>>();
				ManagedBufferMetadata<T> result = getGenericMetadata();
				MetadataCache<T>.cache.Add(result);
				while (AllocatedBuffer.GetMaxValue(MetadataCache<T>.cache.MaxSpace) < result.Size)
					MetadataCache<T>.cache.MaxSpace *= 2;
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
		public static void RegisterBuffer<TBuffer>() where TBuffer : struct, IManagedBuffer<T>
		{
			ManagedBufferMetadata<T> metadata = IManagedBuffer<T>.GetMetadata<TBuffer>();
			lock (MetadataCache<T>.cache.LockObject)
			{
				if (!MetadataCache<T>.cache.Add(metadata) || !TBuffer.IsBinary) return;
				while (AllocatedBuffer.GetMaxValue(MetadataCache<T>.cache.MaxSpace) < metadata.Size)
					MetadataCache<T>.cache.MaxSpace *= 2;
				TBuffer.AppendComponent(MetadataCache<T>.cache.Buffers);
			}
		}
		/// <summary>
		/// Registers space type.
		/// </summary>
		/// <typeparam name="TSpace">Type of the space.</typeparam>
		public static void RegisterBufferSpace<TSpace>() where TSpace : struct, IManagedBuffer<T>
		{
			ValidationUtilities.ThrowIfNotSpace(TSpace.IsPure, TSpace.Metadata.Size, typeof(TSpace));
			MetadataCache<T>.RegisterBuffer<TSpace>();
			lock (MetadataCache<T>.cache.LockObject)
				TSpace.Append<TSpace>(MetadataCache<T>.cache.Buffers);
		}
	}
}
#pragma warning restore CA2252