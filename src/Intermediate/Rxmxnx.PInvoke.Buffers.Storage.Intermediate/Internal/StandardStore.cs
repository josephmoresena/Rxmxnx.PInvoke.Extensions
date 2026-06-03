namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Implementation of <see cref="IMetadataStore"/>
/// </summary>
internal sealed partial class StandardStore : IMetadataStore
{
	/// <summary>
	/// Singleton instance.
	/// </summary>
	public static readonly IMetadataStore Instance = new StandardStore();

	Boolean IMetadataStore.TryAdd<T>(BufferTypeMetadata<T> component) => Generic<T>.Add(component);
	BufferTypeMetadata<T>? IMetadataStore.GetMetadata<T>(UInt16 count)
	{
#if NET9_0_OR_GREATER
		using (Generic<T>.GetLock().EnterScope())
#else
		lock (Generic<T>.GetLock())
#endif
		{
			BufferTypeMetadata<T>? nonBinary = Generic<T>.GetNonBinaryBuffer(count);
			if (nonBinary is not null) return nonBinary;
		}
		return this.GetBinaryMetadata<T>(count, true);
	}
#if NET5_0_OR_GREATER
	[SkipLocalsInit]
#endif
	void IMetadataStore.PrepareBinaryMetadata<T>(UInt16 count)
	{
		Type typeofT = typeof(T);
		BufferTypeMetadata<T>? metadata = default;
		Span<UInt16> components = BuffersHelper.GetBinaryComponents(stackalloc UInt16[16], count);
		foreach (UInt16 comp in components)
		{
			BufferTypeMetadata<T>? compMetadata = this.GetFundamental<T>(comp);
			ValidationUtilities.ThrowIfNullMetadata(typeofT, comp, compMetadata is null);
			if (metadata is null)
			{
				metadata = compMetadata;
				continue;
			}

			UInt16 composeSize = (UInt16)(comp + metadata.Size);
			metadata = this.GetBinaryMetadata<T>(composeSize, false);
			ValidationUtilities.ThrowIfNullMetadata(typeofT, composeSize, metadata is null);
		}
	}
	void IMetadataStore.RegisterBuffer<T,
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer>()
	{
#if NET7_0_OR_GREATER
		BufferTypeMetadata<T> typeMetadata = IManagedBuffer<T>.GetMetadata<TBuffer>();
#else
		BufferTypeMetadata<T> typeMetadata = BuffersHelper.GetMetadata<T, TBuffer>();
#endif
#if NET9_0_OR_GREATER
		using (Generic<T>.GetLock().EnterScope())
#else
		lock (Generic<T>.GetLock())
#endif
		{
			if (!Generic<T>.Add(typeMetadata) || !typeMetadata.IsBinary) return;
			while (BuffersHelper.GetMaxValue(Generic<T>.MaxSpace) < typeMetadata.Size)
				Generic<T>.MaxSpace *= 2;
#if NET7_0_OR_GREATER
			TBuffer.AppendComponent(this);
#else
			typeMetadata.AppendComponent(this);
#endif
		}
	}
	[return: NotNullIfNotNull(nameof(typeMetadata))]
	BufferTypeMetadata<T>? IMetadataStore.AddBinaryMetadata<T>(BufferTypeMetadata<T>? typeMetadata)
	{
		if (typeMetadata is null || !Generic<T>.Add(typeMetadata)) return typeMetadata; // Is null or already exists.
		while (BuffersHelper.GetMaxValue(Generic<T>.MaxSpace) < typeMetadata.Size)
			Generic<T>.MaxSpace *= 2;
		return typeMetadata;
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6670)]
	[SuppressMessage("ReSharper", "HeapView.BoxingAllocation")]
	void IMetadataStore.PrintMetadata<T>(Boolean trace)
	{
		if (!trace) return;
#if NET9_0_OR_GREATER
		using (Generic<T>.GetLock().EnterScope())
#else
		lock (Generic<T>.GetLock())
#endif
		{
			foreach (UInt16 key in Generic<T>.BinaryKeys)
			{
				BufferTypeMetadata<T> m = Generic<T>.GetBinaryBuffer(key);
				Trace.WriteLine($"{typeof(T)} {key}({String.Join(", ", m.Components.ToArray().Select(k => k.Size))}).");
			}
			Trace.WriteLine($"{typeof(T)}: {Generic<T>.BinaryCount}");
		}
	}
#endif
}