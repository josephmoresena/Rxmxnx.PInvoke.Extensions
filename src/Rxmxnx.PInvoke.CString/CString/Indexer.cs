namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Gets the <see cref="Byte"/> object at a specified position in the current <see cref="CString"/>
    /// object.
    /// </summary>
    /// <param name="index">A position in the current UTF-8 text.</param>
    /// <returns>The object at position <paramref name="index"/>.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="index"/> is greater than or equal to the length of this object or less than zero.
    /// </exception>
    [IndexerName("Position")]
    public Byte this[Int32 index] => this._data[index];
    /// <summary>
    /// Gets a <see cref="CString"/> instance at specified range from current <see cref="CString"/> instace.
    /// </summary>
    /// <param name="range"></param>
    /// <returns><see cref="CString"/> range.</returns>
    [IndexerName("Position")]
    public CString this[Range range]
    {
        get
        {
            (Int32 offset, Int32 length) = range.GetOffsetAndLength(this._length);
            Boolean inRange = offset < this._length && length <= this._length;
            if (inRange && this._length > 0 && length == 0)
                return CString.Empty;
            else if (!inRange)
                throw new ArgumentOutOfRangeException(nameof(range));
            return new(this, offset, length);
        }
    }
}