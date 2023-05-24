using Rxmxnx.PInvoke.Internal;

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
    /// <see cref="CString.Empty"/> handle.
    /// </summary>
    private readonly IMutableWrapper<GCHandle>? _emptyHandle;

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
        this._isValid = IMutableWrapper.Create(true);
        this._emptyHandle = IMutableWrapper.Create<GCHandle>();
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

    /// <inheritdoc/>
    public override String? ToString() => this._value?.ToString();

    /// <summary>
    /// Implicit operator <see cref="ReadOnlyFixedMemoryList"/> -> <see cref="FixedMemoryList"/>.
    /// </summary>
    /// <param name="fseq">A <see cref="FixedCStringSequence"/> instance.</param>
    public static implicit operator ReadOnlyFixedMemoryList(FixedCStringSequence fseq)
    {
        FixedMemory[] memories = new FixedMemory[fseq.Values.Count];
        for (Int32 i = 0; i < memories.Length; i++)
            memories[i] = fseq.GetFixedCString(i);
        return new(memories);
    }

    /// <summary>
    /// Invalidates current sequence.
    /// </summary>
    internal void Unload()
    {
        if (this._isValid is not null)
            this._isValid.Value = false;
        if (this.IsEmptyAllocated())
            this._emptyHandle!.Value.Free();
    }

    /// <summary>
    /// Retrieves the <see cref="FixedContext{Byte}"/> for element at <paramref name="index"/>.
    /// </summary>
    /// <param name="index"></param>
    /// <returns><see cref="FixedContext{Byte}"/> for element at <paramref name="index"/>.</returns>
    private FixedContext<Byte> GetFixedCString(Int32 index)
    {
        CString cstr = this._values![index];
        ReadOnlySpan<Byte> span = cstr;

        if (!cstr.IsReference)
        {
            if (!this.IsEmptyAllocated())
                this._emptyHandle!.Value = GCHandle.Alloc(CString.GetBytes(CString.Empty));
            span = (Byte[])this._emptyHandle!.Value.Target!;
        }

        fixed (void* ptr = &MemoryMarshal.GetReference(span))
            return new(ptr, cstr.Length, true, this._isValid!);
    }

    /// <summary>
    /// Indicates whether <see cref="CString.Empty"/> instance is allocated in memory.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if <see cref="CString.Empty"/> instance is allocated in memory;
    /// otherwise, <see langword="false"/>.
    /// </returns>
    private Boolean IsEmptyAllocated()
        => this._emptyHandle is not null && (IntPtr)this._emptyHandle.Value != IntPtr.Zero;
}
