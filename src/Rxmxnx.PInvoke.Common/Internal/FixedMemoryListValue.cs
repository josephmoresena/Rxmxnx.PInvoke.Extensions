namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal <see cref="FixedMemory"/> list.
/// </summary>
internal readonly struct FixedMemoryListValue : IEnumerableSequence<FixedMemory>
{
    /// <summary>
    /// <see cref="FixedMemory"/> values.
    /// </summary>
    private readonly FixedMemory[]? _memories;

    /// <summary>
    /// Gets the total number of elements in the list. 
    /// </summary>
    public Int32 Count => this._memories?.Length ?? default;

    /// <summary>
    /// Gets the element at the given index.
    /// </summary>
    /// <param name="index">A position in the current instance.</param>
    /// <returns>The object at position <paramref name="index"/>.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="index"/> is greater than or equal to the length of this object or less than zero.
    /// </exception>
    public FixedMemory this[Int32 index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ValidationUtilities.ThrowIfInvalidListIndex(index, this.Count, nameof(index));
            return this._memories![index];
        }
    }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="memories"><see cref="FixedMemory"/> array.</param>
    public FixedMemoryListValue(FixedMemory[] memories)
    {
        this._memories = memories;
    }

    /// <summary>
    /// Invalidates all elements of the current list.
    /// </summary>
    public void Unload()
    {
        if (this._memories is not null)
            foreach (FixedMemory mem in this._memories)
                mem.Unload();
    }

    FixedMemory IEnumerableSequence<FixedMemory>.GetItem(Int32 index) => this[index];
    Int32 IEnumerableSequence<FixedMemory>.GetSize() => this.Count;
}
