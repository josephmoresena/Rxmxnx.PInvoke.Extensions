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
#if !PACKAGE && (NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299)
	/// <inheritdoc/>
	public abstract void PrintMetadata<T>(Boolean trace);
#endif

#if NET8_0_OR_GREATER
	/// <summary>
	/// Tries to add <paramref name="typeMetadata"/> to the non-binary cache.
	/// </summary>
	/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	/// <returns>
	/// <see langword="true"/> if <paramref name="typeMetadata"/> was stored; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Boolean TryAddNonBinaryBinary<T>(BufferTypeMetadata<T> typeMetadata)
	{
		Debug.Assert(typeMetadata.IsBinary);
		return NonBinaryStore<T>.TryAddBinary(typeMetadata);
	}
	/// <summary>
	/// Retrieves the exact binary metadata stored for <paramref name="count"/> items.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="count">The number of items in the required buffer.</param>
	/// <returns>
	/// The exact binary metadata. A non-binary entry breaks composition and is reported as missing.
	/// </returns>
	internal static BufferTypeMetadata<T>? GetExactNonBinaryBinary<T>(UInt16 count)
		=> NonBinaryStore<T>.GetExactBinary(count);
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
#pragma warning disable CS0649, S3459
	private TBackend _backend;
#pragma warning restore CS0649, S3459

	/// <inheritdoc/>
	public override Boolean TryAdd<T>(BufferTypeMetadata<T> component)
	{
		Debug.Assert(component.Size > 0);
		Debug.Assert(component.IsBinary);
#if NET8_0_OR_GREATER
		// ReSharper disable once ConvertIfStatementToReturnStatement
		if (component.Size > this._backend.MaxStorageCapacity)
			return MetadataStorage.TryAddNonBinaryBinary(component);
#endif
		return this._backend.TryAdd(component);
	}
	/// <inheritdoc/>
	public override BufferTypeMetadata<T>? GetMetadata<T>(UInt16 count)
	{
		if (count == 0) count++; // Avoid a zero-element buffer.
#if NET8_0_OR_GREATER
		Boolean allowBinary = count <= this._backend.MaxStorageCapacity;
		if (allowBinary && this._backend.GetBinaryValue<T>(count) is { } binary)
#else
		if (this._backend.GetBinaryValue<T>(count) is { } binary)
#endif
			return binary;
		if (NonBinaryStore<T>.GetNonBinary(count, out BufferTypeMetadata<T>? minimalNonBinary) is { } nonBinary)
			// Exact non-binary buffer. Consider the minimal buffer only when a binary buffer cannot be retrieved.
			return nonBinary;
#if NET8_0_OR_GREATER
		if (!allowBinary)
			// The binary capacity does not allow the current count.
			return minimalNonBinary;
#endif
		binary = this._backend.ComputeBinaryMetadata<T>(this, count, minimalNonBinary?.Size ?? 0); // Allow minimal
		return binary ?? minimalNonBinary; // Approximate non-binary buffer.
	}
	/// <inheritdoc/>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	public override void PrepareBinaryMetadata<T>(UInt16 count)
	{
		if (count == 0) count++;
		Type typeofT = typeof(T);
		BufferTypeMetadata<T>? metadata = default;
#if NET8_0_OR_GREATER
		if (count > this._backend.MaxStorageCapacity)
		{
			if (MetadataStorage.GetExactNonBinaryBinary<T>(count) is not null) return;
		}
		else
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
			metadata = this._backend.ComputeBinaryMetadata<T>(this, composeSize, -1);
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
		Debug.Assert(typeMetadata.Size > 0);
#if NET8_0_OR_GREATER
		// ReSharper disable once InvertIf
		if (typeMetadata.Size > this._backend.MaxStorageCapacity)
		{
			MetadataStorage.TryAddNonBinaryBinary(typeMetadata);
			return typeMetadata;
		}
#endif
		return this._backend.SetBinaryValue(typeMetadata);
	}
#if !PACKAGE
	/// <inheritdoc/>
	[ExcludeFromCodeCoverage]
	public override void PrintMetadata<T>(Boolean trace)
	{
		if (!trace) return;
		Int32 count = 0;
#pragma warning disable S6670
		foreach (BufferTypeMetadata<T>? m in this._backend.GetInitial<T>())
		{
			if (m is null) continue;
			// ReSharper disable once HeapView.BoxingAllocation
			Trace.WriteLine($"{typeof(T)} {m.Size}({String.Join(", ", m.Components.ToArray().Select(k => k.Size))})");
			count++;
		}
		foreach (BufferTypeMetadata<T>?[]? a in this._backend.GetSlots<T>())
		{
			foreach (BufferTypeMetadata<T>? m in a.AsSpan())
			{
				if (m is null) continue;
				// ReSharper disable once HeapView.BoxingAllocation
				Trace.WriteLine(
					$"{typeof(T)} {m.Size}({String.Join(", ", m.Components.ToArray().Select(k => k.Size))})");
				count++;
			}
		}
		// ReSharper disable once HeapView.BoxingAllocation
		Trace.WriteLine($"{typeof(T)}: {count}");
#pragma warning restore S6670
	}
#endif
}