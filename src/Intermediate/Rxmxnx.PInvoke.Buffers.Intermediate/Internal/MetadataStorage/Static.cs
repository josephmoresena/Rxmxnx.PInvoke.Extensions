namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Metadata storage utility class.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal static partial class MetadataStorage
{
	/// <summary>
	/// Maximum capacity of the buffers.
	/// </summary>
	public static UInt16 MaxCapacity => 0; // Standard storage.
	/// <summary>
	/// Singleton instance.
	/// </summary>
#if !NET8_0_OR_GREATER
	public static readonly IMetadataStorageExt Instance = new ObjectStandardStorage();
#else
	public static readonly IMetadataStorageExt Instance = MetadataStorage.MaxCapacity switch
	{
		0 => new ObjectStandardStorage(),
		31 => new ObjectMetadataStorage31(),
		127 => new ObjectMetadataStorage127(),
		2047 => new ObjectMetadataStorage2047(false),
		_ => new ObjectMetadataStorage2047(true),
	};
#endif

	/// <summary>
	/// Initializes a <see cref="MetadataStorage{T}"/> instance.
	/// </summary>
	/// <param name="manager">A <see cref="MetadataStorage{T}"/> instance.</param>
	/// <param name="initial">The <see cref="BufferTypeMetadata{Object}"/> initial instance.</param>
	public static void Initialize(MetadataStorage<Object> manager, BufferTypeMetadata<Object> initial)
	{
		ref BufferTypeMetadata<Object>? refComponent = ref Unsafe.Add(ref manager.MetadataReference, initial.Size - 1);
		if (refComponent is not null) return;

		refComponent = initial;
		foreach (BufferTypeMetadata<Object> metadataComponent in initial.Components.Span)
			MetadataStorage.Initialize(manager, metadataComponent);
	}
}

