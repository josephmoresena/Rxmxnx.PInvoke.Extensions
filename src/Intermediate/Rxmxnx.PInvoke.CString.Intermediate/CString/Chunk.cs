namespace Rxmxnx.PInvoke;

public partial class CString
{
	public sealed partial class Builder
	{
		/// <summary>
		/// Represents a chunk of a mutable <see cref="CString"/> instance.
		/// </summary>
		private sealed class Chunk
		{
			/// <summary>
			/// Number of valid bytes stored in this chunk.
			/// </summary>
			private Int32 _count;
			/// <summary>
			/// Underlying byte buffer for this chunk.
			/// </summary>
			private Byte[] _buffer;
			/// <summary>
			/// The previous chunk in the chain.
			/// </summary>
			private Chunk? _previous;
			/// <summary>
			/// The number of bytes in the entire sequence up to and including this chunk.
			/// </summary>
			public Int32 Count => this.GetOffset() + this._count;

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="capacity">Initial buffer capacity.</param>
			public Chunk(Int32 capacity) : this(null, Chunk.CreateArray(capacity), 0) { }

			/// <summary>
			/// Private constructor.
			/// </summary>
			/// <param name="previous">The previous <see cref="Chunk"/> instance.</param>
			private Chunk(Chunk previous) : this(previous, Chunk.CreateNextArray(previous), 0) { }
			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="previous">The previous <see cref="Chunk"/> instance.</param>
			/// <param name="buffer">The current data chunk.</param>
			private Chunk(Chunk? previous, Byte[] buffer) : this(previous, buffer, buffer.Length) { }
			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="previous">The previous <see cref="Chunk"/> instance.</param>
			/// <param name="buffer">The current data chunk.</param>
			/// <param name="count">The current count instance.</param>
			private Chunk(Chunk? previous, Byte[] buffer, Int32 count)
			{
				this._previous = previous;
				this._count = count;
				this._buffer = buffer;
			}

			/// <summary>
			/// Retrieves the chunk that contains the element at the global index <paramref name="index"/>.
			/// </summary>
			/// <param name="index">Input: global index; Output: local index in the resulting chunk.</param>
			/// <returns>The chunk corresponding to the provided index.</returns>
			public Chunk GetChunkFor(ref Int32 index)
			{
				Chunk result = this;
				ValidationUtilities.ThrowIfInvalidSequenceIndex(index, this.GetOffset() + this._count);
				while (index < result.GetOffset())
					result = result._previous!; // After the index validation, previous will never be null.
				index -= result.GetOffset();
				return result;
			}
			/// <summary>
			/// Appends a span of bytes to the sequence, allocating new chunks as needed.
			/// </summary>
			/// <param name="newData">Data to append.</param>
			/// <returns>The chunk into which the final portion of <paramref name="newData"/> was written.</returns>
			public Chunk Append(ReadOnlySpan<Byte> newData)
			{
				Span<Byte> span = this.GetAvailable();
				if (newData.Length <= span.Length)
				{
					newData.CopyTo(span);
					this._count += newData.Length;
					return this;
				}
				newData[..span.Length].CopyTo(span);
				return new Chunk(this).Append(newData[span.Length..]);
			}
			/// <summary>
			/// Inserts <paramref name="newData"/> at the specified local <paramref name="index"/>, creating
			/// additional chunks if necessary.
			/// </summary>
			/// <param name="index">Local insertion index.</param>
			/// <param name="newData">Bytes to insert.</param>
			public void Insert(Int32 index, ReadOnlySpan<Byte> newData)
			{
				Int32 available = this._buffer.Length - this._count;
				Int32 remaining = this._count - index;
				if (newData.Length <= available)
				{
					Span<Byte> span = this._buffer.AsSpan()[index..];
					span[..^remaining].CopyTo(span[newData.Length..]);
					newData.CopyTo(span);
					this._count += newData.Length;
					return;
				}

				Int32 usableCount = this._buffer.Length - index;
				Int32 additionalRequired = newData.Length + remaining - usableCount;
				InsertInfo info = Chunk.GetInsertInfo(this._buffer.Length, additionalRequired);
				Chunk[] chunks = Chunk.GetInsertChunks(this, info);
				Chunk.Fill(chunks.AsSpan()[1..], newData[usableCount..], this._buffer.AsSpan()[index..remaining]);
				newData[..usableCount].CopyTo(chunks[0]._buffer.AsSpan()[index..]); // Copy the remaining newData.
			}
			/// <summary>
			/// Returns a read-only span covering the valid portion of the current instance.
			/// </summary>
			public ReadOnlySpan<Byte> AsSpan() => this._buffer.AsSpan()[..this._count];

