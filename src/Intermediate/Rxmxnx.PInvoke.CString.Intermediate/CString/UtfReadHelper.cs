namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Helper struct for span/sequence reading.
	/// </summary>
	private readonly ref struct UtfReadHelper
	{
		/// <summary>
		/// Internal array instance.
		/// </summary>
		private readonly Byte[]? _array;

		/// <summary>
		/// Buffer span.
		/// </summary>
		public Span<Byte> Bytes { get; }
		/// <summary>
		/// Indicates whether the current buffer is an array instance.
		/// </summary>
		public Boolean HasArray => this._array is not null;

		/// <summary>
		/// Constructor. Used for stack allocation.
		/// </summary>
		/// <param name="bytes">A stack alocated span.</param>
		public UtfReadHelper(Span<Byte> bytes)
		{
			this._array = default;
			this.Bytes = bytes;
#if NET7_0_OR_GREATER
			bytes[^1] = default;
#endif
		}
		/// <summary>
		/// Constructor. Used for heap allocation.
		/// </summary>
		/// <param name="length">Array length.</param>
		public UtfReadHelper(Int32 length) : this()
		{
			this._array = CString.CreateByteArray(length + 1);
			this.Bytes = this._array;
#if NET5_0_OR_GREATER
			this._array[^1] = default;
#endif
		}

		/// <summary>
		/// Retrieves a <see cref="Byte"/> array from current instance.
		/// </summary>
		/// <param name="textLength">UTF-8 text length in the buffer.</param>
		/// <returns>A <see cref="Byte"/> array from current instance.</returns>
		public Byte[] ToArray(Int32 textLength)
		{
			if (this._array is not null && StackAllocationHelper.IsReusableBuffer(this._array.Length, textLength))
				return this._array;

			// Allocate a new array sized to fit the UTF-8 data.
			Byte[] byteArray = CString.CreateByteArray(textLength + 1);
			// Copy the valid UTF-8 data into the new buffer.
			this.Bytes[..byteArray.Length].CopyTo(byteArray.AsSpan());
			return byteArray;
		}
	}
}