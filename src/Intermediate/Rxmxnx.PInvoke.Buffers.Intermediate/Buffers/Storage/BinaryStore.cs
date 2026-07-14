namespace Rxmxnx.PInvoke.Buffers.Storage;

/// <summary>
/// Generic storage.
/// </summary>
/// <typeparam name="TMain">Type of main storage.</typeparam>
/// <typeparam name="T">Type of items in the buffer.</typeparam>
internal static class BinaryStore<TMain, T> where TMain : struct, IMainBinaryStore<T>
{
	/// <summary>
	/// Initial storage.
	/// </summary>
	private static readonly TMain initial;
	/// <summary>
	/// Additional slots.
	/// </summary>
	private static readonly BufferTypeMetadata<T>?[]?[] slots;
	/// <summary>
	/// Cached capacity of the currently allocated pages.
	/// </summary>
	// ReSharper disable once StaticMemberInGenericType
	private static Int32 currentSlotCapacity;

	/// <summary>
	/// Retrieves the current capacity.
	/// </summary>
	public static Int32 CurrentCapacity
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => BinaryStore<TMain, T>.initial.Length + Volatile.Read(ref BinaryStore<TMain, T>.currentSlotCapacity);
	}
#if NET8_0_OR_GREATER
	/// <summary>
	/// Maximum capacity.
	/// </summary>
	public static Int32 MaxCapacity
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => ((BinaryStore<TMain, T>.initial.Length + 1) << BinaryStore<TMain, T>.initial.SlotCount) - 1;
	}
#endif

	/// <summary>
	/// Static constructor.
	/// </summary>
	static BinaryStore()
	{
		BinaryStore<TMain, T>.initial = new();
#if NET8_0_OR_GREATER
		if (BinaryStore<TMain, T>.initial.SlotCount == 0)
		{
			BinaryStore<TMain, T>.slots = [];
			return;
		}
#endif
		BinaryStore<TMain, T>.slots = new BufferTypeMetadata<T>?[]?[BinaryStore<TMain, T>.initial.SlotCount];
	}

	/// <summary>
	/// Tries to add the current component
	/// </summary>
	/// <param name="component">The <see cref="BufferTypeMetadata{T}"/> instance to add.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="component"/> was added successfully; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean TryAdd(BufferTypeMetadata<T> component)
	{
		ref BufferTypeMetadata<T>? reference = ref BinaryStore<TMain, T>.GetBinaryReference(component.Size);
		return Interlocked.CompareExchange(ref reference, component, null) is null;
	}
	/// <summary>
	/// Retrieves a managed reference to the <see cref="BufferTypeMetadata{T}"/> instance for <paramref name="componentSize"/>.
	/// </summary>
	/// <param name="componentSize">Size of the requested metadata.</param>
	/// <returns>A managed <see cref="BufferTypeMetadata{T}"/> reference.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref BufferTypeMetadata<T>? GetBinaryReference(UInt16 componentSize)
	{
		Debug.Assert(componentSize > 0);
		if (componentSize <= BinaryStore<TMain, T>.initial.Length)
			return ref BinaryStore<TMain, T>.initial[componentSize - 1];

		Int32 targetSlot = BinaryStore<TMain, T>.GetSlotIndex(componentSize);
		Int32 pageLength = (BinaryStore<TMain, T>.initial.Length + 1) << targetSlot;
		ref BufferTypeMetadata<T>?[]? slot = ref BinaryStore<TMain, T>.slots[targetSlot];

		BufferTypeMetadata<T>?[]? page = Volatile.Read(ref slot);
		page ??= BinaryStore<TMain, T>.GetOrCreatePage(targetSlot);
		return ref page[componentSize - pageLength];
	}
	/// <summary>
	/// Retrieves the <see cref="BufferTypeMetadata{T}"/> instance for <paramref name="componentSize"/>.
	/// </summary>
	/// <param name="componentSize">Size of the requested metadata.</param>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static BufferTypeMetadata<T>? GetBinaryValue(UInt16 componentSize)
	{
		if (componentSize <= BinaryStore<TMain, T>.initial.Length)
			return BinaryStore<TMain, T>.initial[componentSize - 1];
		Int32 targetSlot = BinaryStore<TMain, T>.GetSlotIndex(componentSize);
		Int32 pageLength = (BinaryStore<TMain, T>.initial.Length + 1) << targetSlot;
		// ReSharper disable once InvertIf
		if (Volatile.Read(ref BinaryStore<TMain, T>.slots[targetSlot]) is not { } page)
		{
			if (!BuffersHelper.BufferAutoCompositionEnabled)
				return default;
			page = BinaryStore<TMain, T>.GetOrCreatePage(targetSlot);
		}
		return page[componentSize - pageLength];
	}
	/// <summary>
	/// Retrieves the fundamental component of size <paramref name="space"/>.
	/// </summary>
	/// <param name="storage">A <see cref="MetadataStorage"/> instance.</param>
	/// <param name="space">Size of fundamental component.</param>
	/// <returns>A <see cref="BufferTypeMetadata"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static BufferTypeMetadata<T>? GetFundamental(IMetadataStorage storage, UInt16 space)
	{
		if (BinaryStore<TMain, T>.GetBinaryValue(space) is { } metadata)
			return metadata;
		if (space == 1)
			return BinaryStore<TMain, T>.GetBinaryReference(space) = Atomic<T>.TypeMetadata; // Atomic missing.
		BufferTypeMetadata<T>? result = BinaryStore<TMain, T>.GetMaxBinarySpace((UInt16)(space / 2));
		while (result.Size < space)
		{
			result = result.Double(storage);
			if (result is null) break;
			BinaryStore<TMain, T>.GetBinaryReference(result.Size) = result;
		}
		return result;
	}
	/// <summary>
	/// Computes the binary metadata required for a buffer with <paramref name="count"/> items.
	/// </summary>
	/// <param name="storage">A <see cref="IMetadataStorage"/> instance.</param>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <param name="nonBinaryMinimal">Indicates the value fo the non-binary buffer minimal.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1199)]