internal abstract partial class MetadataStorage<T>
{
	/// <summary>
	/// Adds <paramref name="typeMetadata"/> to binary cache.
	/// </summary>
	/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	public static void AddBinaryMetadata(BufferTypeMetadata<T> typeMetadata)
	{
		BinaryMap<T> binaryMap =
			MetadataStorage.Instance.GetBinaryMap(typeMetadata.Size, ref MetadataStorage<T>.instance, true);
		binaryMap[typeMetadata.Size] = typeMetadata;
	}
	/// <summary>
	/// Tries to add the current component
	/// </summary>
	/// <param name="component">The <see cref="BufferTypeMetadata{T}"/> instance to add.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="component"/> was added successfully; otherwise, <see langword="false"/>.
	/// </returns>
	public static Boolean TryAdd(BufferTypeMetadata<T> component)
	{
		BinaryMap<T> binaryMap =
			MetadataStorage.Instance.GetBinaryMap(component.Size, ref MetadataStorage<T>.instance, true);
		ref BufferTypeMetadata<T>? reference = ref binaryMap[component.Size];
		if (reference is not null) return false;
		reference = component;
		return true;
	}
	/// <summary>
	/// Retrieves metadata required for a buffer with <paramref name="count"/> items.
	/// </summary>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	public static BufferTypeMetadata<T>? GetMetadata(UInt16 count)
	{
		Debug.Assert(count > 0);
		Boolean allowNonBinaryMinimal =
			!MetadataStorage<T>.IsBinaryPrepared(count) && !BuffersHelper.BufferAutoCompositionEnabled;
		if (NonBinaryStore.GetNonBinary(count, allowNonBinaryMinimal) is { } nonBinary)
			// Exact non-binary buffer. Allow minimal at first only if unable to retrieve a binary buffer.
			return nonBinary;
#if NET8_0_OR_GREATER
		if (MetadataStorage.MaxCapacity > 0 && count > MetadataStorage.MaxCapacity)
			// Binary capacity doesn't allow current count.
			return default;
#endif
		BinaryMap<T> binaryMap = MetadataStorage<T>.GetBinaryMap(count);
		BufferTypeMetadata<T>? binary = MetadataStorage<T>.GetBinaryMetadata(binaryMap, count, true);
		return binary ?? NonBinaryStore.GetNonBinary(count, true); // Approximate non-Binary buffer.
	}
	/// <summary>
	/// Prepares internal metadata cache for allocations of <paramref name="count"/> items.
	/// </summary>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <exception cref="InvalidOperationException">Throw if missing metadata for any buffer component.</exception>
	public static void PrepareBinaryMetadata(UInt16 count)
	{
		Debug.Assert(count > 0);
		Type typeofT = typeof(T);
		BufferTypeMetadata<T>? metadata = default;
		BinaryMap<T> binaryMap = MetadataStorage<T>.GetBinaryMap(count);
		ValidationUtilities.ThrowIfNullMetadata(typeofT, count, !binaryMap.IsAllowed(count));
		Span<UInt16> components = BuffersHelper.GetBinaryComponents(stackalloc UInt16[16], count);
		foreach (UInt16 comp in components)
		{
			BufferTypeMetadata<T>? compMetadata = MetadataStorage<T>.GetFundamental(binaryMap, comp);
			ValidationUtilities.ThrowIfNullMetadata(typeofT, comp, compMetadata is null);
			if (metadata is null)
			{
				metadata = compMetadata;
				continue;
			}

			UInt16 composeSize = (UInt16)(comp + metadata.Size);
			metadata = MetadataStorage<T>.GetBinaryMetadata(binaryMap, composeSize, false);
			ValidationUtilities.ThrowIfNullMetadata(typeofT, composeSize, metadata is null);
		}
	}
	/// <summary>
	/// Registers buffer type.
	/// </summary>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	public static void RegisterBuffer<[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer>()
		where TBuffer : struct, IManagedBuffer<T>
	{
#if NET7_0_OR_GREATER
		BufferTypeMetadata<T> typeMetadata = IManagedBuffer<T>.GetMetadata<TBuffer>();
#else
		BufferTypeMetadata<T> typeMetadata = BuffersHelper.GetMetadata<T, TBuffer>();
#endif
		if (!typeMetadata.IsBinary)
		{
			NonBinaryStore.AddNonBinary(typeMetadata);
			return;
		}
		if (!MetadataStorage<T>.TryAdd(typeMetadata)) return;
#if NET7_0_OR_GREATER
		TBuffer.AppendComponent(MetadataStorage.Instance);
#else
		typeMetadata.AppendComponent(MetadataStorage.Instance);
#endif
	}
	/// <summary>
	/// Initialize pages.
	/// </summary>
	/// <param name="count">Requested count.</param>
	/// <param name="pageLength">Current page length.</param>
	/// <param name="page">Reference to the current page slot.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void InitializePages(UInt16 count, Int32 pageLength, ref BufferTypeMetadata<T>?[]? page)
	{
		while (count >= pageLength)
		{
			if (page is null)
				Interlocked.CompareExchange(ref page, new BufferTypeMetadata<T>?[pageLength], null);
			pageLength *= 2;
			page = ref Unsafe.Add(ref page, 1)!;
		}
	}
#if !PACKAGE
	/// <summary>
	/// Retrieves the current binary map.
	/// </summary>
	/// <returns>A <see cref="ReadOnlySpan{BufferTypeMetadata}"/> instances.</returns>
	[ExcludeFromCodeCoverage]
	public static ReadOnlySpan<BufferTypeMetadata<T>?> GetBinaryMap()
	{
		if (MetadataStorage<T>.instance is null) return default;
		return MemoryMarshal.CreateReadOnlySpan(ref MetadataStorage<T>.instance.MetadataReference,
		                                        MetadataStorage<T>.instance.Capacity);
	}
	/// <summary>
	/// Retrieves the current binary slots.
	/// </summary>
	/// <returns>A <see cref="ReadOnlySpan{BufferTypeMetadata}"/> instances.</returns>
	[ExcludeFromCodeCoverage]
	public static ReadOnlySpan<BufferTypeMetadata<T>?[]> GetBinarySlots()
	{
		if ((MetadataStorage<T>.instance as IBinarySlotsOwner<T>)?.Slots is not { } slots) return default;
		BufferTypeMetadata<T>?[][] result = new BufferTypeMetadata<T>?[slots.Length][];
		for (Int32 i = 0; i < result.Length; i++)
			result[i] = slots[i] ?? [];
		return result.AsSpan();
	}
#endif
}