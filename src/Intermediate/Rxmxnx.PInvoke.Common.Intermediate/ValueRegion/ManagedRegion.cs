namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
    /// <summary>
    /// Represents a memory region that is contained within an array of array of
    /// <typeparamref name="T"/> values.
    /// </summary>
    private sealed class ManagedRegion : ValueRegion<T>
    {
        /// <summary>
        /// The internal array of <typeparamref name="T"/>.
        /// </summary>
        private readonly T[] _array;

        /// <inheritdoc/>
        public override T this[Int32 index] => this._array[index];

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedRegion"/> class.
        /// </summary>
        /// <param name="array">The array of <typeparamref name="T"/> values.</param>
        public ManagedRegion(T[] array)
        {
            this._array = array;
        }

        /// <inheritdoc/>
        public override ValueRegion<T> Slice(Int32 startIndex)
            => this.Slice(startIndex, this._array.Length - startIndex);
        /// <inheritdoc/>
        public override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
        {
            ValidationUtilities.ThrowIfInvalidSubregion(this._array.Length, startIndex, length);
            return this.InternalSlice(startIndex, length);
        }

        /// <inheritdoc/>
        internal override ReadOnlySpan<T> AsSpan() => this._array.AsSpan();
        /// <inheritdoc/>
        internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
            => new ManagedMemorySlice(this, startIndex, length);

        /// <inheritdoc/>
        [return: NotNull]
        protected override T[]? AsArray() => this._array;
    }
}