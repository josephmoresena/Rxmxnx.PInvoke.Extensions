namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
    /// <summary>
    /// This class represents a memory region in which an array of <typeparamref name="T"/> 
    /// values is found.
    /// </summary>
    private sealed class ManagedRegion : ValueRegion<T>
    {
        /// <summary>
        /// Internal <typeparamref name="T"/> array.
        /// </summary>
        private readonly T[] _array;

        /// <inheritdoc/>
        public override T this[Int32 index] => this._array[index];

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="array"><typeparamref name="T"/> array instance.</param>
        public ManagedRegion([DisallowNull] T[] array) => this._array = array;

        /// <inheritdoc/>
        public override ValueRegion<T> Slice(Int32 startIndex)
            => this.Slice(startIndex, this._array.Length - startIndex);

        /// <inheritdoc/>
        public override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
        {
            ThrowSubregionArgumentOutOfRange(this._array.Length, startIndex, length);
            return this.InternalSlice(startIndex, length);
        }

        /// <inheritdoc/>
        internal override ReadOnlySpan<T> AsSpan() => this._array.AsSpan();

        /// <inheritdoc/>
        internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
            => new SegmentedManagedRegion(this, startIndex, length);

        /// <inheritdoc/>
        protected override T[]? AsArray() => this._array;
    }
}
