namespace Rxmxnx.PInvoke;

public static partial class BufferManager
{
	/// <summary>
	/// Static class to manage metadata buffer types for <typeparamref name="T"/> type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	internal static partial class MetadataManager<T>
	{
		/// <summary>
		/// Retrieves the capacity of a composite buffer of <paramref name="componentA"/> and <paramref name="componentB"/>.
		/// </summary>
		/// <param name="componentA">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
		/// <param name="componentB">A <see cref="BufferTypeMetadata"/> instance.</param>
		/// <param name="isBinary">Output. Indicates whether resulting composition type is binary.</param>
		/// <returns>Resulting composition type capacity.</returns>
		public static Int32 GetCapacity(BufferTypeMetadata<T> componentA, BufferTypeMetadata<T> componentB,
			out Boolean isBinary)
		{
			UInt16 sizeA = componentA.Size;
			UInt16 sizeB = componentB.Size;
			isBinary = false;

			if (!componentA.IsBinary || !componentB.IsBinary ||
			    (componentB.Components is { Length: 2, Span: var bComponents, } && bComponents[0] != bComponents[^1]))
			{
				isBinary = false;
			}
			else
			{
				Int32 diff = sizeB - sizeA;
				isBinary = diff >= 0 && diff <= sizeB;
			}
			return componentA.Size + componentB.Size;
		}
		/// <summary>
		/// Retrieves metadata required for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
#endif
		public static BufferTypeMetadata<T>? GetMetadata(UInt16 count)
		{
#if NET9_0_OR_GREATER
			using (MetadataManager<T>.store.LockObject.EnterScope())
#else
			lock (MetadataManager<T>.store.LockObject)
#endif
			{
				BufferTypeMetadata<T>? nonBinary = MetadataManager<T>.store.GetNonBinaryBuffer(count);
				if (nonBinary is not null) return nonBinary;
			}
			return MetadataManager<T>.GetBinaryMetadata(count, true);
		}
#if !PACKAGE
		/// <summary>
		/// Retrieves metadata required for a buffer of <paramref name="bufferType"/> type.
		/// </summary>
		/// <param name="bufferType">Type of buffer.</param>
		/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
		public static BufferTypeMetadata<T> GetMetadata(
			[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)] Type bufferType)
			=> bufferType == typeof(Atomic<T>) ? Atomic<T>.TypeMetadata : BufferManager.GetMetadata<T>(bufferType);
#endif
		/// <summary>
		/// Retrieves the components array for the composition type of <typeparamref name="TBufferA"/> and
		/// <typeparamref name="TBufferB"/> .
		/// </summary>
		/// <typeparam name="TBufferA">The type of low buffer.</typeparam>
		/// <typeparam name="TBufferB">The type of high buffer.</typeparam>
		/// <returns>The components array for the composition type.</returns>
#if !PACKAGE && NET7_0_OR_GREATER
		[ExcludeFromCodeCoverage]
#endif
		public static BufferTypeMetadata<T>[] GetComponents<
			[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)] TBufferA,
			[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)] TBufferB>()
			where TBufferA : struct, IManagedBinaryBuffer<T> where TBufferB : struct, IManagedBinaryBuffer<T>

		{
			BufferTypeMetadata<T>[] components = new BufferTypeMetadata<T>[2];
			components[0] = BufferManager.GetMetadata<T, TBufferA>();
			components[1] = BufferManager.GetMetadata<T, TBufferB>();
			return components;
		}
		/// <summary>
		/// Prepares internal metadata cache for allocations of <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <exception cref="InvalidOperationException">Throw if missing metadata for any buffer component.</exception>
		public static void PrepareBinaryMetadata(UInt16 count)
		{
			Type typeofT = typeof(T);
			BufferTypeMetadata<T>? metadata = default;
			foreach (UInt16 comp in BufferManager.GetBinaryComponents(count))
			{
				BufferTypeMetadata<T>? compMetadata = MetadataManager<T>.GetFundamental(comp);
				ValidationUtilities.ThrowIfNullMetadata(typeofT, comp, compMetadata is null);
				if (metadata is null)
				{
					metadata = compMetadata;
					continue;
				}

				UInt16 composeSize = (UInt16)(comp + metadata.Size);
				metadata = MetadataManager<T>.GetBinaryMetadata(composeSize, false);
				ValidationUtilities.ThrowIfNullMetadata(typeofT, composeSize, metadata is null);
			}
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
		[UnconditionalSuppressMessage("Trimming", "IL2055")]
		[UnconditionalSuppressMessage("Trimming", "IL2055")]
		public static BufferTypeMetadata<T>? ComposeWithReflection(
			[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)] Type typeofA,
			[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)] Type typeofB)
		{
#if NET7_0_OR_GREATER
			if (MetadataManager<T>.store.GetMetadataInfo is null) return default;
			Type? genericType = default;
			BufferTypeMetadata<T>? result;
			try
			{
#pragma warning disable IL3050
				genericType = BufferManager.TypeofComposite.MakeGenericType(typeofA, typeofB, typeof(T));
				MethodInfo getGenericMetadataInfo =
					MetadataManager<T>.store.GetMetadataInfo.MakeGenericMethod(genericType);
#pragma warning restore IL3050
				Func<BufferTypeMetadata<T>> getGenericMetadata =
					getGenericMetadataInfo.CreateDelegate<Func<BufferTypeMetadata<T>>>();
				result = getGenericMetadata();
			}
			catch (Exception)
			{
				// This may never be called.
				result = ManagedBinaryBuffer<T>.GetMetadata(genericType);
			}
#else
			if (!BufferManager.GetMetadata<T>(typeofB).IsBinary || !BufferManager.BufferAutoCompositionEnabled)
				return default;
			BufferTypeMetadata<T>? result = default;
			try
			{
				Type genericType = BufferManager.TypeofComposite.MakeGenericType(typeofA, typeofB, typeof(T));
				result = BufferManager.GetMetadata<T>(genericType);
			}
			catch (Exception)
			{
				// Ignore
			}
#endif
			return MetadataManager<T>.AddBinaryMetadata(result);
		}
		/// <summary>
		/// Registers buffer type.
		/// </summary>
		/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
		public static void RegisterBuffer<
			[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)] TBuffer>()
			where TBuffer : struct, IManagedBuffer<T>
		{
#if NET7_0_OR_GREATER
			BufferTypeMetadata<T> typeMetadata = IManagedBuffer<T>.GetMetadata<TBuffer>();
#else
			BufferTypeMetadata<T> typeMetadata = BufferManager.GetMetadata<T, TBuffer>();
#endif
#if NET9_0_OR_GREATER
			using (MetadataManager<T>.store.LockObject.EnterScope())
#else
			lock (MetadataManager<T>.store.LockObject)
#endif
			{
				if (!MetadataManager<T>.store.Add(typeMetadata) || !typeMetadata.IsBinary) return;
				while (BufferManager.GetMaxValue(MetadataManager<T>.store.MaxSpace) < typeMetadata.Size)
					MetadataManager<T>.store.MaxSpace *= 2;
#if NET7_0_OR_GREATER
				TBuffer.AppendComponent(MetadataManager<T>.store.BinaryBuffers);
#else
				typeMetadata.AppendComponent(MetadataManager<T>.store.BinaryBuffers);
#endif
			}
		}