#endif
	public static BufferTypeMetadata<T>? ComputeBinaryMetadata(IMetadataStorage storage, UInt16 count,
		Int32 nonBinaryMinimal)
	{
		if (BufferTypeMetadata.HasError(typeof(T), count))
			return nonBinaryMinimal > -1 ? BinaryStore<TMain, T>.GetMinimal(count, nonBinaryMinimal) : default;
		BufferTypeMetadata<T>? result = BinaryStore<TMain, T>.ComputeBinaryMetadata(storage, count);
		return result is null && nonBinaryMinimal > -1 ?
			BinaryStore<TMain, T>.GetMinimal(count, nonBinaryMinimal) :
			result;
	}

	/// <summary>
	/// Computes the binary metadata required for a buffer with <paramref name="count"/> items.
	/// </summary>
	/// <param name="storage">A <see cref="IMetadataStorage"/> instance.</param>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1199)]
#endif
	private static BufferTypeMetadata<T>? ComputeBinaryMetadata(IMetadataStorage storage, UInt16 count)
	{
		BufferTypeMetadata<T>? result = BinaryStore<TMain, T>.GetFundamental(storage, BuffersHelper.GetSpaceFor(count));
		// Fundamental metadata not found.
		if (result is null) return default;
		while (count - result.Size > 0)
		{
			UInt16 diff = (UInt16)(count - result.Size);
			BufferTypeMetadata<T>? aux = BinaryStore<TMain, T>.GetBinaryValue(diff) ??
				BinaryStore<TMain, T>.ComputeBinaryMetadata(storage, diff);
			{
				// Auxiliary metadata not found. Use minimal.
				if (aux is null)
					return default;
				result = result.Compose(storage, aux);
				if (result is null)
					// Unable to create composed metadata. Use minimal.
					return default;
				BinaryStore<TMain, T>.GetBinaryReference(result.Size) = result;
			}
		}
		return result;
	}

	/// <summary>
	/// Retrieves the nearest fundamental component to <paramref name="space"/> size.
	/// </summary>
	/// <param name="space">Size of fundamental component.</param>
	/// <returns>A <see cref="BufferTypeMetadata"/> instance.</returns>
	private static BufferTypeMetadata<T> GetMaxBinarySpace(UInt16 space)
	{
		ref BufferTypeMetadata<T>? result = ref BinaryStore<TMain, T>.GetBinaryReference(space);
		while (result is null)
		{
			if (space == 1)
			{
				result = Atomic<T>.TypeMetadata;
				break;
			}
			space /= 2;
			result = ref BinaryStore<TMain, T>.GetBinaryReference(space);
		}
		return result;
	}
	/// <summary>
	/// Retrieves the smallest registered binary buffer metadata whose capacity is greater than <paramref name="count"/>.
	/// </summary>
	/// <param name="count">
	/// Requested buffer capacity. Binary metadata for this exact capacity is assumed to be unavailable.
	/// </param>
	/// <param name="nonBinaryMinimal">
	/// Capacity of the smallest non-binary buffer previously found, or <c>0</c> when no non-binary candidate is available.
	/// When specified, only binary capacities smaller than this value are considered.
	/// </param>
	/// <returns>The smallest qualifying binary buffer metadata; otherwise, <see langword="null"/>.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3776)]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS907)]
