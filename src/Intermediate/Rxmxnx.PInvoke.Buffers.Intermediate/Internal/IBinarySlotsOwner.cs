namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Exposes binary type metadata slots.
/// </summary>
internal interface IBinarySlotsOwner<T>
{
	/// <summary>
	/// Additional slots.
	/// </summary>
	BufferTypeMetadata<T>?[]?[] Slots { get; }
	/// <summary>
	/// Initial binary capacity.
	/// </summary>
	UInt16 InitialBinaryCapacity { get; }
	/// <summary>
	/// Additional binary capacity.
	/// </summary>
	UInt16 AdditionalBinaryCapacity
	{
		get
		{
			UInt32 result = 0;
			foreach (BufferTypeMetadata<T>?[]? page in this.Slots.AsSpan())
			{
				if (page is null) break;
				result += (UInt32)page.Length;
			}
			Debug.Assert(result <= UInt16.MaxValue);
			return (UInt16)result;
		}
	}

	/// <summary>
	/// Prepares the current instance for <paramref name="count"/>.
	/// </summary>
	/// <param name="count">Requested count.</param>
	void PrepareFor(UInt16 count)
	{
		if (count <= this.InitialBinaryCapacity)
			return; // Nothing to prepare.
		ValidationUtilities.ThrowIfInvalidSequenceIndex(
			count - 1, this.Slots.Length == 0 ? this.InitialBinaryCapacity : UInt16.MaxValue);
		MetadataStorage<T>.InitializePages(count, this.InitialBinaryCapacity + 1, ref this.Slots[0]);
	}
}