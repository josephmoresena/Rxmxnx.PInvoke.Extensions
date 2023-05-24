namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a read-only list of <see cref="IFixedMemory"/> instances.
/// </summary>
public unsafe readonly ref struct FixedMemoryList
{
    /// <summary>
    /// <see cref="FixedMemoryListValue"/> value.
    /// </summary>
    private readonly FixedMemoryListValue _values;

    /// <summary>
    /// Gets the total number of elements in the list. 
    /// </summary>
    public Int32 Count => this._values.Count;

    /// <summary>
    /// Gets the element at the given index.
    /// </summary>
    /// <param name="index">A position in the current instance.</param>
    /// <returns>The object at position <paramref name="index"/>.</returns>
    /// <exception cref="IndexOutOfRangeException">
    /// <paramref name="index"/> is greater than or equal to the length of this object or less than zero.
    /// </exception>
    public IFixedMemory this[Int32 index] => this._values[index];

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="memories"><see cref="FixedMemory"/> values.</param>
    internal FixedMemoryList(params FixedMemory[] memories)
    {
        this._values = new(memories);
    }

    /// <summary>
    /// Creates an array of <see cref="IReadOnlyFixedMemory"/> instances from current instance.
    /// </summary>
    /// <returns>An array of <see cref="IReadOnlyFixedMemory"/> instances.</returns>
    public IFixedMemory[] ToArray() => _values.ToArray();

    /// <summary>
    /// Gets an enumerator for this list.
    /// </summary>
    /// <returns>Enumerator instance for current list.</returns>
    public Enumerator GetEnumerator() => new(this._values);

    /// <summary>
    /// Invalidates all elements of the current list.
    /// </summary>
    internal void Unload() => this._values.Unload();

    /// <summary>
    /// Implicit operator <see cref="ReadOnlyFixedMemoryList"/> -> <see cref="FixedMemoryList"/>.
    /// </summary>
    /// <param name="fml">A <see cref="FixedMemoryList"/> instance.</param>
    public static implicit operator ReadOnlyFixedMemoryList(FixedMemoryList fml) => new(fml._values);

    /// <summary>
    /// Enumerates the elements of a <see cref="FixedMemoryList"/>.
    /// </summary>
    public readonly ref struct Enumerator
    {
        /// <summary>
        /// Internal enumerator.
        /// </summary>
        private readonly IEnumerator<FixedMemory> _enumerator;

        /// <summary>
        /// Gets the element at the current position of the enumerator.
        /// </summary>
        public IFixedMemory Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return this._enumerator.Current;
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="values">A <see cref="FixedMemory"/> enumerable instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(IEnumerable<FixedMemory> values)
        {
            this._enumerator = values.GetEnumerator();
        }

        /// <summary>
        /// Advances the enumerator to the next element of the list.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> if the enumerator was able to advance; otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Boolean MoveNext() => this._enumerator.MoveNext();
    }
}