namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// State value for buffer item.
	/// </summary>
	[Preserve(AllMembers = true, Conditional = true)]
	private readonly struct BufferItemState<TBuffer> where TBuffer : class, IUtf8Buffer
	{
		/// <summary>
		/// Buffer element index.
		/// </summary>
		private readonly Int32 _index;
		/// <summary>
		/// Internal UTF-8 buffer.
		/// </summary>
		private readonly TBuffer _utf8;
		/// <summary>
		/// Length of the current element.
		/// </summary>
		private readonly Int32 _length;
		/// <summary>
		/// Offset for the current element.
		/// </summary>
		private readonly Int32 _offset;

		/// <summary>
		/// State value for sequence item.
		/// </summary>
		/// <param name="utf8">A <typaramref name="TBuffer"/> instance.</param>
		/// <param name="index">The zero-based index of the element into the sequence.</param>
		/// <param name="length">Current UTF-8 text length.</param>
		public BufferItemState(TBuffer utf8, Int32 index, Int32 length)
		{
			this._index = index;
			this._utf8 = utf8;
			this._length = length;
			this._offset = utf8.GetBinaryOffset(index);
		}

		/// <summary>
		/// Retrieves the <see cref="CStringSequence"/> instance of the current item.
		/// </summary>
		/// <param name="itemIndex">Output. The index of the current item in the sequence.</param>
		/// <returns>The <see cref="CStringSequence"/> instance of the current item.</returns>
		public TBuffer GetSequence(out Int32 itemIndex)
		{
			itemIndex = this._index;
			return this._utf8;
		}

		/// <summary>
		/// Allocates a <see cref="GCHandle"/> for the specified <paramref name="t"/>.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static GCHandle Alloc(BufferItemState<TBuffer> s, GCHandleType t) => s._utf8.Alloc(t);

		/// <summary>
		/// Retrieves the span from <paramref name="state"/>.
		/// </summary>
		/// <param name="state">Current item sequence state.</param>
		/// <returns>The binary span for the specified state.</returns>
		public static ReadOnlySpan<Byte> GetSpan(BufferItemState<TBuffer> state)
			=> state._utf8.Buffer.Slice(state._offset, state._length);
	}
}