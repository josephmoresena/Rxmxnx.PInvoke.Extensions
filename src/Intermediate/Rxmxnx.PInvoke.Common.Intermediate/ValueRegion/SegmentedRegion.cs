namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
    /// <summary>
    /// This class represents a segmented memory region in which an array of <typeparamref name="T"/> 
    /// values is found.
    /// </summary>
    private abstract class SegmentedRegion : ValueRegion<T>
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
        /// Indicates whether current instance is segmented.
        /// </summary>
        protected readonly Boolean _isSegmented;

        /// <inheritdoc/>
        public override Boolean IsSegmented => this._isSegmented;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="initialLength">Initial region length.</param>
        /// <param name="offset">Offset for range.</param>
        /// <param name="length">Length of range.</param>
        /// <param name="initialLength">Initial region offset.</param>
        public SegmentedRegion(Int32 initialLength, Int32 offset, Int32 length, Int32 initalOffset = 0)
		{
            this._offset = offset + initalOffset;
            this._end = this._offset + length;
            this._isSegmented = this._offset != default || this._end != initialLength;
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
