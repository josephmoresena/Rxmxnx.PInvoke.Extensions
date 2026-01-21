namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Helper class for stack/heap buffer allocation.
/// </summary>
internal static class StackAllocationHelper
{
	/// <summary>
	/// Threshold for stackalloc usage in bytes.
	/// </summary>
	internal const Int32 StackallocByteThreshold = 256;

	/// <summary>
	/// Threshold for stackalloc usage in bytes.
	/// </summary>
	[ThreadStatic]
	private static Int32 stackallocByteConsumed;

	/// <summary>
	/// Initialize stack bytes consume.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void InitStackBytes() => StackAllocationHelper.stackallocByteConsumed = 0;
	/// <summary>
	/// Consumes the stackalloc bytes if the required size is within the threshold.
	/// </summary>
	/// <param name="stackRequired">Required stack bytes to consume.</param>
	/// <param name="stackConsumed">
	/// Reference. Stack bytes consumed so far. This value is updated with the newly consumed bytes.
	/// </param>
	/// <returns>
	/// <see langword="true"/> if the stackalloc bytes were successfully consumed; otherwise, <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean ConsumeStackBytes(Int32 stackRequired, ref Int32 stackConsumed)
	{
		if (stackRequired <= 0) return true; // No bytes to consume, return true.
		if (StackAllocationHelper.stackallocByteConsumed + stackRequired >
		    StackAllocationHelper.StackallocByteThreshold)
			return false; // Stackalloc threshold exceeded.
		StackAllocationHelper.stackallocByteConsumed += stackRequired;
		stackConsumed += stackRequired; // Update the consumed bytes.
		return true;
	}
	/// <summary>
	/// Returns a rented array of the specified length and clears it.
	/// </summary>
	/// <typeparam name="T">Type of the array elements.</typeparam>
	/// <param name="length">Required length of the array to rent.</param>
	/// <param name="arr">Output. Rented array.</param>
	/// <param name="clear">Indicates whether the array is required to be cleared.</param>
	/// <returns>A span of the rented array with the specified length, cleared.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Span<T> RentArray<T>(Int32 length, out T[]? arr, Boolean clear) where T : unmanaged
	{
		arr = ArrayPool<T>.Shared.Rent(length); // Rent an array of the specified length.

		Span<T> result = arr.AsSpan()[..length];
		if (clear) result.Clear(); // Clear the usable span.
		return result;
	}
	/// <summary>
	/// Returns a rented array of the specified length and clears it.
	/// </summary>
	/// <typeparam name="T">Type of the array elements.</typeparam>
	/// <param name="tArray">Rented array to return to the pool.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ReturnArray<T>(T[]? tArray) where T : unmanaged
	{
		if (tArray is not null)
			ArrayPool<T>.Shared.Return(tArray);
	}
	/// <summary>
	/// Releases the stack bytes consumed by the converter.
	/// </summary>
	/// <param name="stackConsumed">Number of stack bytes consumed to release.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void ReleaseStackBytes(Int32 stackConsumed)
	{
		if (stackConsumed <= 0) return; // No bytes to release, return.
		StackAllocationHelper.stackallocByteConsumed -= stackConsumed;
		if (StackAllocationHelper.stackallocByteConsumed < 0)
			StackAllocationHelper.stackallocByteConsumed = 0; // Prevent negative consumption.
	}
	/// <summary>
	/// Determines whether a buffer of size <paramref name="bufferLength"/> can be reused to store a UTF-8 encoded
	/// text of length <paramref name="textLength"/> without causing excessive unused capacity.
	/// </summary>
	/// <param name="bufferLength">The total length of the buffer, in bytes.</param>
	/// <param name="textLength">The length of the UTF-8 encoded text, in bytes.</param>
	/// <returns>
	/// <see langword="true"/> if the buffer can be reused to hold the UTF-8 text efficiently; otherwise,
	/// <see langword="false"/>.
	/// </returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Boolean IsReusableBuffer(Int32 bufferLength, Int32 textLength)
		=> bufferLength - textLength <= bufferLength >> 4;
}