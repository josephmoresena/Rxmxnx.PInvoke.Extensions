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
        //Multiple UTF-8 null ending character.
        while(bytes.Length > length && bytes[length] == default)
            length++;
        return length;
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
}