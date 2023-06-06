namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
    /// <summary>
    /// This class represents a memory slice that contains a sequence of <typeparamref name="T"/> values.
    /// </summary>
    private abstract class MemorySlice : ValueRegion<T>
    {
        /// <summary>
        /// Internal offset.
        /// </summary>
        protected readonly Int32 _offset;
        /// <summary>
        /// Internal length.
        /// </summary>
        protected readonly Int32 _end;
        /// <summary>
        /// Indicates whether the current instance represents a memory slice extracted from a larger memory region.
        /// </summary>
        protected readonly Boolean _slice;

        /// <inheritdoc/>
        public override Boolean IsMemorySlice => this._slice;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemorySlice"/> class.
        /// </summary>
        /// <param name="initialLength">The initial length of the memory slice.</param>
        /// <param name="offset">The offset for the memory slice.</param>
        /// <param name="length">The length of the memory slice.</param>
        /// <param name="initialOffset">The initial offset of the memory slice.</param>
        protected MemorySlice(Int32 initialLength, Int32 offset, Int32 length, Int32 initialOffset = 0)
        {
            this._offset = offset + initialOffset;
            this._end = this._offset + length;
            this._slice = this._offset != 0 || this._end != initialLength;
        }

        /// <inheritdoc/>
        public override ValueRegion<T> Slice(Int32 startIndex)
        {
            Int32 regionLength = this._end - this._offset;
            Int32 length = regionLength - startIndex;
            ValidationUtilities.ThrowIfInvalidSubregion(regionLength, startIndex, length);
            return this.InternalSlice(startIndex, length);
        }
        /// <inheritdoc/>
        public override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
        {
            Int32 regionLength = this._end - this._offset;
            ValidationUtilities.ThrowIfInvalidSubregion(regionLength, startIndex, length);
            return this.InternalSlice(startIndex, length);
        }
    }
}
