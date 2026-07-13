// ReSharper disable ConvertIfStatementToReturnStatement

#if NET8_0_OR_GREATER
using Rxmxnx.PInvoke.Buffers.Storage.Bootstrap;

namespace Rxmxnx.PInvoke.Buffers.Storage;

/// <summary>
/// Metadata storage backend with 2^5-1 capacity.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct BootstrapBackend31 : IMetadataStorageBackend
{
	/// <inheritdoc/>
	public Int32 MaxStorageCapacity
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => 31;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Boolean TryAdd<T>(BufferTypeMetadata<T> component) => BinaryStore<G31<T>, T>.TryAdd(component);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Int32 GetCurrentCapacity<T>() => BinaryStore<G31<T>, T>.CurrentCapacity;
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref BufferTypeMetadata<T>? GetBinaryReference<T>(UInt16 componentSize)
		=> ref BinaryStore<G31<T>, T>.GetBinaryReference(componentSize);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? GetBinaryValue<T>(UInt16 componentSize)
		=> BinaryStore<G31<T>, T>.GetBinaryValue(componentSize);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? ComputeBinaryMetadata<T>(MetadataStorage storage, UInt16 count,
		Int32 nonBinaryMinimal)
		=> BinaryStore<G31<T>, T>.ComputeBinaryMetadata(storage, count, nonBinaryMinimal);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? GetFundamental<T>(MetadataStorage storage, UInt16 space)
		=> BinaryStore<G31<T>, T>.GetFundamental(storage, space);
#if !PACKAGE
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<BufferTypeMetadata<T>?> GetInitial<T>() => BinaryStore<G31<T>, T>.Initial;
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<BufferTypeMetadata<T>?[]?> GetSlots<T>() => BinaryStore<G31<T>, T>.Slots;
#endif
}

/// <summary>
/// Metadata storage backend with 2^7-1 capacity.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct BootstrapBackend127 : IMetadataStorageBackend
{
	/// <inheritdoc/>
	public Int32 MaxStorageCapacity
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => 127;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Boolean TryAdd<T>(BufferTypeMetadata<T> component) => BinaryStore<G127<T>, T>.TryAdd(component);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Int32 GetCurrentCapacity<T>() => BinaryStore<G127<T>, T>.CurrentCapacity;
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref BufferTypeMetadata<T>? GetBinaryReference<T>(UInt16 componentSize)
		=> ref BinaryStore<G127<T>, T>.GetBinaryReference(componentSize);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? GetBinaryValue<T>(UInt16 componentSize)
		=> BinaryStore<G127<T>, T>.GetBinaryValue(componentSize);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? ComputeBinaryMetadata<T>(MetadataStorage storage, UInt16 count,
		Int32 nonBinaryMinimal)
		=> BinaryStore<G127<T>, T>.ComputeBinaryMetadata(storage, count, nonBinaryMinimal);
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? GetFundamental<T>(MetadataStorage storage, UInt16 space)
		=> BinaryStore<G127<T>, T>.GetFundamental(storage, space);
#if !PACKAGE
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<BufferTypeMetadata<T>?> GetInitial<T>() => BinaryStore<G127<T>, T>.Initial;
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<BufferTypeMetadata<T>?[]?> GetSlots<T>() => BinaryStore<G127<T>, T>.Slots;
#endif
}

/// <summary>
/// Bootstrap storage backend.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct BootstrapBackend<TSpace> : IMetadataStorageBackend where TSpace : struct, IBinarySpace
{
	/// <inheritdoc/>
	public Int32 MaxStorageCapacity
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => (1 << TSpace.Dimension) - 1;
	}

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Boolean TryAdd<T>(BufferTypeMetadata<T> component)
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.TryAdd(component);
		return BinaryStore<G2047<TSpace, T>, T>.TryAdd(component);
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Int32 GetCurrentCapacity<T>()
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.CurrentCapacity;
		return BinaryStore<G2047<TSpace, T>, T>.CurrentCapacity;
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ref BufferTypeMetadata<T>? GetBinaryReference<T>(UInt16 componentSize)
	{
		if (typeof(T).IsValueType)
			return ref BinaryStore<G255<TSpace, T>, T>.GetBinaryReference(componentSize);
		return ref BinaryStore<G2047<TSpace, T>, T>.GetBinaryReference(componentSize);
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? GetBinaryValue<T>(UInt16 componentSize)
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.GetBinaryValue(componentSize);
		return BinaryStore<G2047<TSpace, T>, T>.GetBinaryValue(componentSize);
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? ComputeBinaryMetadata<T>(MetadataStorage storage, UInt16 count,
		Int32 nonBinaryMinimal)
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.ComputeBinaryMetadata(storage, count, nonBinaryMinimal);
		return BinaryStore<G2047<TSpace, T>, T>.ComputeBinaryMetadata(storage, count, nonBinaryMinimal);
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public BufferTypeMetadata<T>? GetFundamental<T>(MetadataStorage storage, UInt16 space)
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.GetFundamental(storage, space);
		return BinaryStore<G2047<TSpace, T>, T>.GetFundamental(storage, space);
	}
#if !PACKAGE
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<BufferTypeMetadata<T>?> GetInitial<T>()
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.Initial;
		return BinaryStore<G2047<TSpace, T>, T>.Initial;
	}
	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ReadOnlySpan<BufferTypeMetadata<T>?[]?> GetSlots<T>()
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.Slots;
		return BinaryStore<G2047<TSpace, T>, T>.Slots;
	}
#endif
}
#endif