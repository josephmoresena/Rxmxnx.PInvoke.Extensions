namespace Rxmxnx.PInvoke;

public partial class CStringBuilder
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
			if (newData.IsEmpty) return this;

			Span<Byte> span = this.GetAvailable();
			if (newData.Length <= span.Length)
			{
				newData.CopyTo(span);
				this._count += newData.Length;
				return this;
			}
			newData[..span.Length].CopyTo(span);
			this._count += span.Length;
			return new Chunk(this).Append(newData[span.Length..]);
		}
		/// <summary>
		/// Appends a span of chars to the sequence, allocating new chunks as needed.
		/// </summary>
		/// <param name="newData">Data to append.</param>
		/// <returns>The chunk into which the final portion of <paramref name="newData"/> was written.</returns>
		public Chunk Append(ReadOnlySpan<Char> newData)
			=> newData.IsEmpty ? this : this.Append(Encoding.UTF8.GetByteCount(newData), newData);
#if NET8_0_OR_GREATER
		/// <summary>
		/// Appends a <see cref="IUtf8SpanFormattable"/> value to the sequence, allocating new chunks as needed.
		/// </summary>
		/// <typeparam name="T">A <see cref="IUtf8SpanFormattable"/> instance.</typeparam>
		/// <param name="value">A <see cref="IUtf8SpanFormattable"/> instance.</param>
		/// <returns>The chunk into which the final portion of <paramref name="value"/> was written.</returns>
		public Chunk AppendUtf8<T>(T value) where T : IUtf8SpanFormattable, ISpanFormattable
		{
			if (!value.TryFormat(this.GetAvailable(), out Int32 count, default, default))
				return (this._buffer.Length - this._count) switch
				{
					< StackAllocationHelper.StackallocByteThreshold when Chunk.AppendUtf8(this, value) is { } chunk =>
						chunk,
					_ => this.AppendUtf16(value),
				};

			this._count += count;
			return this;
		}
#endif
#if NET6_0_OR_GREATER
		/// <summary>
		/// Appends a <see cref="ISpanFormattable"/> value to the sequence, allocating new chunks as needed.
		/// </summary>
		/// <typeparam name="T">A <see cref="ISpanFormattable"/> instance.</typeparam>
		/// <param name="value">A <see cref="ISpanFormattable"/> instance.</param>
		/// <returns>The chunk into which the final portion of <paramref name="value"/> was written.</returns>
#if !NET8_0_OR_GREATER
		public Chunk AppendUtf16<T>(T value) where T : ISpanFormattable
#else
		private Chunk AppendUtf16<T>(T value) where T : ISpanFormattable
#endif
		{
			if (Chunk.AppendUtf16(this, value) is { } chunk)
				return chunk;

			ReadOnlySpan<Char> chars = value.ToString();
			return this.Append(Encoding.UTF8.GetByteCount(chars), chars);
		}
#endif
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
		/// Inserts <paramref name="newData"/> at the specified index creating additional chunks if necessary.
		/// </summary>
		/// <param name="index">Insertion index.</param>
		/// <param name="newData">Chars to insert.</param>
		public void Insert(Int32 index, ReadOnlySpan<Char> newData)
		{
			Int32 byteCount = Encoding.UTF8.GetByteCount(newData);
			CharSpanUtf8Split split = new(newData, byteCount, StackAllocationHelper.StackallocByteThreshold);
			Int32 bytes = 0;
			do
			{
				bytes += Chunk.InsertChars(this, index + bytes, split.Left);
				split = new(split.Right, Encoding.UTF8.GetByteCount(split.Right),
				            StackAllocationHelper.StackallocByteThreshold);
			} while (!split.Left.IsEmpty);
		}
		/// <summary>
		/// Inserts <paramref name="newData"/> at the specified index creating additional chunks if necessary.
		/// </summary>
		/// <param name="index">Insertion index.</param>
		/// <param name="newData">Bytes to insert.</param>
		public void Insert(Int32 index, ReadOnlySpan<Byte> newData)
		{
			Chunk chunk = index == this.Count ? this : this.GetChunkFor(ref index);
			Int32 available = chunk._buffer.Length - chunk._count;
			Int32 remaining = chunk._count - index;

			if (newData.Length <= available)
			{
				// The new data fits entirely in the current chunk.
				Span<Byte> span = chunk._buffer.AsSpan()[index..];
				span[..remaining].CopyTo(span[newData.Length..]);
				newData.CopyTo(span);
				chunk._count += newData.Length;
				return;
			}

			// Maximum number of bytes that can be handled by reusing the current chunk and its existing data.
			Int32 offset = Math.Min(available + remaining, newData.Length);
			// Portion of new data that does not fit in the current buffer chunk (in another buffer instance).
			ReadOnlySpan<Byte> lastNewData = newData[offset..];
			// Portion of new data that will be inserted into the current chunk (with a new buffer).
			ReadOnlySpan<Byte> firstNewData = newData[..offset];
			// Existing data that must be moved after the inserted data.
			ReadOnlySpan<Byte> oldData = chunk._buffer.AsSpan().Slice(index, remaining);
			// Destination span in the current chunk buffer for the first portion.
			Span<Byte> firstSpan = chunk._buffer.AsSpan()[index..];

			// Compute how many chunks are required and their sizes to store the remaining new data and shifted old data.
			Int32 newRequiredBytes = lastNewData.Length + oldData.Length;
			Boolean useSameSize = !Object.ReferenceEquals(this, chunk) && chunk._buffer.Length >= newRequiredBytes / 2;
			InsertInfo info = Chunk.GetInsertInfo(chunk._buffer.Length, newRequiredBytes, useSameSize);
			// Create and link the new chunks before the current one. 
			Chunk[] chunks = Chunk.GetInsertChunks(chunk, info, index + firstNewData.Length, useSameSize);

			// Fill the chunks with the last portion of the new data followed by old data.
			Chunk.Fill(chunks.AsSpan()[1..], lastNewData, oldData);
			// Copy the first portion of the new data into the first chunk buffer.
			firstNewData.CopyTo(firstSpan);
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

			endChunk._previous = startChunk;
			endChunk.RemoveRange(0, endIndex + 1);
			startChunk._count = startIndex;
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
		/// <summary>
		/// Enumerates the information of the linked chunks.
		/// </summary>
		/// <returns>An enumeration with the linked chunks information.</returns>
		public IEnumerable<CStringBuilderDebugView.ChunkInfo> EnumerateInformation()
		{
			Chunk? chunk = this;
			while (chunk is not null)
			{
				yield return new(chunk._buffer.Length, chunk._count);
				chunk = chunk._previous;
			}
		}
	}
}