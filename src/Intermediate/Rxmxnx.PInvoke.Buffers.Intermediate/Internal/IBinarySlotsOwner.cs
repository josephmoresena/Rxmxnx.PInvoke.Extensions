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
}

/// <summary>
/// A <see cref="IBinarySlotsOwner{T}"/> extension class.
/// </summary>
internal static class BinarySlotsOwnerExtensions
{
	/// <summary>
	/// Calculates the additional binary capacity.
	/// </summary>
	/// <param name="owner">Current <see cref="IBinarySlotsOwner{T}"/> instance.</param>
	/// <returns>Additional binary capacity</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UInt16 GetAdditionalBinaryCapacity<T>(this IBinarySlotsOwner<T> owner)
	{
		UInt32 result = 0;
		foreach (BufferTypeMetadata<T>?[]? page in owner.Slots.AsSpan())
		{
			if (page is null) break;
			result += (UInt32)page.Length;
		}
		Debug.Assert(result <= UInt16.MaxValue);
		return (UInt16)result;
	}
	/// <summary>
	/// Prepares the current instance for <paramref name="count"/>.
	/// </summary>
	/// <param name="owner">Current <see cref="IBinarySlotsOwner{T}"/> instance.</param>
	/// <param name="count">Requested count.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void PrepareFor<T>(this IBinarySlotsOwner<T> owner, UInt16 count)
	{
		if (count <= owner.InitialBinaryCapacity)
			return; // Nothing to prepare.
		ValidationUtilities.ThrowIfInvalidSequenceIndex(
			count - 1, owner.Slots.Length == 0 ? owner.InitialBinaryCapacity : UInt16.MaxValue);
		Int32 pageLength = owner.InitialBinaryCapacity + 1;
		foreach (ref BufferTypeMetadata<T>?[]? page in owner.Slots.AsSpan())
		{
			if (page is not null)
			{
				pageLength = page.Length;
				continue;
			}
			if (count < pageLength) return;
			Interlocked.CompareExchange(ref page, new BufferTypeMetadata<T>?[pageLength], null);
			pageLength *= 2;
		}
	}
}