#if NET7_0_OR_GREATER && BINARY_SPACES
		/// <summary>
		/// Registers space type.
		/// </summary>
		/// <typeparam name="TSpace">Type of the space.</typeparam>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public static void RegisterBufferSpace<
			[DynamicallyAccessedMembers(BufferManager.DynamicallyAccessedMembers)]
			TSpace>() where TSpace : struct, IManagedBinaryBuffer<TSpace, T>
		{
			BufferTypeMetadata<T> typeMetadata = IManagedBuffer<T>.GetMetadata<TSpace>();
			Boolean isBinary = typeMetadata.IsBinary;
			Span<UInt16> sizes = MetadataManager<T>.WriteSizes(typeMetadata, stackalloc UInt16[3]);
			ValidationUtilities.ThrowIfNotSpace(isBinary, sizes, typeof(TSpace));
#if NET9_0_OR_GREATER
			using (MetadataManager<T>.store.LockObject.EnterScope())
#else
			lock (MetadataManager<T>.store.LockObject)
#endif
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
		/// <param name="trace">Indicates whether trace should be written.</param>
		[ExcludeFromCodeCoverage]
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6670)]
#if !PACKAGE
		[SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
#endif
		public static void PrintMetadata(Boolean trace)
		{
			if (!trace) return;
#if NET9_0_OR_GREATER
			using (MetadataManager<T>.store.LockObject.EnterScope())
#else
			lock (MetadataManager<T>.store.LockObject)
#endif
			{
				foreach (UInt16 key in MetadataManager<T>.store.BinaryBuffers.Keys)
				{
					BufferTypeMetadata<T> m = MetadataManager<T>.store.BinaryBuffers[key];
					Trace.WriteLine(
						$"{typeof(T)} {key}({String.Join(", ", m.Components.ToArray().Select(k => k.Size))}): {m.IsBinary}.");
				}
				Trace.WriteLine($"{typeof(T)}: {MetadataManager<T>.store.BinaryBuffers.Count}");
			}
		}
#endif
	}
}