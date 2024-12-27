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
			lock (MetadataManager<T>.store.LockObject)
			{
				BufferTypeMetadata<T>? nonBinary = MetadataManager<T>.store.GetNonBinaryBuffer(count);
				if (nonBinary is not null) return nonBinary;
			}
			return MetadataManager<T>.GetBinaryMetadata(count, true);
		}
		/// <summary>
		/// Creates <see cref="BufferTypeMetadata{T}"/> for <see cref="Composite{TBufferA,TBufferB,T}"/>.
		/// </summary>
		/// <param name="typeofA">The type of low buffer.</param>
		/// <param name="typeofB">The type of high buffer.</param>
		/// <returns>
		/// The <see cref="BufferTypeMetadata{T}"/> for <see cref="Composite{TBufferA,TBufferB,T}"/> buffer.
		/// </returns>
		[UnconditionalSuppressMessage("AOT", "IL2055")]
		[UnconditionalSuppressMessage("AOT", "IL2060")]
		[UnconditionalSuppressMessage("AOT", "IL2077")]
		[UnconditionalSuppressMessage("AOT", "IL3050")]
		public static BufferTypeMetadata<T>? ComposeWithReflection(Type typeofA, Type typeofB)
		{
			if (MetadataManager<T>.store.GetMetadataInfo is null) return default;
			Type? genericType = default;
			BufferTypeMetadata<T>? result;
			try
			{
				genericType = BufferManager.typeofComposite.MakeGenericType(typeofA, typeofB, typeof(T));
				MethodInfo getGenericMetadataInfo =
					MetadataManager<T>.store.GetMetadataInfo.MakeGenericMethod(genericType);
				Func<BufferTypeMetadata<T>> getGenericMetadata =
					getGenericMetadataInfo.CreateDelegate<Func<BufferTypeMetadata<T>>>();
				result = getGenericMetadata();
			}
			catch (Exception)
			{
				// This may never be called.
				result = IManagedBinaryBuffer<T>.GetMetadata(genericType);
			}
			return MetadataManager<T>.AddBinaryMetadata(result);
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
				TBuffer.AppendComponent(MetadataManager<T>.store.BinaryBuffers);
			}
		}
#if BINARY_SPACES
		/// <summary>
		/// Registers space type.
		/// </summary>
		/// <typeparam name="TSpace">Type of the space.</typeparam>
		[ExcludeFromCodeCoverage]
		public static void RegisterBufferSpace<TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, T>
		{
			BufferTypeMetadata<T> typeMetadata = IManagedBuffer<T>.GetMetadata<TSpace>();
			Boolean isBinary = typeMetadata.IsBinary;
			Span<UInt16> sizes = MetadataManager<T>.WriteSizes(typeMetadata, stackalloc UInt16[3]);
			ValidationUtilities.ThrowIfNotSpace(isBinary, sizes, typeof(TSpace));
			lock (MetadataManager<T>.store.LockObject)
			{
				using StaticCompositionHelper<T> helper = StaticCompositionHelper<T>.Create<TSpace>();
				try
				{
					TSpace.StaticCompose<TSpace>(sizes[0], helper);
				}
				finally
				{
					helper.Append(MetadataManager<T>.store.BinaryBuffers);
					if (MetadataManager<T>.store.MaxSpace < sizes[0])
						MetadataManager<T>.store.MaxSpace = sizes[0];
				}
			}
		}
#endif
#if !PACKAGE
		/// <summary>
		/// Prints metadata dictionary.
		/// </summary>
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6670)]
		public static void PrintMetadata()
		{
			lock (MetadataManager<T>.store.LockObject)
			{
				foreach (UInt16 key in MetadataManager<T>.store.BinaryBuffers.Keys)
				{
					BufferTypeMetadata<T> m = MetadataManager<T>.store.BinaryBuffers[key];
					Trace.WriteLine(
						$"{typeof(T)} {key}({String.Join(", ", m.Components.Select(k => k.Size))}): {m.IsBinary}.");
				}
				Trace.WriteLine($"{typeof(T)}: {MetadataManager<T>.store.BinaryBuffers.Count}");
			}
		}
#endif
	}
}
#pragma warning restore CA2252