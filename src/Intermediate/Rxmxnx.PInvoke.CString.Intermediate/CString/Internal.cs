namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class from the value indicated by a specified
	/// pointer to an array of UTF-8 characters and a given length.
	/// </summary>
	/// <param name="ptr">A pointer to an array of UTF-8 characters.</param>
	/// <param name="length">The number of <see cref="Byte"/> units within the pointed array to be used.</param>
	internal CString(IntPtr ptr, Int32 length) : this(ptr, length, false) { }
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class which lives inside
	/// <paramref name="sequence"/> with <paramref name="index"/> as index element.
	/// </summary>
	/// <param name="sequence">The <see cref="CStringSequence"/> containing current UTF-8 text.</param>
	/// <param name="index">Index element of current UTF-8 text into <paramref name="sequence"/>.</param>
	internal CString(CStringSequence sequence, Int32 index)
	{
		this._isLocal = false;
		this.IsFunction = true;
		this._data = ValueRegion<Byte>.Create(new SequenceItemState(sequence, index), SequenceItemState.GetSpan);

		ReadOnlySpan<Byte> data = CStringSequence.GetItemSpan(sequence, index);
		this._isNullTerminated = true;
		this.Length = data.Length;
	}

	/// <summary>
	/// Writes the sequence of bytes to the provided <see cref="Stream"/> and advances
	/// the current position within this stream by the number of bytes written.
	/// </summary>
	/// <param name="strm">
	/// The <see cref="Stream"/> to which the content of the current <see cref="CString"/>
	/// will be copied.
	/// </param>
	/// <param name="writeNullTermination">
	/// Specifies whether the UTF-8 text should be written with a null-termination character
	/// into the <see cref="Stream"/>.
	/// </param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void Write(Stream strm, Boolean writeNullTermination)
	{
		strm.Write(this.AsSpan());
		if (writeNullTermination)
			strm.Write(CString.empty);
	}
	/// <summary>
	/// Writes the sequence of bytes to the given <see cref="Stream"/>, starting at the
	/// byte located at the <paramref name="startIndex"/> position, and writing up
	/// to <paramref name="count"/> bytes.
	/// </summary>
	/// <param name="strm">
	/// The <see cref="Stream"/> to which the contents of the current <see cref="CString"/>
	/// will be copied.
	/// </param>
	/// <param name="startIndex">The position of the first byte in the current instance to write.</param>
	/// <param name="count">The number of bytes in the current instance to write.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void Write(Stream strm, Int32 startIndex, Int32 count)
		=> strm.Write(this.AsSpan().Slice(startIndex, count));
	/// <summary>
	/// Asynchronously writes the sequence of bytes to the given <see cref="Stream"/> and advances
	/// the current position within this stream by the number of bytes written.
	/// </summary>
	/// <param name="strm">
	/// The <see cref="Stream"/> to which the contents of the current <see cref="CString"/>
	/// will be copied.
	/// </param>
	/// <param name="writeNullTermination">
	/// Specifies whether the UTF-8 text should be written with a null-termination character
	/// into the <see cref="Stream"/>.
	/// </param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	/// <returns>A task that represents the asynchronous write operation.</returns>
	internal async Task WriteAsync(Stream strm, Boolean writeNullTermination,
		CancellationToken cancellationToken = default)
	{
		await this.GetWriteTask(strm, 0, this.Length, cancellationToken).ConfigureAwait(false);
		if (writeNullTermination)
			await strm.WriteAsync(CString.empty, cancellationToken);
	}
	/// <summary>
	/// Asynchronously writes the sequence of bytes to the given <see cref="Stream"/>,
	/// starting at the byte located at the <paramref name="startIndex"/> position, and
	/// writing up to <paramref name="count"/> bytes.
	/// </summary>
	/// <param name="strm">
	/// The <see cref="Stream"/> to which the contents of the current <see cref="CString"/>
	/// will be copied.
	/// </param>
	/// <param name="startIndex">The position of the first byte in the current instance to write.</param>
	/// <param name="count">The number of bytes in the current instance to write.</param>
	/// <param name="cancellationToken">
	/// The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.
	/// </param>
	/// <returns>A task that represents the asynchronous write operation.</returns>
	internal async Task WriteAsync(Stream strm, Int32 startIndex, Int32 count,
		CancellationToken cancellationToken = default)
		=> await this.GetWriteTask(strm, startIndex, count, cancellationToken).ConfigureAwait(false);

	/// <summary>
	/// Creates a non-null-terminated <see cref="CString"/> instance that contains a single
	/// <paramref name="c"/> character.
	/// </summary>
	/// <param name="c">A UTF-8 byte.</param>
	/// <returns>
	/// A non-null-terminated <see cref="CString"/> instance that contains a single <paramref name="c"/> character.
	/// </returns>
	private static CString Create(Byte c) => new([c,], false);
	/// <summary>
	/// Retrieves the internal binary data from a given <see cref="CString"/>.
	/// </summary>
	/// <param name="value">A non-reference <see cref="CString"/> instance.</param>
	/// <returns>A <see cref="Byte"/> array representing the UTF-8 text.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
	/// <exception cref="InvalidOperationException"><paramref name="value"/> does not contain valid UTF-8 text.</exception>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Byte[] GetBytes(CString value)
	{
		ArgumentNullException.ThrowIfNull(value);
		ValidationUtilities.ThrowIfInvalidUtf8Region(value._data, nameof(value), out Byte[] result);
		return result;
	}
}