			/// <summary>
			/// Retrieves the cumulative offset of this chunk within the full sequence.
			/// </summary>
			private Int32 GetOffset() => this._previous?.Count ?? 0;
			/// <summary>
			/// Retrieves the span of unused space in this chunk.
			/// </summary>
			/// <returns>The span of unused space in this chunk.</returns>
			private Span<Byte> GetAvailable() => this._buffer.AsSpan()[this._count..];

			/// <summary>
			/// Allocates a new buffer for the next chunk based on the capacity of the current chunk.
			/// </summary>
			/// <param name="chunk">Current <see cref="Chunk"/> instance.</param>
			/// <returns>A new <see cref="Byte"/> array.</returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static Byte[] CreateNextArray(Chunk chunk)
				=> Chunk.CreateArray(Chunk.GetNextCapacity(chunk._buffer.Length));
			/// <summary>
			/// Allocates a new byte buffer of the specified length.
			/// </summary>
			/// <param name="length">Length of the array.</param>
			/// <returns>A new <see cref="Byte"/> array.</returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static Byte[] CreateArray(Int32 length)
#if !NET5_0_OR_GREATER
				=> new Byte[length];
#else
				=> GC.AllocateUninitializedArray<Byte>(length);
#endif
			/// <summary>
			/// Computes the next capacity in the growth sequence for chunk buffers.
			/// </summary>
			/// <param name="capacity">Current chunk capacity.</param>
			/// <returns>The next chunk capacity.</returns>
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			private static Int32 GetNextCapacity(Int32 capacity) => Math.Min(2 * capacity, UInt16.MaxValue);
			/// <summary>
			/// Computes how many additional chunks are required for an insertion, and their sizes.
			/// </summary>
			/// <param name="capacity">Current chunk capacity.</param>
			/// <param name="required">Required new bytes.</param>
			/// <returns>Information for data insertion.</returns>
			private static InsertInfo GetInsertInfo(Int32 capacity, Int32 required)
			{
				Byte count = 0;
				Int32 sumCapacity = 0;
				while (required > sumCapacity)
				{
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
			/// <returns>The array of insertion chunks.</returns>
			private static Chunk[] GetInsertChunks(Chunk current, InsertInfo info)
			{
				Chunk[] chunks = new Chunk[info.Chunks + 1];
				chunks[0] = new(current._previous, current._buffer); // The lowest chunk retains current buffer.
				for (Int32 i = 1; i < chunks.Length - 1; i++)
					chunks[i] = new(current._previous, Chunk.CreateNextArray(chunks[i - 1]));
				chunks[^1] = current; // The highest chunk is the current chunk.
				current._count = info.LastCount;
				current._previous = chunks[^2];
				current._buffer = Chunk.CreateNextArray(current._previous); // The current chunk has new buffer.
				return chunks;
			}
			/// <summary>
			/// Fills the provided insertion chunks with the supplied byte segments.
			/// </summary>
			/// <param name="chunks">A read-only <see cref="Chunk"/> span.</param>
			/// <param name="firstData">First data bytes.</param>
			/// <param name="lastData">Last data bytes.</param>
			private static void Fill(ReadOnlySpan<Chunk> chunks, ReadOnlySpan<Byte> firstData,
				ReadOnlySpan<Byte> lastData)
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
			/// Structure containing metadata required to allocate chunks for an insertion.
			/// </summary>
			private struct InsertInfo
			{
				/// <summary>
				/// Total number of newly allocated chunks.
				/// </summary>
				public Byte Chunks { get; init; }
				/// <summary>
				/// Number of bytes written into the last chunk.
				/// </summary>
				public Int32 LastCount { get; init; }
			}
		}
	}
}