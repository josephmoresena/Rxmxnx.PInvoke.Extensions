namespace Rxmxnx.PInvoke.Internal;

internal abstract partial class MetadataStorage<T>
{
	/// <summary>
	/// Static class for non-binary buffer types metadata.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2743)]
#endif
	private static class NonBinaryStore
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
		/// <param name="allowMinimal">Allow to return minimal buffer.</param>
		/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
		public static BufferTypeMetadata<T>? GetNonBinary(UInt16 count, Boolean allowMinimal)
		{
			if (!NonBinaryStore.HasNonBinaryMap()) return default;

			// Recuperamos de forma perezosa y segura la instancia del bloqueo
			ReaderWriterLockSlim lockInstance = NonBinaryStore.GetLock();

			lockInstance.EnterReadLock();
			try
			{
				SortedList<UInt16, BufferTypeMetadata<T>> map = NonBinaryStore.GetNonBinaryMap();
				if (map.TryGetValue(count, out BufferTypeMetadata<T>? result))
					return result;
				if (!allowMinimal || map.Count == 0) return default;

				IList<UInt16> keys = map.Keys;
				Int32 lo = 0;
				Int32 hi = keys.Count - 1;

				while (lo <= hi)
				{
					Int32 mid = lo + ((hi - lo) >> 1);

					if (keys[mid] < count)
						lo = mid + 1;
					else
						hi = mid - 1;
				}
				if ((UInt32)lo >= (UInt32)keys.Count) return default;
				return keys[lo] <= (UInt32)count << 1 ? map.Values[lo] : default;
			}
			finally
			{
				lockInstance.ExitReadLock();
			}
		}

		/// <summary>
		/// Adds non-binary metadata to current cache.
		/// </summary>
		/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
		public static void AddNonBinary(BufferTypeMetadata<T> typeMetadata)
		{
			ReaderWriterLockSlim lockInstance = NonBinaryStore.GetLock();
			lockInstance.EnterWriteLock();
			try
			{
				NonBinaryStore.GetNonBinaryMap().TryAdd(typeMetadata.Size, typeMetadata);
			}
			finally
			{
				lockInstance.ExitWriteLock();
			}
		}

		/// <summary>
		/// Retrieves the reader-writer lock object for concurrent operations.
		/// </summary>
		/// <returns>A <see cref="ReaderWriterLockSlim"/> instance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static ReaderWriterLockSlim GetLock() => NativeUtilities.GetConcurrentObject(ref NonBinaryStore.rwLock);

		/// <summary>
		/// Retrieves the non-binary map for concurrent operations.
		/// </summary>
		/// <returns>A <see cref="SortedList{UInt16, BufferTypeMetadata}"/> instance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static SortedList<UInt16, BufferTypeMetadata<T>> GetNonBinaryMap()
			=> NativeUtilities.GetConcurrentObject(ref NonBinaryStore.nonBinaryMap);

		/// <summary>
		/// Indicates whether the current type has a non-binary map.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if current type has a non-binary map; otherwise, <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Boolean HasNonBinaryMap() => NonBinaryStore.nonBinaryMap is not null;
	}
}