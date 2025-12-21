namespace Rxmxnx.PInvoke;

public partial class CStringBuilder
{
	private sealed partial class Chunk
	{
		/// <summary>
		/// Underlying byte buffer for this chunk.
		/// </summary>
		private Byte[] _buffer;
		/// <summary>
		/// Number of valid bytes stored in this chunk.
		/// </summary>
		private Int32 _count;
		/// <summary>
		/// The previous chunk in the chain.
		/// </summary>
		private Chunk? _previous;

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
		private Chunk GetChunkFor(ref Int32 index) => this.GetChunkFor(ref index, out _);
		/// <summary>
		/// Retrieves the chunk that contains the element at the global index <paramref name="index"/>.
		/// </summary>
		/// <param name="index">Input: global index; Output: local index in the resulting chunk.</param>
		/// <param name="next">Output: Next chunk to the resulting chunk.</param>
		/// <returns>The chunk corresponding to the provided index.</returns>
		private Chunk GetChunkFor(ref Int32 index, out Chunk? next)
		{
			Chunk result = this;
			ValidationUtilities.ThrowIfInvalidSequenceIndex(index, this.GetOffset() + this._count);
			next = default;
			while (index < result.GetOffset())
			{
				next = result;
				result = result._previous!; // After the index validation, previous will never be null.
			}
			index -= result.GetOffset();
			return result;
		}
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
		/// Removes the range from <paramref name="start"/> to <paramref name="length"/>
		/// </summary>
		/// <param name="start">Start index.</param>
		/// <param name="length">Range length.</param>
		private void RemoveRange(Int32 start, Int32 length)
		{
			if (length >= this._count)
			{
				// Truncate the chunk at the start position.
				this._count = start;
				return;
			}

			ReadOnlySpan<Byte> source = this._buffer.AsSpan()[length..this._count];
			Span<Byte> chunkBuffer = this._buffer.AsSpan()[start..];
			source.CopyTo(chunkBuffer);

			this._count = start + source.Length;
		}
		/// <summary>
		/// Appends a span of chars to the sequence, allocating new chunks as needed.
		/// </summary>
		/// <param name="byteCount">UTF-8 bytes required to encode <paramref name="newData"/>.</param>
		/// <param name="newData">Data to append.</param>
		/// <returns>The chunk into which the final portion of <paramref name="newData"/> was written.</returns>
		private Chunk Append(Int32 byteCount, ReadOnlySpan<Char> newData)
		{
			Span<Byte> span = this.GetAvailable();
			if (byteCount <= span.Length)
			{
				this._count += Encoding.UTF8.GetBytes(newData, span);
				return this;
			}
			CharSpanUtf8Split split = new(newData, byteCount, span.Length);
			Int32 leftByteCount = Encoding.UTF8.GetBytes(split.Left, span);
			this._count += leftByteCount;
			return new Chunk(this).Append(Encoding.UTF8.GetByteCount(split.Right), split.Right);
		}
	}
}