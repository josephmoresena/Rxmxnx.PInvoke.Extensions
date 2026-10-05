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
#if !PACKAGE
	internal static class NonBinaryStore<T>
#else
	protected static class NonBinaryStore<T>
#endif
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
		/// <param name="count">The number of items in the required buffer.</param>
		/// <param name="minimal">Output. The smallest non-binary buffer.</param>
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
		/// Adds non-binary metadata to the current cache.
		/// </summary>
		/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
		public static void AddNonBinary(BufferTypeMetadata<T> typeMetadata)
		{
			Debug.Assert(!typeMetadata.IsBinary);
			using WriteScope scope = NonBinaryStore<T>.GetLock();
#if NETSTANDARD2_1 || NETCOREAPP2_0_OR_GREATER
			NonBinaryStore<T>.GetNonBinaryMap().TryAdd(typeMetadata.Size, typeMetadata);
#else
			SortedList<UInt16, BufferTypeMetadata<T>> maps = NonBinaryStore<T>.GetNonBinaryMap();
			if (maps.ContainsKey(typeMetadata.Size)) return;
			try
			{
				maps.Add(typeMetadata.Size, typeMetadata);
			}
			catch (Exception)
			{
				// NONE
			}
#endif
		}
#if NET8_0_OR_GREATER
		/// <summary>
		/// Tries to add <paramref name="typeMetadata"/> to the non-binary cache.
		/// </summary>
		/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="typeMetadata"/> was stored; otherwise, <see langword="false"/>.
		/// </returns>
		/// <remarks>
		/// A stored non-binary entry is replaced when <paramref name="typeMetadata"/> is binary.
		/// A stored binary entry is left unchanged.
		/// </remarks>
		public static Boolean TryAddBinary(BufferTypeMetadata<T> typeMetadata)
		{
			Debug.Assert(typeMetadata.IsBinary);
			using WriteScope scope = NonBinaryStore<T>.GetLock();
			SortedList<UInt16, BufferTypeMetadata<T>> map = NonBinaryStore<T>.GetNonBinaryMap();
#if NET8_0_OR_GREATER
			// ReSharper disable once InvertIf
			if (map.TryGetValue(typeMetadata.Size, out BufferTypeMetadata<T>? current))
			{
				if (current.IsBinary || !typeMetadata.IsBinary) return false;
				map[typeMetadata.Size] = typeMetadata;
				return true;
			}
#endif
#if NETSTANDARD2_1 || NETCOREAPP2_0_OR_GREATER
			return map.TryAdd(typeMetadata.Size, typeMetadata);
#else
			try
			{
				map.Add(typeMetadata.Size, typeMetadata);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
#endif
		}
		/// <summary>
		/// Retrieves the exact binary metadata for a buffer with <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">The number of items in the required buffer.</param>
		/// <returns>
		/// The exact binary metadata. A non-binary entry breaks composition and is reported as missing.
		/// </returns>
		public static BufferTypeMetadata<T>? GetExactBinary(UInt16 count)
		{
			if (!NonBinaryStore<T>.HasNonBinaryMap()) return default;

			using ReadScope scope = NonBinaryStore<T>.GetLock();
			SortedList<UInt16, BufferTypeMetadata<T>> map = NonBinaryStore<T>.GetNonBinaryMap();
			return map.TryGetValue(count, out BufferTypeMetadata<T>? result) && result.IsBinary ? result : default;
		}
#endif

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