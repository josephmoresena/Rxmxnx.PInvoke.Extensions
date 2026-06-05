namespace Rxmxnx.PInvoke.Internal.Bootstrap;

/// <summary>
/// Internal object store with 2^5-1 binary space.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class ObjectMetadataStorage31 : G31<Object>, IBootstrapMetadataStorage
{
	/// <summary>
	/// Parameterless constructor.
	/// </summary>
	public ObjectMetadataStorage31() => MetadataStorage.Initialize(this, this.TypeMetadata);

	/// <inheritdoc/>
	public BinaryMap<T> GetBinaryMap<T>(UInt16 capacity, ref MetadataStorage<T>? instance, Boolean prepareSlots)
	{
		ValidationUtilities.ThrowIfInvalidSequenceIndex(capacity - 1, this.Capacity);
		if (typeof(T) == typeof(Object))
		{
			Debug.Assert(instance is not null);
			return new(instance);
		}
		if (instance is null || instance.Capacity < capacity)
			instance = new ValueMetadataStorage31<T>();
		return new(instance);
	}
}

/// <summary>
/// Internal object store with 2^7-1 binary space.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal sealed class ObjectMetadataStorage127 : G127<Object>, IBootstrapMetadataStorage
{
	/// <summary>
	/// Parameterless constructor.
	/// </summary>
	public ObjectMetadataStorage127() => MetadataStorage.Initialize(this, this.TypeMetadata);

	/// <inheritdoc/>
	public BinaryMap<T> GetBinaryMap<T>(UInt16 capacity, ref MetadataStorage<T>? instance, Boolean prepareSlots)
	{
		ValidationUtilities.ThrowIfInvalidSequenceIndex(capacity - 1, this.Capacity);
		if (typeof(T) == typeof(Object))
		{
			Debug.Assert(instance is not null);
			return new(instance);
		}
		if (instance is null || instance.Capacity < capacity)
			instance = capacity switch
			{
				> 31 => new ValueMetadataStorage127<T>(),
				_ => new ValueMetadataStorage31<T>(),
			};
		return new(instance);
	}
}

/// <summary>
/// Internal object store with 2^11-1 binary space.
/// </summary>
internal sealed class ObjectMetadataStorage2047 : G2047<Object>, IBootstrapMetadataStorage
{
	/// <summary>
	/// Parameterless constructor.
	/// </summary>
	public ObjectMetadataStorage2047(Boolean withSlots) : base(withSlots)
		=> MetadataStorage.Initialize(this, this.TypeMetadata);

	/// <inheritdoc/>
	public BinaryMap<T> GetBinaryMap<T>(UInt16 capacity, ref MetadataStorage<T>? instance, Boolean prepareSlots)
	{
		ValidationUtilities.ThrowIfInvalidSequenceIndex(capacity - 1,
		                                                this.Slots.Length == 0 ? this.Capacity : UInt16.MaxValue);
		if (typeof(T) == typeof(Object))
		{
			Debug.Assert(instance is not null);
			this.PrepareFor(capacity);
			return new(instance);
		}
		if (instance is null || instance.Capacity < capacity)
			instance = capacity switch
			{
				> 2047 when instance is ValueMetadataStorage2047<T> s2047 && prepareSlots => s2047.PrepareFor(capacity),
				> 127 => new ValueMetadataStorage2047<T>(this.Slots.Length > 0).PrepareFor(capacity),
				> 31 => new ValueMetadataStorage127<T>(),
				_ => new ValueMetadataStorage31<T>(),
			};
		return new(instance);
	}
}