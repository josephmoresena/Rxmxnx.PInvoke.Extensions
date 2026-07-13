namespace Rxmxnx.PInvoke.Internal;

internal abstract partial class MetadataStorage
{
	/// <summary>
	/// Static class for non-binary buffer types metadata.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2743)]
#endif
	protected static class NonBinaryStore<T>
	{
		/// <summary>
		/// Reader-Writer lock object initialized lazily to orchestrate concurrent access.
		/// </summary>
		// ReSharper disable once StaticMemberInGenericType
		private static ReaderWriterLockSlim? rwLock;
		/// <summary>
		/// Internal non-binary metadata list.
		/// </summary>
		private static SortedList<UInt16, BufferTypeMetadata<T>>? nonBinaryMap;

		/// <summary>
		/// Retrieves non-binary metadata required for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Amount of items in required buffer.</param>
		/// <param name="minimal">Output. Minimal non-binary buffer.</param>
		/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
		public static BufferTypeMetadata<T>? GetNonBinary(UInt16 count, out BufferTypeMetadata<T>? minimal)
		{
			minimal = default;
			if (!NonBinaryStore<T>.HasNonBinaryMap()) return default;

			using ReadScope scope = NonBinaryStore<T>.GetLock();
			SortedList<UInt16, BufferTypeMetadata<T>> map = NonBinaryStore<T>.GetNonBinaryMap();
			if (map.TryGetValue(count, out BufferTypeMetadata<T>? result))
				return result;
			IList<UInt16> keys = map.Keys;
			Int32 low = 0;
			Int32 high = keys.Count - 1;
			while (low <= high)
			{
				Int32 middle = low + ((high - low) >> 1);
				UInt16 size = keys[middle];

				if (size < count)
					low = middle + 1;
				else if (size > count)
					high = middle - 1;
				else
					return map.Values[middle];
			}
			if ((UInt32)low < (UInt32)keys.Count && keys[low] <= (UInt32)count << 1)
				minimal = map.Values[low];
			return default;
		}

		/// <summary>
		/// Adds non-binary metadata to current cache.
		/// </summary>
		/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
		public static void AddNonBinary(BufferTypeMetadata<T> typeMetadata)
		{
			using WriteScope scope = NonBinaryStore<T>.GetLock();
			NonBinaryStore<T>.GetNonBinaryMap().TryAdd(typeMetadata.Size, typeMetadata);
		}

		/// <summary>
		/// Retrieves the reader-writer lock object for concurrent operations.
		/// </summary>
		/// <returns>A <see cref="ReaderWriterLockSlim"/> instance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ReaderWriterLockSlim GetLock()
			=> NativeUtilities.GetConcurrentObject(ref NonBinaryStore<T>.rwLock);

		/// <summary>
		/// Retrieves the non-binary map for concurrent operations.
		/// </summary>
		/// <returns>A <see cref="SortedList{UInt16, BufferTypeMetadata}"/> instance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static SortedList<UInt16, BufferTypeMetadata<T>> GetNonBinaryMap()
			=> NativeUtilities.GetConcurrentObject(ref NonBinaryStore<T>.nonBinaryMap);

		/// <summary>
		/// Indicates whether the current type has a non-binary map.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if current type has a non-binary map; otherwise, <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Boolean HasNonBinaryMap() => Volatile.Read(ref NonBinaryStore<T>.nonBinaryMap) is not null;
	}
}