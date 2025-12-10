namespace Rxmxnx.PInvoke;

public partial class CString
{
	public sealed partial class Builder
	{
		/// <summary>
		/// Represents a chunk of a mutable <see cref="CString"/> instance.
		/// </summary>
		private sealed partial class Chunk
		{
			/// <summary>
			/// The number of bytes in the entire sequence up to and including this chunk.
			/// </summary>
			public Int32 Count => this.GetOffset() + this._count;

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="capacity">Initial buffer capacity.</param>
			public Chunk(UInt16 capacity) : this(null, CString.CreateByteArray(capacity), 0) { }

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
			/// Resets the current instance.
			/// </summary>
			/// <param name="capacity">Reset capacity.</param>
			public void Reset(Int32 capacity)
			{
				if (this._buffer.Length != capacity)
					this._buffer = CString.CreateByteArray(capacity);
				this._count = 0;
				this._previous = default;
			}
			/// <summary>
			/// Inserts <paramref name="newData"/> at the specified local <paramref name="index"/>, creating
			/// additional chunks if necessary.
			/// </summary>
			/// <param name="index">Local insertion index.</param>
			/// <param name="newData">Bytes to insert.</param>
			public void Insert(Int32 index, ReadOnlySpan<Byte> newData)
			{
				Chunk chunk = this.GetChunkFor(ref index);
				Int32 available = chunk._buffer.Length - chunk._count;
				Int32 remaining = chunk._count - index;

				if (newData.Length <= available)
				{
					Span<Byte> span = chunk._buffer.AsSpan()[index..];
					span[..^remaining].CopyTo(span[newData.Length..]);
					newData.CopyTo(span);
					chunk._count += newData.Length;
					return;
				}

				Int32 usableCount = chunk._buffer.Length - index;
				Int32 additionalRequired = newData.Length + remaining - usableCount;
				InsertInfo info = Chunk.GetInsertInfo(chunk._buffer.Length, additionalRequired);
				Chunk[] chunks = Chunk.GetInsertChunks(chunk, info);
				Chunk.Fill(chunks.AsSpan()[1..], newData[usableCount..], chunk._buffer.AsSpan()[index..remaining]);
				newData[..usableCount].CopyTo(chunks[0]._buffer.AsSpan()[index..]); // Copy the remaining newData.
			}
			/// <summary>
			/// Removes the specified range of characters from this instance.
			/// </summary>
			/// <param name="startIndex">The zero-based position in this instance where removal begins.</param>
			/// <param name="length">The number of characters to remove.</param>
			/// <returns>A reference to this instance after the excise operation has completed.</returns>
			public void Remove(Int32 startIndex, Int32 length)
			{
				Int32 endIndex = length + startIndex - 1;
				Chunk endChunk = this.GetChunkFor(ref endIndex);
				Chunk startChunk = this.GetChunkFor(ref startIndex);

				if (length == 0) return;
				if (Object.ReferenceEquals(endChunk, startChunk))
				{
					endChunk.RemoveRange(startIndex, endIndex + 1);
					return;
				}

				endChunk._count = 0;
				endChunk._previous = startChunk;
				startChunk.Remove(startIndex, startChunk._count - 1);
			}
			/// <summary>
			/// Copies the units from a specified segment of this instance to a destination <see cref="Byte"/> span.
			/// </summary>
			/// <param name="sourceIndex">
			/// The starting position in this instance where units will be copied from. The index is zero-based.
			/// </param>
			/// <param name="destination">The writable span where units will be copied.</param>
			public void CopyTo(Int32 sourceIndex, Span<Byte> destination)
			{
				if (destination.Length == 0) return;

				Int32 endIndex = destination.Length + sourceIndex - 1;
				Chunk endChunk = this.GetChunkFor(ref endIndex);
				Chunk startChunk = this.GetChunkFor(ref sourceIndex);

				if (Object.ReferenceEquals(endChunk, startChunk))
				{
					endChunk._buffer.AsSpan()[sourceIndex..(endIndex + 1)].CopyTo(destination);
					return;
				}
				Chunk.CopyTo(new() { Chunk = startChunk, Start = sourceIndex, Count = startChunk._count, },
				             new() { Chunk = endChunk, Start = 0, Count = endChunk._count, }, destination);
			}
		}
	}
}