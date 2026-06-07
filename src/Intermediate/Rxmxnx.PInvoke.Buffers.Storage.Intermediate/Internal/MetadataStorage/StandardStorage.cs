namespace Rxmxnx.PInvoke.Internal;

internal static partial class MetadataStorage
{
	/// <summary>
	/// Standard buffer type metadata storage.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	private class StandardStorage<T> : MetadataStorage<T>, IBinarySlotsOwner<T>
	{
		/// <summary>
		/// Initial binary storage.
		/// </summary>
		private readonly BufferTypeMetadata<T>?[] _initial;

		/// <summary>
		/// Additional slots.
		/// </summary>
		public BufferTypeMetadata<T>?[]?[] Slots { get; }

		/// <inheritdoc/>
		public sealed override UInt16 Capacity => (UInt16)this._initial.Length;
		/// <inheritdoc/>
		public sealed override ref BufferTypeMetadata<T>? MetadataReference
			=> ref MemoryMarshal.GetReference(this._initial.AsSpan());

		UInt16 IBinarySlotsOwner<T>.InitialBinaryCapacity => this.Capacity;

		/// <summary>
		/// Parameterless constructor.
		/// </summary>
		// ReSharper disable once MemberCanBeProtected.Local
		public StandardStorage()
		{
			this._initial = new BufferTypeMetadata<T>?[2047];
			this.Slots = new BufferTypeMetadata<T>[]?[5];
		}

		/// <summary>
		/// Prepares the current instance for <paramref name="count"/>.
		/// </summary>
		/// <param name="count">Requested count.</param>
		public MetadataStorage<T> PrepareFor(UInt16 count)
		{
			(this as IBinarySlotsOwner<T>).PrepareFor(count);
			return this;
		}

		/// <summary>
		/// Defines an implicit conversion of a given <see cref="StandardStorage{T}"/> to a <see cref="BinaryMap{T}"/>.
		/// </summary>
		/// <param name="value">A <see cref="StandardStorage{T}"/> to implicitly convert.</param>
		public static implicit operator BinaryMap<T>(StandardStorage<T> value) => new(value._initial, value.Slots);
	}

	/// <summary>
	/// Standard Object buffer type metadata storage.
	/// </summary>
	private sealed class ObjectStandardStorage : StandardStorage<Object>, IMetadataStorageExt
	{
		/// <inheritdoc/>
		public BinaryMap<T> GetBinaryMap<T>(UInt16 capacity, ref MetadataStorage<T>? instance, Boolean prepareSlots)
		{
			if (typeof(T) == typeof(Object))
			{
				Debug.Assert(instance is not null);
				if (prepareSlots)
					this.PrepareFor(capacity);
				return (StandardStorage<T>)instance;
			}
			if (instance is StandardStorage<T> standard)
				standard.PrepareFor(capacity);
			else
				instance = new StandardStorage<T>().PrepareFor(capacity);
			return (StandardStorage<T>)instance;
		}
	}
}