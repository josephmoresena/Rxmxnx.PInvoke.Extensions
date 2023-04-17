namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a <see cref="CStringSequence"/> fixed in memory.
/// </summary>
public unsafe readonly ref struct FixedCStringSequence
{
    /// <summary>
    /// <see cref="CString"/> values.
    /// </summary>
    private readonly CString? _value;
    /// <summary>
    /// <see cref="CString"/> values.
    /// </summary>
    private readonly CString[]? _values;

    /// <summary>
    /// <see cref="CString"/> values.
    /// </summary>
    public IReadOnlyList<CString> Values => this._values ?? Array.Empty<CString>();

    /// <summary>
    /// Gets the element at the given index.
    /// </summary>
    /// <param name="index">A position in the current instance.</param>
    /// <returns>The object at position <paramref name="index"/>.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="index"/> is greater than or equal to the length of this object or less than zero.
    /// </exception>
    public IReadOnlyFixedContext<Byte> this[Int32 index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (index < 0 || index >= this.Values.Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index was outside the bounds of the sequence.");
            return this.GetFixedCString(index);
        }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="values"><see cref="CString"/> values.</param>
    /// <param name="value">Internal <see cref="CString"/> value.</param>
    internal FixedCStringSequence(CString[] values, CString value)
    {
        this._values = values;
        this._value = value;
    }

    /// <summary>
    /// Retrieves the <see cref="IFixedContext{Byte}"/> for element at <paramref name="index"/>.
    /// </summary>
    /// <param name="index"></param>
    /// <returns><see cref="IFixedContext{Byte}"/> for element at <paramref name="index"/>.</returns>
    private IReadOnlyFixedContext<Byte> GetFixedCString(Int32 index)
    {
        CString cstr = this._values![index];
        fixed (void* ptr = cstr.AsSpan())
            return new FixedContext<Byte>(ptr, cstr.Length, true);
    }

    /// <inheritdoc/>
    public override String? ToString() => this._value?.ToString();
}
