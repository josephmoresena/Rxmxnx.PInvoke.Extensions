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
}