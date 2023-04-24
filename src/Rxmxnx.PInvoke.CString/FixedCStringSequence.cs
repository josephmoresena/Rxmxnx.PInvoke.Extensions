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
    /// Indicates whether current instance remains valid.
    /// </summary>
    private readonly IMutableWrapper<Boolean>? _isValid;

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
    public IReadOnlyFixedMemory this[Int32 index]
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
        this._isValid = IMutableWrapper<Boolean>.Create(true);
    }

    /// <summary>
    /// Creates an array of <see cref="IReadOnlyFixedMemory"/> instances from current instance.
    /// </summary>
    /// <returns>An array of <see cref="IReadOnlyFixedMemory"/> instances.</returns>
    public IReadOnlyFixedMemory[] ToArray()
    {
        IReadOnlyFixedMemory[] result = new IReadOnlyFixedMemory[this.Values.Count];
        for (Int32 i = 0; i < result.Length; i++)
            result[i] = this[i];
        return result;
    }

    /// <summary>
    /// Invalidates current context.
    /// </summary>
    public void Unload()
    {
        if (this._isValid is not null)
            this._isValid.Value = false;
    }

    /// <inheritdoc/>
    public override String? ToString() => this._value?.ToString();

    /// <summary>
    /// Retrieves the <see cref="IFixedContext{Byte}"/> for element at <paramref name="index"/>.
    /// </summary>
    /// <param name="index"></param>
    /// <returns><see cref="IFixedContext{Byte}"/> for element at <paramref name="index"/>.</returns>
    private IReadOnlyFixedContext<Byte> GetFixedCString(Int32 index)
    {
        CString cstr = this._values![index];
        fixed (void* ptr = cstr.AsSpan())
            return new FixedContext<Byte>(ptr, cstr.Length, true, this._isValid!);
    }
}
