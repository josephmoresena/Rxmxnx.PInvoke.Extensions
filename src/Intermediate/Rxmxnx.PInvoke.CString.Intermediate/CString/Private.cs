namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Internal object that stores the data for this instance as a series of bytes.
	/// </summary>
	private readonly ValueRegion<Byte> _data;
	/// <summary>
	/// Indicates whether the UTF-8 data is retrieved using a function (delegate).
	/// </summary>
	private readonly Boolean _isFunction;
	/// <summary>
	/// Indicates whether the UTF-8 data is locally stored within this instance.
	/// </summary>
	private readonly Boolean _isLocal;
	/// <summary>
	/// Indicates whether the UTF-8 string represented by this instance is null-terminated.
	/// </summary>
	private readonly Boolean _isNullTerminated;
	/// <summary>
	/// The length, in bytes, of the UTF-8 string represented by this instance.
	/// </summary>
	private readonly Int32 _length;

	/// <summary>
	/// Calculates the final length of a segment of the current <see cref="CString"/>,
	/// starting at a given <paramref name="offset"/> and with an initial
	/// <paramref name="length"/>. The final length accounts for multiple UTF-8 null
	/// ending characters.
	/// </summary>
	/// <param name="offset">
	/// The starting position of the segment within the current <see cref="CString"/>.
	/// </param>
	/// <param name="length">The initial length of the segment.</param>
	/// <returns>The final length of the segment, accounting for any trailing null characters.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private Int32 GetDataLength(Int32 offset, Int32 length)
	{
		ReadOnlySpan<Byte> bytes = this._data;
		bytes = bytes[offset..];
		// Account for multiple UTF-8 null ending characters.
		while (bytes.Length > length && bytes[length] == default)
			length++;
		return length;
	}
	/// <summary>
	/// Retrieves a Task representing the asynchronous operation to write the content of the
	/// current <see cref="CString"/> into the specified <see cref="Stream"/>.
	/// </summary>
	/// <param name="strm">
	/// The <see cref="Stream"/> where the contents of the current <see cref="CString"/> will be written.
	/// </param>
	/// <param name="startIndex">The index in the current instance where writing begins.</param>
	/// <param name="count">The number of bytes to write from the current instance.</param>
	/// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
	/// <returns>A task representing the asynchronous write operation.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private Task GetWriteTask(Stream strm, Int32 startIndex, Int32 count, CancellationToken cancellationToken)
		=> (Byte[]?)this._data is { } array ?
			strm.WriteAsync(array, startIndex, count, cancellationToken) :
			Task.Run(() => this.Write(strm, startIndex, count), cancellationToken);
}