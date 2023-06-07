namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Represents a list of <see cref="FixedMemory"/> instances.
/// </summary>
internal readonly struct FixedMemoryListValue : IEnumerableSequence<FixedMemory>
{
    /// <summary>
    /// Array of <see cref="FixedMemory"/> instances.
    /// </summary>
    private readonly FixedMemory[]? _memories;

    /// <summary>
    /// Gets the total number of elements in the list.
    /// </summary>
    public Int32 Count => this._memories?.Length ?? default;
    /// <summary>
    /// Gets the element at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the element to get.</param>
    /// <returns>The element at the specified index.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// Thrown when <paramref name="index"/> is not a valid index in the list.
    /// </exception>
    public FixedMemory this[Int32 index]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ValidationUtilities.ThrowIfInvalidListIndex(index, this.Count);
            return this._memories![index];
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FixedMemoryListValue"/> struct.
    /// </summary>
    /// <param name="memories">The array of <see cref="FixedMemory"/> instances.</param>
    public FixedMemoryListValue(FixedMemory[] memories)
    {
        this._memories = memories;
    }

    /// <summary>
    /// Releases all resources used by the <see cref="FixedMemory"/> instances in the list.
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
