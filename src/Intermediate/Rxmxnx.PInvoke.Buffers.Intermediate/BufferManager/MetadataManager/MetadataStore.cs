namespace Rxmxnx.PInvoke;

public static partial class BufferManager
{
	internal static partial class MetadataManager<T>
	{
		/// <summary>
		/// Class to store metadata buffer types.
		/// </summary>
		private sealed class MetadataStore
		{
			/// <summary>
			/// Internal binary metadata dictionary.
			/// </summary>
			private readonly SortedDictionary<UInt16, BufferTypeMetadata<T>> _binaryCache = new();
			/// <summary>
			/// Internal non-binary metadata dictionary.
			/// </summary>
			private readonly SortedDictionary<UInt16, BufferTypeMetadata<T>> _nonBinaryCache = new();
			/// <summary>
			/// Lock object.
			/// </summary>
			public Object LockObject { get; } = new();
			/// <summary>
			/// Buffers dictionary.
			/// </summary>
			public IDictionary<UInt16, BufferTypeMetadata<T>> BinaryBuffers => this._binaryCache;
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
#pragma warning disable CA2252
			public MetadataStore()
			{
				this._binaryCache.Add(1, IManagedBuffer<T>.GetMetadata<Atomic<T>>());
				try
				{
					if (BufferManager.BufferAutoCompositionEnabled)
						this.GetMetadataInfo = MetadataStore.ReflectGetMetadataMethod();
				}
				catch (Exception)
				{
					// ignored
				}
			}
#pragma warning restore CA2252
			/// <summary>
			/// Adds metadata to current cache.
			/// </summary>
			/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
			/// <returns>
			/// <see langword="true"/> if <paramref name="typeMetadata"/> was successfully added; otherwise,
			/// <see langword="false"/>.
			/// </returns>
			public Boolean Add(BufferTypeMetadata<T> typeMetadata)
			{
				SortedDictionary<UInt16, BufferTypeMetadata<T>> buffers =
					typeMetadata.IsBinary ? this._binaryCache : this._nonBinaryCache;
				return buffers.TryAdd(typeMetadata.Size, typeMetadata);
			}
			/// <summary>
			/// Retrieves the minimal buffer metadata registered to hold at least <paramref name="count"/> items.
			/// </summary>
			/// <param name="count">Minimal number of items in buffer.</param>
			/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
			public BufferTypeMetadata<T>? GetMinimal(UInt16 count)
				=> MetadataStore.GetMinimal(this._nonBinaryCache, count) ??
					MetadataStore.GetMinimal(this._binaryCache, count);
			/// <summary>
			/// Retrieves the buffer metadata registered to hold exact <paramref name="count"/> items.
			/// </summary>
			/// <param name="count">Number of items in buffer.</param>
			/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
			public BufferTypeMetadata<T>? GetNonBinaryBuffer(UInt16 count)
				=> this._nonBinaryCache.GetValueOrDefault(count);

			/// <summary>
			/// Retrieves the reflected <see cref="IManagedBuffer{T}.GetMetadata{TBuffer}()"/> method.
			/// </summary>
			/// <returns>A <see cref="MethodInfo"/> instance.</returns>
			private static MethodInfo ReflectGetMetadataMethod()
			{
				Type typeofT = typeof(IManagedBuffer<T>);
				return typeofT.GetMethod(BufferManager.getMetadataName, BufferManager.getMetadataFlags)!;
			}
			/// <summary>
			/// Retrieves the minimal buffer metadata registered to hold at least <paramref name="count"/> items.
			/// </summary>
			/// <param name="cache">Buffer type metadata cache.</param>
			/// <param name="count">Minimal elements items in buffer.</param>
			/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
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
		}
	}
}