namespace Rxmxnx.PInvoke;

public partial class CString
{
    /// <summary>
    /// Private constructor.
    /// </summary>
    /// <param name="bytes">Binary internal information.</param>
    private CString([DisallowNull] Byte[] bytes)
    {
        this._isLocal = true;
        this._data = ValueRegion<Byte>.Create(bytes);
        this._isNullTerminated = bytes.Any() && bytes[^1] == default;
        this._length = bytes.Length - (this._isNullTerminated ? 1 : 0);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="func"><see cref="ReadOnlySpanFunc{Byte}"/> delegate.</param>
    /// <param name="isLiteral">Indicates whether returned span is from UTF-8 literal.</param>
    private CString(ReadOnlySpanFunc<Byte> func, Boolean isLiteral)
    {
        this._isLocal = false;
        this._isFunction = true;
        this._data = ValueRegion<Byte>.Create(func);

        ReadOnlySpan<Byte> data = func();
        Boolean isNullTerminatedSpan = IsNullTerminatedSpan(data);
        this._isNullTerminated = isLiteral || isNullTerminatedSpan;
        this._length = data.Length - (isNullTerminatedSpan ? 1 : 0);
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="value">A <see cref="CString"/> value.</param>
    /// <param name="offset">Offset for range.</param>
    /// <param name="length">Length of range.</param>
    private CString(CString value, Int32 offset, Int32 length)
    {
        this._isLocal = value._isLocal;
        this._isFunction = value._isFunction;
        this._data = ValueRegion<Byte>.Create(value._data, offset, value.GetDataLength(offset, length));

        ReadOnlySpan<Byte> data = this._data;
        Boolean isNullTerminatedSpan = IsNullTerminatedSpan(data);
        Boolean isLiteral = value._isFunction && value._isNullTerminated;
        this._isNullTerminated = isLiteral && value._length - offset == length || isNullTerminatedSpan;
        this._length = data.Length - (isNullTerminatedSpan ? 1 : 0);
    }
}