namespace Rxmxnx.PInvoke.Buffers;

#pragma warning disable CA2252
public static partial class AllocatedBuffer
{
	internal static partial class MetadataCache<T>
	{
		/// <summary>
		/// Internal cache object.
		/// </summary>
		private sealed class Cache
		{
			/// <summary>
			/// Dictionary.
			/// </summary>
			private readonly SortedDictionary<UInt16, IBufferTypeMetadata<T>> _cache = new();
			/// <summary>
			/// Lock object.
			/// </summary>
			public Object LockObject { get; } = new();
			/// <summary>
			/// Buffers dictionary.
			/// </summary>
			public IDictionary<UInt16, IBufferTypeMetadata<T>> Buffers => this._cache;
			/// <summary>
			/// <see cref="MethodInfo"/> of buffer metadata.
			/// </summary>
			public MethodInfo? GetMetadataInfo { get; }
			/// <summary>
			/// Maximum binary space size.
			/// </summary>
			public UInt16 MaxSpace { get; set; } = 1;
			/// <summary>
			/// Constructor.
			/// </summary>
			public Cache()
			{
				this._cache.Add(1, IAllocatedBuffer<T>.GetMetadata<Primordial<T>>());
				try
				{
					if (!AllocatedBuffer.disabledReflection)
						this.GetMetadataInfo = Cache.ReflectGetMetadataMethod();
				}
				catch (Exception)
				{
					// ignored
				}
			}
			/// <summary>
			/// Adds metadata to current cache.
			/// </summary>
			/// <param name="metadata">A <see cref="IBufferTypeMetadata{T}"/> instance.</param>
			/// <returns>
			/// <see langword="true"/> if <paramref name="metadata"/> was successfully added; otherwise,
			/// <see langword="false"/>.
			/// </returns>
			public Boolean Add(IBufferTypeMetadata<T> metadata) => this._cache.TryAdd(metadata.Size, metadata);
			/// <summary>
			/// Retrieves the minimal buffer metadata registered to hold at least <paramref name="count"/> items.
			/// </summary>
			/// <param name="count">Minimal elements items in buffer.</param>
			/// <returns>A <see cref="IBufferTypeMetadata{T}"/> instance.</returns>
			public IBufferTypeMetadata<T>? GetMinimal(UInt16 count)
			{
				using SortedDictionary<UInt16, IBufferTypeMetadata<T>>.KeyCollection.Enumerator enumerator =
					this._cache.Keys.GetEnumerator();
				while (enumerator.MoveNext())
				{
					UInt16 current = enumerator.Current;
					if (current < count) continue;
					if (current >= 2 * count) break;
					return this._cache[current];
				}
				return default;
			}
			/// <summary>
			/// Retrieves the reflected <see cref="IAllocatedBuffer{T}.GetMetadata{TBuffer}()"/> method.
			/// </summary>
			/// <returns>A <see cref="MethodInfo"/> instance.</returns>
			private static MethodInfo ReflectGetMetadataMethod()
			{
				Type typeofT = typeof(IAllocatedBuffer<T>);
				return typeofT.GetMethod(AllocatedBuffer.getMetadataName, AllocatedBuffer.getMetadataFlags)!;
			}
		}
	}
}