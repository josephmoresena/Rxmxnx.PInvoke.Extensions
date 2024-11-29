namespace Rxmxnx.PInvoke;

#pragma warning disable CA2252
public static partial class BufferManager
{
	/// <summary>
	/// Static class to manage metadata buffer types for <typeparamref name="T"/> type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	internal static partial class MetadataManager<T>
	{
		/// <summary>
		/// Retrieves metadata required for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
		public static BufferTypeMetadata<T>? GetMetadata(UInt16 count)
		{
			BufferTypeMetadata<T>? result = MetadataManager<T>.GetFundamental(count);
			if (result is null) return result;
			while (count - result.Size > 0)
			{
				BufferTypeMetadata<T>? aux = MetadataManager<T>.GetMetadata((UInt16)(count - result.Size));
				lock (MetadataManager<T>.store.LockObject)
				{
					// Auxiliary metadata not found. Use minimal.
					if (aux is null)
						return MetadataManager<T>.store.GetMinimal(count);
					result = result.Compose(aux);
					if (result is null)
						// Unable to create composed metadata. Use minimal.
						return MetadataManager<T>.store.GetMinimal(count);
					MetadataManager<T>.store.Add(result);
				}
			}
			return result;
		}
		/// <summary>
		/// Creates <see cref="BufferTypeMetadata{T}"/> for <see cref="Composite{TBufferA,TBufferB,T}"/>.
		/// </summary>
		/// <param name="typeofA">The type of low buffer.</param>
		/// <param name="typeofB">The type of high buffer.</param>
		/// <returns>
		/// The <see cref="BufferTypeMetadata{T}"/> for <see cref="Composite{TBufferA,TBufferB,T}"/> buffer.
		/// </returns>
		[UnconditionalSuppressMessage("AOT", "IL2055",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		[UnconditionalSuppressMessage("AOT", "IL2060",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		[UnconditionalSuppressMessage("AOT", "IL3050",
		                              Justification = SuppressMessageConstants.AvoidableReflectionUseJustification)]
		public static BufferTypeMetadata<T>? ComposeWithReflection(Type typeofA, Type typeofB)
		{
			if (MetadataManager<T>.store.GetMetadataInfo is null) return default;
			try
			{
				Type genericType = BufferManager.typeofComposite.MakeGenericType(typeofA, typeofB, typeof(T));
				MethodInfo getGenericMetadataInfo =
					MetadataManager<T>.store.GetMetadataInfo.MakeGenericMethod(genericType);
				Func<BufferTypeMetadata<T>> getGenericMetadata =
					getGenericMetadataInfo.CreateDelegate<Func<BufferTypeMetadata<T>>>();
				BufferTypeMetadata<T> result = getGenericMetadata();
				MetadataManager<T>.store.Add(result);
				while (BufferManager.GetMaxValue(MetadataManager<T>.store.MaxSpace) < result.Size)
					MetadataManager<T>.store.MaxSpace *= 2;
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
			BufferTypeMetadata<T> typeMetadata = IManagedBuffer<T>.GetMetadata<TBuffer>();
			lock (MetadataManager<T>.store.LockObject)
			{
				if (!MetadataManager<T>.store.Add(typeMetadata) || !typeMetadata.IsBinary) return;
				while (BufferManager.GetMaxValue(MetadataManager<T>.store.MaxSpace) < typeMetadata.Size)
					MetadataManager<T>.store.MaxSpace *= 2;
				TBuffer.AppendComponent(MetadataManager<T>.store.Buffers);
			}
		}
		/// <summary>
		/// Registers space type.
		/// </summary>
		/// <typeparam name="TSpace">Type of the space.</typeparam>
		public static void RegisterBufferSpace<TSpace>() where TSpace : struct, IManagedBuffer<T>
		{
			BufferTypeMetadata<T> typeMetadata = IManagedBuffer<T>.GetMetadata<TSpace>();
			Boolean isBinary = typeMetadata.IsBinary;
			Span<UInt16> sizes = MetadataManager<T>.WriteSizes(typeMetadata, stackalloc UInt16[3]);
			ValidationUtilities.ThrowIfNotSpace(isBinary, sizes, typeof(TSpace));
			lock (MetadataManager<T>.store.LockObject)
			{
				using StaticCompositionHelper<T> helper = new(sizes[0]);
				try
				{
					TSpace.StaticCompose<TSpace>(helper);
				}
				finally
				{
					helper.Append(MetadataManager<T>.store.Buffers);
					if (MetadataManager<T>.store.MaxSpace < sizes[0])
						MetadataManager<T>.store.MaxSpace = sizes[0];
				}
			}
		}
#if !PACKAGE
		/// <summary>
		/// Prints metadata dictionary.
		/// </summary>
		public static void PrintMetadata()
		{
			foreach (UInt16 key in MetadataManager<T>.store.Buffers.Keys)
			{
				BufferTypeMetadata<T> m = MetadataManager<T>.store.Buffers[key];
				Trace.WriteLine(
					$"{typeof(T)} {key}({String.Join(", ", m.Components.Select(k => k.Size))}): {m.IsBinary}.");
			}
			Trace.WriteLine($"{typeof(T)}: {MetadataManager<T>.store.Buffers.Count}");
		}
#endif
	}
}
#pragma warning restore CA2252