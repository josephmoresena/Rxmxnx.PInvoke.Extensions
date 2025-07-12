namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// State value for sequence item.
	/// </summary>
	/// <param name="sequence">A <see cref="CStringSequence"/> instance.</param>
	/// <param name="index">The zero-based index of the element into the sequence.</param>
	/// <param name="length">Current UTF-8 text length.</param>
	private readonly struct SequenceItemState(CStringSequence sequence, Int32 index, Int32 length)
	{
		/// <summary>
		/// Sequence buffer.
		/// </summary>
		private readonly String _buffer = sequence.ToString();
		/// <summary>
		/// Offset for the current element.
		/// </summary>
		private readonly Int32 _offset = sequence.GetBinaryOffset(index);
		/// <summary>
		/// Length of the current element.
		/// </summary>
		private readonly Int32 _length = length;

		/// <summary>
		/// Allocates a <see cref="GCHandle"/> for the specified <paramref name="t"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GCHandle Alloc(SequenceItemState s, GCHandleType t) => GCHandle.Alloc(s._buffer, t);

		/// <summary>
		/// Retrieves the span from <paramref name="state"/>.
		/// </summary>
		/// <param name="state">Current item sequence state.</param>
		/// <returns>The binary span for the specified state.</returns>
		public static ReadOnlySpan<Byte> GetSpan(SequenceItemState state)
		{
			ReadOnlySpan<Byte> buffer = MemoryMarshal.AsBytes(state._buffer.AsSpan());
			return buffer.Slice(state._offset, state._length);
		}
	}
}