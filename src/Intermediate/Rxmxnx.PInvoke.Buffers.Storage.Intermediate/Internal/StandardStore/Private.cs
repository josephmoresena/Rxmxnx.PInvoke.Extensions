namespace Rxmxnx.PInvoke.Internal;

internal sealed partial class StandardStore
{
	/// <summary>
	/// Retrieves binary metadata required for a buffer with <paramref name="count"/> items.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <param name="allowMinimal">Allow to return minimal buffer.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	private BufferTypeMetadata<T>? GetBinaryMetadata<T>(UInt16 count, Boolean allowMinimal)
	{
		BufferTypeMetadata<T>? result = this.GetFundamental<T>(count);
		if (result is null) return result;
		while (count - result.Size > 0)
		{
			UInt16 diff = (UInt16)(count - result.Size);
			BufferTypeMetadata<T>? aux = this.GetBinaryMetadata<T>(diff, false);
#if NET9_0_OR_GREATER
			using (Generic<T>.GetLock().EnterScope())
#else
			lock (Generic<T>.GetLock())
#endif
			{
				// Auxiliary metadata not found. Use minimal.
				if (aux is null)
					return allowMinimal ? Generic<T>.GetMinimal(count) : default;
				result = result.Compose(this, aux);
				if (result is null)
					// Unable to create composed metadata. Use minimal.
					return allowMinimal ? Generic<T>.GetMinimal(count) : default;
				Generic<T>.Add(result);
			}
		}
		return result;
	}
	/// <summary>
	/// Retrieves the fundamental metadata for a buffer with <paramref name="count"/> items.
	/// </summary>
	/// <typeparam name="T">Type of items in the buffer.</typeparam>
	/// <param name="count">Amount of items in required buffer.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	private BufferTypeMetadata<T>? GetFundamental<T>(UInt16 count)
	{
#if NET9_0_OR_GREATER
		using (Generic<T>.GetLock().EnterScope())
#else
		lock (Generic<T>.GetLock())
#endif
		{
			if (Generic<T>.TryGetBinary(count, out BufferTypeMetadata<T>? metadata))
				return metadata;
			UInt16 space = Generic<T>.MaxSpace;
			while (count < space) space /= 2;
			BufferTypeMetadata<T>? result = Generic<T>.GetBinaryBuffer(space);
			while (BuffersHelper.GetMaxValue(result.Size) < count)
			{
				result = result.Double(this);
				if (result is null) break;
				Generic<T>.Add(result);
				Generic<T>.MaxSpace = result.Size;
			}
			return result;
		}
	}
}