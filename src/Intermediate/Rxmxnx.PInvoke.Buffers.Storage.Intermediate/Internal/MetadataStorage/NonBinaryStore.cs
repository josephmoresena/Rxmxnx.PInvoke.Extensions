#if !NET9_0_OR_GREATER
using Lock = System.Object;
#endif

namespace Rxmxnx.PInvoke.Internal;

internal abstract partial class MetadataStorage<T>
{
	/// <summary>
	/// Static class for non-binary buffer types metadata.
	/// </summary>
	private static class NonBinaryStore
	{
		/// <summary>
		/// Lock object.
		/// </summary>
		// ReSharper disable once StaticMemberInGenericType
		private static Lock? lockObj;
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
#if NET9_0_OR_GREATER
			using (NonBinaryStore.GetLock().EnterScope())
#else
			lock (NonBinaryStore.GetLock())
#endif
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
		}
		/// <summary>
		/// Adds non-binary metadata to current cache.
		/// </summary>
		/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="typeMetadata"/> was successfully added; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		public static void AddNonBinary(BufferTypeMetadata<T> typeMetadata)
		{
#if NET9_0_OR_GREATER
			using (NonBinaryStore.GetLock().EnterScope())
#else
			lock (NonBinaryStore.GetLock())
#endif
				NonBinaryStore.GetNonBinaryMap().TryAdd(typeMetadata.Size, typeMetadata);
		}

		/// <summary>
		/// Retrieves the lock object to concurrent operations.
		/// </summary>
		/// <returns>A <see cref="Lock"/> instance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Lock GetLock() => NativeUtilities.GetConcurrentObject(ref NonBinaryStore.lockObj);
		/// <summary>
		/// Retrieves the non-binary map to concurrent operations.
		/// </summary>
		/// <returns>A <see cref="SortedDictionary{UInt16, BufferTypeMetadata}"/> instance.</returns>
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