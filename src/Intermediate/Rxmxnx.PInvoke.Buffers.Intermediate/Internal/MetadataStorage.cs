namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Base class for built-in implementations of <see cref="IMetadataStorage"/> interface.
/// </summary>
internal abstract partial class MetadataStorage : IMetadataStorage
{
	/// <inheritdoc/>
	public abstract Boolean TryAdd<T>(BufferTypeMetadata<T> component);
	/// <inheritdoc/>
	public abstract BufferTypeMetadata<T>? GetMetadata<T>(UInt16 count);
	/// <inheritdoc/>
	public abstract void PrepareBinaryMetadata<T>(UInt16 count);
	/// <inheritdoc/>
	public abstract void RegisterBuffer<T,
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer>()
		where TBuffer : struct, IManagedBuffer<T>;
	/// <inheritdoc/>
	[return: NotNullIfNotNull("typeMetadata")]
	public abstract BufferTypeMetadata<T>? AddBinaryMetadata<T>(BufferTypeMetadata<T>? typeMetadata);
#if !PACKAGE
	/// <inheritdoc/>
	public abstract void PrintMetadata<T>(Boolean trace);
#endif
}

/// <summary>
/// Implementation <see cref="MetadataStorage"/> interface.
/// </summary>
/// <typeparam name="TBackend">Type of <see cref="IMetadataStorageBackend"/>.</typeparam>
internal sealed class MetadataStorage<TBackend> : MetadataStorage where TBackend : struct, IMetadataStorageBackend
{
	/// <summary>
	/// Default-initialized stateless backend.
	/// </summary>
	/// <remarks>
	/// Cannot be readonly because constrained interface calls on generic value types are treated as potentially mutating.
	/// </remarks>
#pragma warning disable CS0649
	private TBackend _backend;
#pragma warning restore CS0649

	/// <inheritdoc/>
	public override Boolean TryAdd<T>(BufferTypeMetadata<T> component) => this._backend.TryAdd(component);
	/// <inheritdoc/>
	public override BufferTypeMetadata<T>? GetMetadata<T>(UInt16 count)
	{
		if (count == 0) count++; // Avoid Zero elements buffer.
		if (this._backend.GetCurrentCapacity<T>() >= count && this._backend.GetBinaryValue<T>(count) is { } binary)
			return binary;
		if (NonBinaryStore<T>.GetNonBinary(count, out BufferTypeMetadata<T>? minimalNonBinary) is { } nonBinary)
			// Exact non-binary buffer. Allow minimal at first only if unable to retrieve a binary buffer.
			return nonBinary;
#if NET8_0_OR_GREATER
		if (count > this._backend.MaxStorageCapacity)
			// Binary capacity doesn't allow current count.
			return default;
#endif
		binary = this._backend.ComputeBinaryMetadata<T>(this, count, true);
		//return binary is not null && binary.Size > count ? binary : default;
		return binary ?? minimalNonBinary; // Approximate non-Binary buffer.
	}
	/// <inheritdoc/>
	public override void PrepareBinaryMetadata<T>(UInt16 count)
	{
		if (count == 0) count++;
		Type typeofT = typeof(T);
		BufferTypeMetadata<T>? metadata = default;
#if NET8_0_OR_GREATER
		ValidationUtilities.ThrowIfNullMetadata(typeofT, count, count > this._backend.MaxStorageCapacity);
#endif
		if (this._backend.GetBinaryValue<T>(count) is not null) return;
		Span<UInt16> components = BuffersHelper.GetBinaryComponents(stackalloc UInt16[16], count);
		foreach (UInt16 comp in components)
		{
			BufferTypeMetadata<T>? compMetadata = this._backend.GetFundamental<T>(this, comp);
			ValidationUtilities.ThrowIfNullMetadata(typeofT, comp, compMetadata is null);
			if (metadata is null)
			{
				metadata = compMetadata;
				continue;
			}

			UInt16 composeSize = (UInt16)(comp + metadata.Size);
			metadata = this._backend.ComputeBinaryMetadata<T>(this, composeSize, false);
			ValidationUtilities.ThrowIfNullMetadata(typeofT, composeSize, metadata is null);
		}
	}
	/// <inheritdoc/>
	public override void RegisterBuffer<T,
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBuffer>()
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
	public override BufferTypeMetadata<T>? AddBinaryMetadata<T>(BufferTypeMetadata<T>? typeMetadata)
	{
		if (typeMetadata is null) return default;
		this._backend.GetBinaryReference<T>(typeMetadata.Size) = typeMetadata;
		return typeMetadata;
	}
#if !PACKAGE
	/// <inheritdoc/>
	public override void PrintMetadata<T>(Boolean trace)
	{
		if (!trace) return;
		Int32 count = 0;
		foreach (BufferTypeMetadata<T>? m in this._backend.GetInitial<T>())
		{
			if (m is null) continue;
			// ReSharper disable once HeapView.BoxingAllocation
			Trace.WriteLine($"{typeof(T)} {m.Size}({String.Join(", ", m.Components.ToArray().Select(k => k.Size))})");
			count++;
		}
		foreach (BufferTypeMetadata<T>?[]? a in this._backend.GetSlots<T>())
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