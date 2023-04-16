namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a <see cref="CStringSequence"/> fixed in memory.
/// </summary>
public unsafe readonly ref struct FixedCStringSequence
{
    /// <summary>
    /// Pointer to fixed memory.
    /// </summary>
    private readonly void* _ptr;
    /// <summary>
    /// Memory size in bytes.
    /// </summary>
    private readonly Int32 _length;
    /// <summary>
    /// <see cref="CString"/> values.
    /// </summary>
    private readonly CString[]? _values;

    /// <summary>
    /// A read-only binary span over the fixed memory block.
    /// </summary>
    public ReadOnlySpan<Byte> Bytes => new(this._ptr, this._length);
    /// <summary>
    /// Pointer to fixed memory.
    /// </summary>
    public IntPtr Pointer => new(this._ptr);
    /// <summary>
    /// <see cref="CString"/> values.
    /// </summary>
    public IReadOnlyList<CString> Values => this._values ?? Array.Empty<CString>();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="ptr">Pointer to fixed memory.</param>
    /// <param name="length">Memory size in bytes.</param>
    /// <param name="values"><see cref="CString"/> values.</param>
    internal FixedCStringSequence(void* ptr, Int32 length, CString[] values)
    {
        this._ptr = ptr;
        this._length = length;
        this._values = values;
    }
}
