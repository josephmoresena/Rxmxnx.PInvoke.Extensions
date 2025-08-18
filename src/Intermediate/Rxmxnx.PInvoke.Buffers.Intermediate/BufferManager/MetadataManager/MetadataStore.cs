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

#if NET7_0_OR_GREATER
			/// <see cref="MetadataStore.GetMetadataInfo"/>
			private MethodInfo? _getMetadataInfo;
#endif

			/// <summary>
			/// Lock object.
			/// </summary>
			public Object LockObject { get; } = new();
			/// <summary>
			/// Buffers dictionary.
			/// </summary>
			public IDictionary<UInt16, BufferTypeMetadata<T>> BinaryBuffers => this._binaryCache;
#if NET7_0_OR_GREATER
			/// <summary>
			/// <see cref="MethodInfo"/> to retrieve buffer metadata.
			/// </summary>
#if !PACKAGE
			[ExcludeFromCodeCoverage]
#endif
			public MethodInfo? GetMetadataInfo
			{
				get
				{
					try
					{
						if (this._getMetadataInfo is null && BufferManager.BufferAutoCompositionEnabled)
							this._getMetadataInfo = MetadataStore.ReflectGetMetadataMethod();
					}
					catch (Exception)
					{
						// ignored
					}
					return this._getMetadataInfo;
				}
			}
#endif
			/// <summary>
			/// Maximum binary space size.
			/// </summary>
			public UInt16 MaxSpace { get; set; } = 1;

			/// <summary>
			/// Constructor.
			/// </summary>
#if !PACKAGE
			[ExcludeFromCodeCoverage]
#endif
			public MetadataStore()
			{
#if NET7_0_OR_GREATER
				this._binaryCache.Add(1, IManagedBuffer<T>.GetMetadata<Atomic<T>>());
#else
				this._binaryCache.Add(1, Atomic<T>.TypeMetadata);
#endif
			}
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
#if !PACKAGE
			[ExcludeFromCodeCoverage]
#endif
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

#if NET7_0_OR_GREATER
			/// <summary>
			/// Retrieves the reflected <see cref="IManagedBuffer{T}.GetMetadata{TBuffer}()"/> method.
			/// </summary>
			/// <returns>A <see cref="MethodInfo"/> instance.</returns>
			private static MethodInfo ReflectGetMetadataMethod()
			{
				Type typeofT = typeof(IManagedBuffer<T>);
				return typeofT.GetMethod(nameof(IManagedBuffer<Object>.GetMetadata), BufferManager.GetMetadataFlags)!;
			}
#endif
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
		}
	}
}