namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
    /// <summary>
    /// Gets the element at the given index.
    /// </summary>
    /// <param name="index">A position in the current instance.</param>
    /// <returns>The object at position <paramref name="index"/>.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="index"/> is greater than or equal to the length of this object or less than zero.
    /// </exception>
    public CString this[Int32 index]
    {
        get
        {
            if (index < 0 || index >= this._lengths.Length)
                throw new ArgumentOutOfRangeException(nameof(index), "Index was out of range. Must be non-negative and less than the size of the collection.");
            return new CString(() => this.GetBinarySpan(index));
        }
    }

    /// <summary>
    /// Retrieves the binary span for given index.
    /// </summary>
    /// <param name="index">A position in the current instance.</param>
    /// <returns>Binary span for given index.</returns>
    private ReadOnlySpan<Byte> GetBinarySpan(Int32 index)
    {
        Int32 binaryOffset = this._lengths[..index].Sum();
        Int32 binaryLength = this._lengths[index];
        return MemoryMarshal.Cast<Char, Byte>(this._value)[binaryOffset..binaryLength];
    }
}

