namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Basic implementation of <see cref="IMetadataStorage"/> interface.
/// </summary>
internal abstract partial class MetadataStorage : IMetadataStorage
{
	/// <inheritdoc/>
	public abstract Boolean TryAdd<T>(BufferTypeMetadata<T> component);
	/// <inheritdoc/>
	public BufferTypeMetadata<T>? GetMetadata<T>(UInt16 count)
	{
		if (count == 0) count++; // Avoid Zero elements buffer.
		if (this.GetCurrentCapacity<T>() >= count && this.GetBinaryValue<T>(count) is { } binary)
			return binary;
		if (NonBinaryStore<T>.GetNonBinary(count, out BufferTypeMetadata<T>? minimalNonBinary) is { } nonBinary)
			// Exact non-binary buffer. Allow minimal at first only if unable to retrieve a binary buffer.
			return nonBinary;
#if NET8_0_OR_GREATER
		if (count > this.MaxStorageCapacity)
			// Binary capacity doesn't allow current count.
			return default;
#endif
		binary = this.ComputeBinaryMetadata<T>(count, true);
		//return binary is not null && binary.Size > count ? binary : default;
		return binary ?? minimalNonBinary; // Approximate non-Binary buffer.
	}
	/// <inheritdoc/>
	public void PrepareBinaryMetadata<T>(UInt16 count)
	{
		if (count == 0) count++;
		Type typeofT = typeof(T);
		BufferTypeMetadata<T>? metadata = default;
#if NET8_0_OR_GREATER
		ValidationUtilities.ThrowIfNullMetadata(typeofT, count, count > this.MaxStorageCapacity);
#endif
		if (this.GetBinaryValue<T>(count) is not null) return;
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
			metadata = this.ComputeBinaryMetadata<T>(composeSize, false);
			ValidationUtilities.ThrowIfNullMetadata(typeofT, composeSize, metadata is null);
		}
	}
	/// <inheritdoc/>
	public void RegisterBuffer<T, [DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer>()
		where TBuffer : struct, IManagedBuffer<T>
	{
#if NET7_0_OR_GREATER
		BufferTypeMetadata<T> typeMetadata = IManagedBuffer<T>.GetMetadata<TBuffer>();
#else
		BufferTypeMetadata<T> typeMetadata = BuffersHelper.GetMetadata<T, TBuffer>();
#endif
		if (!typeMetadata.IsBinary)
		{
			NonBinaryStore<T>.AddNonBinary(typeMetadata);
			return;
		}
		if (!this.TryAdd(typeMetadata)) return;
#if NET7_0_OR_GREATER
		TBuffer.AppendComponent(this);
#else
		typeMetadata.AppendComponent(this);
#endif
	}
	/// <inheritdoc/>
	[return: NotNullIfNotNull("typeMetadata")]
	public BufferTypeMetadata<T>? AddBinaryMetadata<T>(BufferTypeMetadata<T>? typeMetadata)
	{
		if (typeMetadata is null) return default;
		this.GetBinaryReference<T>(typeMetadata.Size) = typeMetadata;
		return typeMetadata;
	}
#if !PACKAGE
	/// <inheritdoc/>
	public void PrintMetadata<T>(Boolean trace)
	{
		if (!trace) return;
		Int32 count = 0;
		foreach (BufferTypeMetadata<T>? m in this.GetInitial<T>())
		{
			if (m is null) continue;
			// ReSharper disable once HeapView.BoxingAllocation
			Trace.WriteLine($"{typeof(T)} {m.Size}({String.Join(", ", m.Components.ToArray().Select(k => k.Size))})");
			count++;
		}
		foreach (BufferTypeMetadata<T>?[]? a in this.GetSlots<T>())
		foreach (BufferTypeMetadata<T>? m in a.AsSpan())
		{
			if (m is null) continue;
			// ReSharper disable once HeapView.BoxingAllocation
			Trace.WriteLine($"{typeof(T)} {m.Size}({String.Join(", ", m.Components.ToArray().Select(k => k.Size))})");
			count++;
		}
		// ReSharper disable once HeapView.BoxingAllocation
		Trace.WriteLine($"{typeof(T)}: {count}");
	}
#endif
}