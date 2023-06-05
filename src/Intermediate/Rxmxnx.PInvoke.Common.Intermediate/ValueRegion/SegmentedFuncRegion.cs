namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
    /// <summary>
    /// This class represents a function segmended memory region that returns a <see cref="ReadOnlySpan{T}"/>
    /// instance.
    /// </summary>
    private sealed class SegmentedFuncRegion : SegmentedRegion
    {
        /// <summary>
        /// Internal <see cref="ReadOnlySpanFunc{T}"/> instance. 
        /// </summary>
        private readonly ReadOnlySpanFunc<T> _func;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="region"><see cref="FuncRegion"/> instance. </param>
        /// <param name="offset">Offset for range.</param>
        /// <param name="length">Length of range.</param>
        public SegmentedFuncRegion(FuncRegion region, Int32 offset, Int32 length)
            : base(region.AsReadOnlySpanFunc()!().Length, offset, length)
        {
            this._func = region.AsReadOnlySpanFunc()!;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="region"><see cref="ManagedRegion"/> instance.</param>
        /// <param name="offset">Offset for range.</param>
        /// <param name="length">Length of range.</param>
        public SegmentedFuncRegion([DisallowNull] SegmentedFuncRegion region, Int32 offset, Int32 length)
            : base(region._func().Length, offset, length, region._offset)
        {
            this._func = region._func;
        }

        /// <inheritdoc/>
        internal override ReadOnlySpan<T> AsSpan() => this._func()[base._offset..base._end];

        /// <inheritdoc/>
        internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
            => new SegmentedFuncRegion(this, startIndex, length);
    }
}
