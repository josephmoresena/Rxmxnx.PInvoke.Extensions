// ReSharper disable ConvertIfStatementToReturnStatement

#if NET8_0_OR_GREATER
using Rxmxnx.PInvoke.Buffers.Storage.Bootstrap;

namespace Rxmxnx.PInvoke.Buffers.Storage;

/// <summary>
/// Metadata storage with 2^5-1 capacity.
/// </summary>
internal sealed class BootstrapStorage31 : MetadataStorage
{
	/// <inheritdoc/>
	protected override Int32 MaxStorageCapacity => 5;

	/// <inheritdoc/>
	public override Boolean TryAdd<T>(BufferTypeMetadata<T> component) => BinaryStore<G31<T>, T>.TryAdd(component);
	/// <inheritdoc/>
	protected override Int32 GetCurrentCapacity<T>() => BinaryStore<G31<T>, T>.CurrentCapacity;
	/// <inheritdoc/>
	protected override ref BufferTypeMetadata<T>? GetBinaryReference<T>(UInt16 componentSize)
		=> ref BinaryStore<G31<T>, T>.GetBinaryReference(componentSize);
	/// <inheritdoc/>
	protected override BufferTypeMetadata<T>? GetBinaryValue<T>(UInt16 componentSize)
		=> BinaryStore<G31<T>, T>.GetBinaryValue(componentSize);
	/// <inheritdoc/>
	protected override BufferTypeMetadata<T>? ComputeBinaryMetadata<T>(UInt16 count, Boolean allowMinimal)
		=> BinaryStore<G31<T>, T>.ComputeBinaryMetadata(this, count, allowMinimal);
	/// <inheritdoc/>
	protected override BufferTypeMetadata<T>? GetFundamental<T>(UInt16 space)
		=> BinaryStore<G31<T>, T>.GetFundamental(this, space);
#if !PACKAGE
	/// <inheritdoc/>
	protected override ReadOnlySpan<BufferTypeMetadata<T>?> GetInitial<T>() => BinaryStore<G31<T>, T>.Initial;
	/// <inheritdoc/>
	protected override ReadOnlySpan<BufferTypeMetadata<T>?[]?> GetSlots<T>() => BinaryStore<G31<T>, T>.Slots;
#endif
}

/// <summary>
/// Metadata storage with 2^7-1 capacity.
/// </summary>
internal sealed class BootstrapStorage127 : MetadataStorage
{
	/// <inheritdoc/>
	protected override Int32 MaxStorageCapacity => 7;

	/// <inheritdoc/>
	public override Boolean TryAdd<T>(BufferTypeMetadata<T> component) => BinaryStore<G127<T>, T>.TryAdd(component);
	/// <inheritdoc/>
	protected override Int32 GetCurrentCapacity<T>() => BinaryStore<G127<T>, T>.CurrentCapacity;
	/// <inheritdoc/>
	protected override ref BufferTypeMetadata<T>? GetBinaryReference<T>(UInt16 componentSize)
		=> ref BinaryStore<G127<T>, T>.GetBinaryReference(componentSize);
	/// <inheritdoc/>
	protected override BufferTypeMetadata<T>? GetBinaryValue<T>(UInt16 componentSize)
		=> BinaryStore<G127<T>, T>.GetBinaryValue(componentSize);
	/// <inheritdoc/>
	protected override BufferTypeMetadata<T>? ComputeBinaryMetadata<T>(UInt16 count, Boolean allowMinimal)
		=> BinaryStore<G127<T>, T>.ComputeBinaryMetadata(this, count, allowMinimal);
	/// <inheritdoc/>
	protected override BufferTypeMetadata<T>? GetFundamental<T>(UInt16 space)
		=> BinaryStore<G127<T>, T>.GetFundamental(this, space);
#if !PACKAGE
	/// <inheritdoc/>
	protected override ReadOnlySpan<BufferTypeMetadata<T>?> GetInitial<T>() => BinaryStore<G127<T>, T>.Initial;
	/// <inheritdoc/>
	protected override ReadOnlySpan<BufferTypeMetadata<T>?[]?> GetSlots<T>() => BinaryStore<G127<T>, T>.Slots;
#endif
}

/// <summary>
/// Bootstrap storage.
/// </summary>
internal sealed class BootstrapStorage<TSpace> : MetadataStorage where TSpace : IBinarySpace
{
	/// <inheritdoc/>
	protected override Int32 MaxStorageCapacity => TSpace.Dimension;

	/// <inheritdoc/>
	public override Boolean TryAdd<T>(BufferTypeMetadata<T> component)
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.TryAdd(component);
		return BinaryStore<G2047<TSpace, T>, T>.TryAdd(component);
	}
	/// <inheritdoc/>
	protected override Int32 GetCurrentCapacity<T>()
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.CurrentCapacity;
		return BinaryStore<G2047<TSpace, T>, T>.CurrentCapacity;
	}
	/// <inheritdoc/>
	protected override ref BufferTypeMetadata<T>? GetBinaryReference<T>(UInt16 componentSize)
	{
		if (typeof(T).IsValueType)
			return ref BinaryStore<G255<TSpace, T>, T>.GetBinaryReference(componentSize);
		return ref BinaryStore<G2047<TSpace, T>, T>.GetBinaryReference(componentSize);
	}
	/// <inheritdoc/>
	protected override BufferTypeMetadata<T>? GetBinaryValue<T>(UInt16 componentSize)
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.GetBinaryValue(componentSize);
		return BinaryStore<G2047<TSpace, T>, T>.GetBinaryValue(componentSize);
	}
	/// <inheritdoc/>
	protected override BufferTypeMetadata<T>? ComputeBinaryMetadata<T>(UInt16 count, Boolean allowMinimal)
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.ComputeBinaryMetadata(this, count, allowMinimal);
		return BinaryStore<G2047<TSpace, T>, T>.ComputeBinaryMetadata(this, count, allowMinimal);
	}
	/// <inheritdoc/>
	protected override BufferTypeMetadata<T>? GetFundamental<T>(UInt16 space)
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.GetFundamental(this, space);
		return BinaryStore<G2047<TSpace, T>, T>.GetFundamental(this, space);
	}
#if !PACKAGE
	/// <inheritdoc/>
	protected override ReadOnlySpan<BufferTypeMetadata<T>?> GetInitial<T>()
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.Initial;
		return BinaryStore<G2047<TSpace, T>, T>.Initial;
	}
	/// <inheritdoc/>
	protected override ReadOnlySpan<BufferTypeMetadata<T>?[]?> GetSlots<T>()
	{
		if (typeof(T).IsValueType)
			return BinaryStore<G255<TSpace, T>, T>.Slots;
		return BinaryStore<G2047<TSpace, T>, T>.Slots;
	}
#endif
}
#endif