#endif
	private static BufferTypeMetadata<T>? GetMinimal(UInt16 count, Int32 nonBinaryMinimal)
	{
		Debug.Assert(count > 1);

		if (count >= BinaryStore<TMain, T>.initial.Length && BinaryStore<TMain, T>.slots.Length == 0)
			return default;

		Int32 remaining = Math.Min(count - 1, UInt16.MaxValue - count);
		if (nonBinaryMinimal > 0)
		{
			if (nonBinaryMinimal <= count + 1)
				return default;
			remaining = Math.Min(remaining, nonBinaryMinimal - count - 1);
		}
		if (remaining <= 0) return default;

		ref BufferTypeMetadata<T>? r0 = ref BinaryStore<TMain, T>.initial[0];
		Int32 spanLength = BinaryStore<TMain, T>.initial.Length;
		Int32 pageIndex = -1;
		Int32 relativeIndex = count;
		UInt16 firstSize = (UInt16)(count + 1);
		if (firstSize > BinaryStore<TMain, T>.initial.Length)
		{
			pageIndex = BinaryStore<TMain, T>.GetSlotIndex(firstSize);
			r0 = ref BinaryStore<TMain, T>.GetPageReference(pageIndex, out spanLength);
			if (spanLength <= 0)
				// Page unavailable.
				return default;
			Int32 pageLength = (BinaryStore<TMain, T>.initial.Length + 1) << pageIndex;
			relativeIndex = firstSize - pageLength;
		}
		while (remaining > 0)
		{
			// Search at the current page.
			Int32 length = Math.Min(spanLength - relativeIndex, remaining);
			if (BinaryStore<TMain, T>.Search(ref r0, relativeIndex, length) is { } result)
				// Minimal metadata found.
				return result;
			// Exclude from total elements the current search length.
			if ((remaining -= length) <= 0) continue;
			// Get the next page.
			pageIndex++;
			r0 = ref BinaryStore<TMain, T>.GetPageReference(pageIndex, out spanLength);
			if (spanLength <= 0)
				// Next page unavailable.
				return default;
			relativeIndex = 0;
		}
		return default;
	}
	/// <summary>
	/// Attempts to retrieve a managed reference to the metadata storage page.
	/// </summary>
	/// <param name="pageIndex">Requested page index.</param>
	/// <param name="pageLength">
	/// Receives the length of the requested page when available; otherwise, <c>-1</c>.
	/// </param>
	/// <returns>A managed reference to the metadata storage page.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ref BufferTypeMetadata<T>? GetPageReference(Int32 pageIndex, out Int32 pageLength)
	{
		Debug.Assert(pageIndex >= 0);
		if ((UInt32)pageIndex >= (UInt32)BinaryStore<TMain, T>.slots.Length ||
		    BinaryStore<TMain, T>.slots[pageIndex] is not { } page)
		{
			pageLength = -1;
			return ref Unsafe.NullRef<BufferTypeMetadata<T>?>();
		}
		pageLength = page.Length;
		return ref MemoryMarshal.GetReference(page);
	}
	/// <summary>
	/// Searches for the first available metadata entry in a page segment.
	/// </summary>
	/// <param name="r0">Managed reference to metadata page.</param>
	/// <param name="start">Zero-based index of the first entry to inspect.</param>
	/// <param name="count">Number of entries to inspect.</param>
	/// <returns>
	/// The first available metadata entry within the specified range; otherwise, <see langword="null"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static unsafe BufferTypeMetadata<T>? Search(ref BufferTypeMetadata<T>? r0, Int32 start, Int32 count)
	{
		Debug.Assert(start >= 0);
		Debug.Assert(count > 0);
		ref BufferTypeMetadata<T>? rS = ref Unsafe.Add(ref r0, start);
#pragma warning disable CS8500
		fixed (void* ptr = &rS)
#pragma warning restore CS8500
		{
			ReadOnlySpan<IntPtr> unsafeSpan = new(ptr, count);
#if NET7_0_OR_GREATER
			Int32 index = unsafeSpan.IndexOfAnyExcept(IntPtr.Zero);
#else
			Int32 index = -1;
			for (Int32 i = 0; i < unsafeSpan.Length; i++)
			{
				IntPtr val = unsafeSpan[i];
				if (val == IntPtr.Zero) continue;
				index = i;
				break;
			}
#endif
			return index < 0 ? default : Unsafe.Add(ref rS, index);
		}
	}
	/// <summary>
	/// Atomically updates the cached capacity when <paramref name="candidate"/> is greater than the current value.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void UpdateSlotCapacity(Int32 candidate)
	{
		Int32 current = Volatile.Read(ref BinaryStore<TMain, T>.currentSlotCapacity);
		while (candidate > current)
		{
			Int32 observed =
				Interlocked.CompareExchange(ref BinaryStore<TMain, T>.currentSlotCapacity, candidate, current);

			if (observed == current)
				return;
			current = observed;
		}
	}
	/// <summary>
	/// Retrieves the slot index for <paramref name="componentSize"/>.
	/// </summary>
	/// <param name="componentSize">The requested value.</param>
	/// <returns>The slot index for <paramref name="componentSize"/>.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 GetSlotIndex(UInt16 componentSize)
	{
		Debug.Assert(componentSize > BinaryStore<TMain, T>.initial.Length);
		Int32 result = BinaryStore<TMain, T>.slots.Length - BuffersHelper.GetLeadingZeros(componentSize) - 1;
		Debug.Assert((UInt32)result < (UInt32)BinaryStore<TMain, T>.slots.Length);
		return result;
	}
	/// <summary>
	/// Retrieves or create the page for <paramref name="targetSlot"/>.
	/// </summary>
	/// <param name="targetSlot">Page number.</param>
	/// <returns>The <see cref="BufferTypeMetadata{T}"/> array for <paramref name="targetSlot"/>.</returns>
	private static BufferTypeMetadata<T>?[] GetOrCreatePage(Int32 targetSlot)
	{
		Int32 firstPageLength = BinaryStore<TMain, T>.initial.Length + 1;
		BufferTypeMetadata<T>?[]?[] slotsArray = BinaryStore<TMain, T>.slots;
		BufferTypeMetadata<T>?[]? result = default;
		for (Int32 i = 0, pageLength = firstPageLength; i <= targetSlot; i++, pageLength <<= 1)
		{
			ref BufferTypeMetadata<T>?[]? slot = ref slotsArray[i];
			result = Volatile.Read(ref slot);
			if (result is not null) continue;
			BufferTypeMetadata<T>?[] created = new BufferTypeMetadata<T>?[pageLength];
			result = Interlocked.CompareExchange(ref slot, created, null) ?? created;
			BinaryStore<TMain, T>.UpdateSlotCapacity(pageLength * 2 - firstPageLength);
		}
		Debug.Assert(result is not null);
		return result;
	}
#if !PACKAGE
	/// <summary>
	/// Initial span.
	/// </summary>
	public static ReadOnlySpan<BufferTypeMetadata<T>?> Initial => BinaryStore<TMain, T>.initial.Span;
	/// <summary>
	/// Slots.
	/// </summary>
	public static ReadOnlySpan<BufferTypeMetadata<T>?[]?> Slots => BinaryStore<TMain, T>.slots.AsSpan();
#endif
}