namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Indicates whether the UTF-8 data is local.
    /// </summary>
    private readonly Boolean _isLocal;
    /// <summary>
    /// Indicates whether the UTF-8 data is a function.
    /// </summary>
    private readonly Boolean _isFunction;
    /// <summary>
    /// Internal object data.
    /// </summary>
    private readonly ValueRegion<Byte> _data;
    /// <summary>
    /// Indicates whether the UTF-8 text is null-terminated.
    /// </summary>
    private readonly Boolean _isNullTerminated;
    /// <summary>
    /// Number of bytes in the current <see cref="CString"/> object.
    /// </summary>
    private readonly Int32 _length;

    /// <summary>
    /// Calculates the data length of a segment of current <see cref="CString"/> whose 
    /// offset is <paramref name="offset"/> and whose length is <paramref name="length"/>.
    /// </summary>
    /// <param name="offset">Offset for segment.</param>
    /// <param name="length">Initial length for segment.</param>
    /// <returns>Final length for segment.</returns>
    private Int32 GetDataLength(Int32 offset, Int32 length)
    {
        ReadOnlySpan<Byte> bytes = this._data;
        bytes = bytes[offset..];
        if (bytes.Length > length && bytes[length] == default)
            length++;
        return length;
    }

    /// <summary>
    /// Hash calculation for instances whose length and termination allow it to be treated 
    /// as a sequence of UTF-16 units.
    /// </summary>
    /// <returns>A hash code for the current object.</returns>
    private Int32? GetHashCodeLengthNullMatch()
    {
        if ((this._length + 1) % CStringSequence.SizeOfChar == 0 && this.IsNullTerminated)
        {
            ReadOnlySpan<Byte> thisSpan = this;
            unsafe
            {
                fixed (void* ptr = &MemoryMarshal.GetReference(thisSpan))
                {
                    Int32 charCount = (this._length + 1) / CStringSequence.SizeOfChar;
                    ReadOnlySpan<Char> thisCharSpan = new(ptr, charCount);
                    return String.GetHashCode(thisCharSpan);
                }
            }
        }
        return default;
    }

    /// <summary>
    /// Retrieves the Task for writing the <see cref="CString"/> content into the 
    /// given <see cref="Stream"/>.
    /// </summary>
    /// <param name="strm">
    /// The <see cref="Stream"/> to which the contents of the current <see cref="CString"/> 
    /// will be copied.
    /// </param>
    /// <returns>A task that represents the asynchronous write operation.</returns>
    private Task GetWriteTask(Stream strm)
        => (Byte[]?)this._data is Byte[] array ? strm.WriteAsync(array, 0, this._length) :
        Task.Run(() => { strm.Write(this.AsSpan()); });

    /// <summary>
    /// Retrieves the equality parameters for current <see cref="CString"/> instance.
    /// </summary>
    /// <typeparam name="TInteger">Type used for integer comparision.</typeparam>
    /// <param name="offset">Output. Calculated offset.</param>
    /// <param name="count">Output. Calculated count.</param>
    private void GetEqualityParameters<TInteger>(out Int32 offset, out Int32 count) where TInteger : unmanaged
    {
        unsafe
        {
            Int32 sizeofT = sizeof(TInteger);
            offset = this._length % sizeofT;
            count = (this._length - offset) / sizeofT;
        }
    }
}