namespace Rxmxnx.PInvoke;

public partial class CString : IEnumerableSequence<Byte>
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
    [IndexerName("Chars")]
    public Byte this[Int32 index] => this._data[index];

    /// <summary>
    /// Gets the number of bytes in the current <see cref="CString"/> object.
    /// </summary>
    /// <returns>
    /// The number of characters in the current string.
    /// </returns>
    public Int32 Length => this._length;

    /// <summary>
    /// Retrieves a substring from this instance.
    /// The substring starts at specified character position and continues to the end of the CString.
    /// </summary>
    /// <param name="startIndex">
    /// The zero-based starting character position of a substring in this instance.
    /// </param>
    /// <returns>
    /// A <see cref="CString"/> that is equivalent to the substring that begins at
    /// <paramref name="startIndex"/> in this instance, or <see cref="CString.Empty"/>
    /// if <paramref name="startIndex"/> is equal to the length of this instance.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public CString Slice(Int32 startIndex) => this[startIndex..this._length];

    /// <summary>
    /// Retrieves a substring from this instance.
    /// The substring starts at a specified character position and has a specified length.
    /// </summary>
    /// <param name="startIndex">
    /// The zero-based starting character position of a substring in this instance.
    /// </param>
    /// <param name="length">The number of characters in the substring.</param>
    /// <returns>
    /// A <see cref="CString"/> that is equivalent to the substring of length
    /// <paramref name="length"/> that begins at <paramref name="startIndex"/> in this
    /// instance, or <see cref="CString.Empty"/> if <paramref name="startIndex"/> is
    /// equal to the length of this instance and <paramref name="length"/> is zero.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public CString Slice(Int32 startIndex, Int32 length)
    {
        this.ThrowSubstringArgumentOutOfRange(startIndex, length);
        if (length == 0)
            return CString.Empty;

        if (startIndex == 0 && length == this._length)
            return this;

        return new(this, startIndex, length);
    }

    Int32 IEnumerableSequence<Byte>.GetSize() => this._length;

    Byte IEnumerableSequence<Byte>.GetItem(Int32 index) => this[index];

    /// <summary>
    /// Validates the input of the substring function.
    /// </summary>
    /// <param name="startIndex">
    /// The zero-based starting character position of a substring in this instance.
    /// </param>
    /// <param name="length">The number of characters in the substring.</param>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void ThrowSubstringArgumentOutOfRange(Int32 startIndex, Int32 length)
    {
        if (startIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(startIndex), "StartIndex cannot be less than zero.");

        if (startIndex > this._length)
            throw new ArgumentOutOfRangeException(nameof(startIndex), $"{nameof(startIndex)} cannot be larger than length of string.");

        if (length < 0)
            throw new ArgumentOutOfRangeException(nameof(length), "Length cannot be less than zero.");

        if (startIndex > this._length - length)
            throw new ArgumentOutOfRangeException(nameof(length), "Index and length must refer to a location within the string.");
    }
}