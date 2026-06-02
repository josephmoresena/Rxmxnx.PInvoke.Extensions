#if !NET9_0_OR_GREATER
using Lock = System.Object;
#endif

namespace Rxmxnx.PInvoke.Internal;

internal sealed partial class StandardStore
{
	/// <summary>
	/// Generic storage
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2743)]
#endif
	private static class Generic<T>
	{
		/// <summary>
		/// Lock object.
		/// </summary>
		// ReSharper disable once StaticMemberInGenericType
		private static readonly Lock lockObj = new();
		/// <summary>
		/// Internal non-binary metadata dictionary.
		/// </summary>
		private static readonly SortedDictionary<UInt16, BufferTypeMetadata<T>> nonBinaryMap = new();
		/// <summary>
		/// Internal binary metadata dictionary.
		/// </summary>
		private static readonly SortedDictionary<UInt16, BufferTypeMetadata<T>> binaryMap = new();

		/// <summary>
		/// Maximum binary space size.
		/// </summary>
		// ReSharper disable once StaticMemberInGenericType
		public static UInt16 MaxSpace { get; set; } = 1;

		/// <summary>
		/// Constructor.
		/// </summary>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		static Generic()
		{
#if NET7_0_OR_GREATER
			Generic<T>.binaryMap.Add(1, IManagedBuffer<T>.GetMetadata<Atomic<T>>());
#else
			Generic<T>.binaryMap.Add(1, Atomic<T>.TypeMetadata);
#endif
		}

		/// <summary>
		/// Retrieves the lock object to concurrent operations.
		/// </summary>
		/// <returns>A <see cref="Lock"/> instance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Lock GetLock() => Generic<T>.lockObj;
		/// <summary>
		/// Adds metadata to current cache.
		/// </summary>
		/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="typeMetadata"/> was successfully added; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		public static Boolean Add(BufferTypeMetadata<T> typeMetadata)
		{
			SortedDictionary<UInt16, BufferTypeMetadata<T>> buffers =
				typeMetadata.IsBinary ? Generic<T>.binaryMap : Generic<T>.nonBinaryMap;
			return buffers.TryAdd(typeMetadata.Size, typeMetadata);
		}
		/// <summary>
		/// Retrieves the buffer metadata registered to hold exact <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Number of items in buffer.</param>
		/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BufferTypeMetadata<T>? GetNonBinaryBuffer(UInt16 count)
			=> Generic<T>.nonBinaryMap.GetValueOrDefault(count);
		/// <summary>
		/// Retrieves the buffer metadata registered to hold exact <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Number of items in buffer.</param>
		/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static BufferTypeMetadata<T> GetBinaryBuffer(UInt16 count) => Generic<T>.binaryMap[count];
		/// <summary>
		/// Retrieves the minimal buffer metadata registered to hold at least <paramref name="count"/> items.
		/// </summary>
		/// <param name="count">Minimal number of items in buffer.</param>
		/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public static BufferTypeMetadata<T>? GetMinimal(UInt16 count)
			=> Generic<T>.GetMinimal(Generic<T>.nonBinaryMap, count) ??
				Generic<T>.GetMinimal(Generic<T>.binaryMap, count);
		/// <summary>
		/// Retrieves the binary buffer type metadata associated with the count.
		/// </summary>
		/// <param name="count">The size of the binary buffer type metadata.</param>
		/// <param name="bufferTypeMetadata">Output. The binary buffer type metadata found.</param>
		/// <returns>
		/// <see langword="true"/> if the current instance contains an element with the specified size; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Boolean TryGetBinary(UInt16 count,
			[NotNullWhen(true)] out BufferTypeMetadata<T>? bufferTypeMetadata)
			=> Generic<T>.binaryMap.TryGetValue(count, out bufferTypeMetadata);

		/// <summary>
		/// Retrieves the minimal buffer metadata registered to hold at least <paramref name="count"/> items.
		/// </summary>
		/// <param name="cache">Buffer type metadata cache.</param>
		/// <param name="count">Minimal elements items in buffer.</param>
		/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		private static BufferTypeMetadata<T>? GetMinimal(SortedDictionary<UInt16, BufferTypeMetadata<T>> cache,
			UInt16 count)
		{
			using SortedDictionary<UInt16, BufferTypeMetadata<T>>.KeyCollection.Enumerator enumerator =
				cache.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				UInt16 current = enumerator.Current;
				if (current < count) continue;
				if (current >= 2 * count) break;
				return cache[current];
			}
			return default;
		}
#if !PACKAGE
		/// <summary>
		/// Binary keys.
		/// </summary>
		public static SortedDictionary<UInt16, BufferTypeMetadata<T>>.KeyCollection BinaryKeys
			=> Generic<T>.binaryMap.Keys;
		/// <summary>
		/// Binary count.
		/// </summary>
		public static Int32 BinaryCount => Generic<T>.binaryMap.Count;
#endif
	}
}