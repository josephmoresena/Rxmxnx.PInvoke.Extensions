namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
    /// <summary>
    /// Internal buffer.
    /// </summary>
    private readonly String _value;
    /// <summary>
    /// Collection of text length for buffer interpretation.
    /// </summary>
    private readonly Int32?[] _lengths;

    /// <summary>
    /// Retrieves the buffer as an <see cref="ReadOnlySpan{Char}"/> instance and creates a
    /// <see cref="CString"/> array which represents text sequence.
    /// </summary>
    /// <param name="output"><see cref="CString"/> array.</param>
    /// <returns>Buffer <see cref="ReadOnlySpan{Char}"/>.</returns>
    private unsafe ReadOnlySpan<Char> AsSpanUnsafe(out CString[] output)
    {
        ReadOnlySpan<Char> result = this._value;
        ref Char firstCharRef = ref MemoryMarshal.GetReference(result);
        IntPtr ptr = new(Unsafe.AsPointer(ref firstCharRef));
        output = this.GetValues(ptr).ToArray();
        return this._value;
    }

    /// <summary>
    /// Retrieves the sequence of <see cref="CString"/> based on the buffer and lengths.
    /// </summary>
    /// <param name="ptr">Buffer pointer.</param>
    /// <returns>Collection of <see cref="CString"/>.</returns>
    private IEnumerable<CString> GetValues(IntPtr ptr)
    {
        Int32 offset = 0;
        for (Int32 i = 0; i < this._lengths.Length; i++)
        {
            Int32? length = this._lengths[i];
            if (length.HasValue && length.Value > 0)
            {
                yield return CString.CreateUnsafe(ptr + offset, length.Value + 1);
                offset += length.Value + 1;
            }
            else if (length.HasValue)
                yield return CString.Empty;
            else
                yield return CString.Zero;
        }
    }
}

