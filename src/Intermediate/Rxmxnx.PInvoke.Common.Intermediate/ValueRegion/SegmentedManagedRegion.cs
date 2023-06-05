namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
    /// <summary>
    /// This class represents a memory region in which an segmented array of <typeparamref name="T"/> 
    /// values is found.
    /// </summary>
    private sealed class SegmentedManagedRegion : SegmentedRegion
    {
        /// <summary>
        /// Internal <typeparamref name="T"/> array.
        /// </summary>
        private readonly T[] _array;

        /// <inheritdoc/>
        public override T this[Int32 index] => this._array[base._offset + index];

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="region"><see cref="ManagedRegion"/> instance.</param>
        /// <param name="offset">Offset for range.</param>
        /// <param name="length">Length of range.</param>
        public SegmentedManagedRegion([DisallowNull] ManagedRegion region, Int32 offset, Int32 length)
            : base(region.AsArray()!.Length, offset, length)
        {
            this._array = region.AsArray()!;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="region"><see cref="ManagedRegion"/> instance.</param>
        /// <param name="offset">Offset for range.</param>
        /// <param name="length">Length of range.</param>
        public SegmentedManagedRegion([DisallowNull] SegmentedManagedRegion region, Int32 offset, Int32 length)
            : base(region._array.Length, offset, length, region._offset)
        {
            this._array = region._array;
        }

        /// <inheritdoc/>
        protected override T[]? AsArray() => !this.IsSegmented ? this._array : default;

        /// <inheritdoc/>
        internal override ReadOnlySpan<T> AsSpan() => this._array.AsSpan()[base._offset..base._end];

        /// <inheritdoc/>
        internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
            => new SegmentedManagedRegion(this, startIndex, length);
    }
}
