namespace Rxmxnx.PInvoke;

public partial class CStringBuilder
{
	private sealed partial class Chunk
	{
		/// <summary>
		/// Maximum array length.
		/// </summary>
		private const Int32 maxLength = UInt16.MaxValue * (Byte.MaxValue * Byte.MaxValue / 2);

		/// <summary>
		/// Allocates a new buffer for the next chunk based on the capacity of the current chunk.
		/// </summary>
		/// <param name="chunk">Current <see cref="Chunk"/> instance.</param>
		/// <param name="sameSize">
		/// Indicates whether the next array is the same size of <paramref name="chunk"/>.
		/// </param>
		/// <returns>A new <see cref="Byte"/> array.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Byte[] CreateNextArray(Chunk chunk, Boolean sameSize = false)
		{
			Int32 newLength = !sameSize ? Chunk.GetNextCapacity(chunk._buffer.Length) : chunk._buffer.Length;
			return CString.CreateByteArray(newLength);
		}
		/// <summary>
		/// Computes the next capacity in the growth sequence for chunk buffers.
		/// </summary>
		/// <param name="capacity">Current chunk capacity.</param>
		/// <returns>The next chunk capacity.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static Int32 GetNextCapacity(Int32 capacity) => Math.Min(2 * capacity, Chunk.maxLength);
		/// <summary>
		/// Computes how many additional chunks are required for an insertion, and their sizes.
		/// </summary>
		/// <param name="capacity">Current chunk capacity.</param>
		/// <param name="required">Required new bytes.</param>
		/// <param name="constantLength">
		/// Indicates whether the required chunk capacity are equals to <paramref name="capacity"/>.
		/// </param>
		/// <returns>Information for data insertion.</returns>
		private static InsertInfo GetInsertInfo(Int32 capacity, Int32 required, Boolean constantLength)
		{
			Byte count = 0;
			Int32 sumCapacity = 0;
			while (required > sumCapacity)
			{
				if (!constantLength)
					capacity = Math.Min(2 * capacity, UInt16.MaxValue);
				sumCapacity += capacity;
				count++;
			}
			return new() { Chunks = count, LastCount = capacity - (sumCapacity - required), };
		}
		/// <summary>
		/// Creates and arranges the chunks needed for an insertion operation.
		/// </summary>
		/// <param name="current">Current <see cref="Chunk"/> instance.</param>
		/// <param name="info">Insertion information.</param>
		/// <param name="initialCount">Initial count value.</param>
		/// <param name="sameSize">
		/// Indicates whether the next array is the same size of <paramref name="current"/>.
		/// </param>
		/// <returns>The array of insertion chunks.</returns>
		private static Chunk[] GetInsertChunks(Chunk current, InsertInfo info, Int32 initialCount, Boolean sameSize)
		{
			Chunk[] chunks = new Chunk[info.Chunks + 1];
			// The lowest chunk retains current buffer.
			chunks[0] = new(current._previous, current._buffer, initialCount);
			// The highest chunk is the current chunk.
			chunks[^1] = current;
			for (Int32 i = 1; i < chunks.Length - 1; i++)
				chunks[i] = new(chunks[i - 1], Chunk.CreateNextArray(chunks[i - 1], sameSize));
			current._count = info.LastCount;
			current._previous = chunks[^2];
			// The current chunk has new buffer.
			current._buffer = Chunk.CreateNextArray(current._previous, sameSize);
			return chunks;
		}
		/// <summary>
		/// Fills the provided insertion chunks with the supplied byte segments.
		/// </summary>
		/// <param name="chunks">A read-only <see cref="Chunk"/> span.</param>
		/// <param name="firstData">First data bytes.</param>
		/// <param name="lastData">Last data bytes.</param>
		private static void Fill(ReadOnlySpan<Chunk> chunks, ReadOnlySpan<Byte> firstData, ReadOnlySpan<Byte> lastData)
		{
			for (Int32 i = chunks.Length - 1; i >= 0; i--)
			{
				Span<Byte> span = chunks[i]._buffer.AsSpan()[..chunks[i]._count]; // Gets the available span.
				if (!lastData.IsEmpty)
				{
					// Copy the last bytes from lastData
					Int32 lastCount = Chunk.CopyLast(ref lastData, span);
					if (span.Length == lastCount)
						continue; // The lastData already fills the available span from the chunk.
					span = span[..^lastCount]; // Shrinks the available span.
				}
				// Copy the last bytes from firstData
				_ = Chunk.CopyLast(ref firstData, span);
			}
		}
		/// <summary>
		/// Copies data from <paramref name="source"/> to <paramref name="destination"/>
		/// and reduces <paramref name="source"/> accordingly.
		/// </summary>
		/// <param name="source">Source span to copy from; updated to represent the remaining data.</param>
		/// <param name="destination">Destination <see cref="Byte"/> span</param>
		/// <returns>The number of bytes copied.</returns>
		private static Int32 CopyLast(ref ReadOnlySpan<Byte> source, Span<Byte> destination)
		{
			Int32 result = Math.Min(source.Length, destination.Length);
			source[^result..].CopyTo(destination[^result..]);
			source = source[..^result];
			return result;
		}
		/// <summary>
		/// Copies the units from a specified segment of this instance to a destination <see cref="Byte"/> span.
		/// </summary>
		/// <param name="start">The starting chunk where units will be copied from.</param>
		/// <param name="end">The ending chunk where units will be copied from.</param>
		/// <param name="destination">The writable span where units will be copied.</param>
		private static void CopyTo(CopyInfo start, CopyInfo end, Span<Byte> destination)
		{
			do
			{
				ReadOnlySpan<Byte> source = end.Chunk._buffer.AsSpan()[end.Start..end.Count];
				end = end.Chunk._previous is not null && !Object.ReferenceEquals(end.Chunk._previous, start.Chunk) ?
					new() { Chunk = end.Chunk._previous, Start = 0, Count = end.Chunk._previous._count, } :
					start;
				source.CopyTo(destination[^source.Length..]);
				destination = destination[..^source.Length];
			} while (!destination.IsEmpty);
		}
		/// <summary>
		/// Inserts <paramref name="chars"/> on <paramref name="chunk"/>.
		/// </summary>
		/// <param name="chunk">A <see cref="Chunk"/> instance.</param>
		/// <param name="index">Insertion index.</param>
		/// <param name="chars">A read-only character span.</param>
		private static Int32 InsertChars(Chunk chunk, Int32 index, ReadOnlySpan<Char> chars)
		{
			Span<Byte> temp = stackalloc Byte[StackAllocationHelper.StackallocByteThreshold];
			Int32 bytes = Encoding.UTF8.GetBytes(chars, temp);
			chunk.Insert(index, temp[..bytes]);
			return bytes;
		}
#if NET8_0_OR_GREATER
		/// <summary>
		/// Appends a <see cref="IUtf8SpanFormattable"/> value to the sequence, allocating new chunks as needed.
		/// </summary>
		/// <typeparam name="T">A <see cref="IUtf8SpanFormattable"/> instance.</typeparam>
		/// <param name="chunk">A <see cref="Chunk"/> instance.</param>
		/// <param name="value">A <see cref="IUtf8SpanFormattable"/> instance.</param>
		/// <returns>A <see cref="Chunk"/> instance.</returns>
		[SkipLocalsInit]
		private static Chunk? AppendUtf8<T>(Chunk chunk, T value) where T : IUtf8SpanFormattable
		{
			Span<Byte> span = stackalloc Byte[StackAllocationHelper.StackallocByteThreshold];
			return value.TryFormat(span, out Int32 count, default, default) ? chunk.Append(span[..count]) : default;
		}
#endif
#if NET6_0_OR_GREATER
		/// <summary>
		/// Appends a <see cref="ISpanFormattable"/> value to the sequence, allocating new chunks as needed.
		/// </summary>
		/// <typeparam name="T">A <see cref="ISpanFormattable"/> instance.</typeparam>
		/// <param name="chunk">A <see cref="Chunk"/> instance.</param>
		/// <param name="value">A <see cref="ISpanFormattable"/> instance.</param>
		/// <returns>A <see cref="Chunk"/> instance.</returns>
		[SkipLocalsInit]
		private static Chunk? AppendUtf16<T>(Chunk chunk, T value) where T : ISpanFormattable
		{
			Span<Char> span = stackalloc Char[StackAllocationHelper.StackallocByteThreshold];
			return value.TryFormat(span, out Int32 count, default, default) ? chunk.Append(span[..count]) : default;
		}
#endif
	}
}