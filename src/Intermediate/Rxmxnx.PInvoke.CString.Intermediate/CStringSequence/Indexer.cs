namespace Rxmxnx.PInvoke;

public partial class CStringSequence : IReadOnlyList<CString>, IEnumerableSequence<CString>
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            ValidationUtilities.ThrowIfInvalidSequenceIndex(index, this._lengths.Length);

            if (!this._lengths[index].HasValue)
                return CString.Zero;

            if (this._lengths[index].GetValueOrDefault() == 0)
                return CString.Empty;

            return new(() => this.GetBinarySpan(index));
        }
    }

    /// <summary>
    /// Gets the number of <see cref="CString"/> contained in <see cref="CStringSequence"/>.
    /// </summary>
    public Int32 Count => this._lengths.Length;

    /// <summary>
    /// Retrieves a subsequence from this instance.
    /// The subsequence starts at specified UTF-8 string position and continues to the
    /// end of the sequence.
    /// </summary>
    /// <param name="startIndex">
    /// The zero-based starting UTF-8 string position of a subsequence in this instance.
    /// </param>
    /// <returns>
    /// A <see cref="CStringSequence"/> that is equivalent to the subsequence that begins
    /// at <paramref name="startIndex"/> in this instance.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CStringSequence Slice(Int32 startIndex) => this[startIndex..this._lengths.Length];

    /// <summary>
    /// Retrieves a subsequence from this instance.
    /// The subsequence starts at a specified character position and has a specified length.
    /// </summary>
    /// <param name="startIndex">
    /// The zero-based starting UTF-8 string position of a subsequence in this instance.
    /// </param>
    /// <param name="length">The number of UTF-8 strings in the subsequence.</param>
    /// <returns>
    /// A <see cref="CStringSequence"/> that is equivalent to the subsequence of length
    /// <paramref name="length"/> that begins at <paramref name="startIndex"/> in this
    /// instance.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException"/>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public CStringSequence Slice(Int32 startIndex, Int32 length)
    {
        ValidationUtilities.ThrowIfInvalidSubsequence(this._lengths.Length, startIndex, length);

        if (length == 0)
            return empty;

        if (startIndex == 0 && length == this._lengths.Length)
            return this;

        return new SubsequenceHelper(this, startIndex, length).CreateSequence();
    }

    Int32 IEnumerableSequence<CString>.GetSize() => this._lengths.Length;

    /// <summary>
    /// Retrieves the binary span for given index.
    /// </summary>
    /// <param name="index">A position in the current instance.</param>
    /// <param name="count">
    /// The count of UTF-8 texts contained into the resulting span.
    /// </param>
    /// <returns>Binary span for given index.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private ReadOnlySpan<Byte> GetBinarySpan(Int32 index, Int32 count = 1)
    {
        ReadOnlySpan<Byte> span = MemoryMarshal.AsBytes<Char>(this._value);
        Int32 binaryOffset = this._lengths[..index].Sum(GetSpanLength);
        Int32 binaryLength = this._lengths.Skip(index).Take(count).Sum(GetSpanLength) - 1;
        return span.Slice(binaryOffset, binaryLength);
    }

    CString IEnumerableSequence<CString>.GetItem(Int32 index) => this[index];

    /// <summary>
    /// Helper class for subsequences creation.
    /// </summary>
    private sealed class SubsequenceHelper
    {
        /// <summary>
        /// Lengths in the subsequence.
        /// </summary>
        private readonly Int32?[] _lengths;
        /// <summary>
        /// Function to binary information of subsequence.
        /// </summary>
        private readonly ReadOnlySpanFunc<Byte> _function;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="sequence">Parent sequence.</param>
        /// <param name="startIndex">
        /// The zero-based starting UTF-8 string position of a subsequence in
        /// <paramref name="sequence"/>.
        /// </param>
        /// <param name="length">The number of UTF-8 strings in the subsequence.</param>
        public SubsequenceHelper(CStringSequence sequence, Int32 startIndex, Int32 length)
        {
            this._lengths = sequence._lengths.Skip(startIndex).Take(length).ToArray();
            this._function = () => sequence.GetBinarySpan(startIndex, length);
        }

        /// <summary>
        /// Creates a new <see cref="CStringSequence"/> instance from current helper instance.
        /// </summary>
        /// <returns>A new <see cref="CStringSequence"/> instance from current helper instance.</returns>
        public CStringSequence CreateSequence()
        {
            Int32 binaryLength = this._lengths.Sum(GetSpanLength);
            Int32 charLength = binaryLength / SizeOfChar + binaryLength % SizeOfChar;
            String value = String.Create(binaryLength, this, CopyBytes);
            return new(value, this._lengths);
        }

        /// <summary>
        /// Copies the source binary information from <paramref name="helper"/> into
        /// <paramref name="destination"/> span.
        /// </summary>
        /// <param name="destination">Span of <see cref="Char"/> values.</param>
        /// <param name="helper">
        /// Helper instance which contains the source binary information.
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CopyBytes(Span<Char> destination, SubsequenceHelper helper)
        {
            Span<Byte> destinationBytes = MemoryMarshal.AsBytes(destination);
            ReadOnlySpan<Byte> sourceBytes = helper._function();
            sourceBytes.CopyTo(destinationBytes);
        }
    }
}
