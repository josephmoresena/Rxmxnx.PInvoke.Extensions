namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
    /// <summary>
    /// This class represents a memory slice provided by a function.
    /// </summary>
    private sealed class FuncMemorySlice : MemorySlice
    {
        /// <summary>
        /// The internal function that provides the memory region.
        /// </summary>
        private readonly ReadOnlySpanFunc<T> _func;

        /// <summary>
        /// Initializes a new instance of the <see cref="FuncMemorySlice"/> class from a subrange of an existing
        /// <see cref="FuncRegion"/>.
        /// </summary>
        /// <param name="region">A <see cref="FuncRegion"/> instance.</param>
        /// <param name="offset">The offset for the range.</param>
        /// <param name="length">The length of the range.</param>
        public FuncMemorySlice(FuncRegion region, Int32 offset, Int32 length)
            : base(region.AsReadOnlySpanFunc()().Length, offset, length)
        {
            this._func = region.AsReadOnlySpanFunc();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="FuncMemorySlice"/> class from a subrange of an existing
        /// <see cref="FuncMemorySlice"/>.
        /// </summary>
        /// <param name="region">A <see cref="FuncRegion"/> instance.</param>
        /// <param name="offset">The offset for the range.</param>
        /// <param name="length">The length of the range.</param>
        public FuncMemorySlice([DisallowNull] FuncMemorySlice region, Int32 offset, Int32 length)
            : base(region._func().Length, offset, length, region._offset)
        {
            this._func = region._func;
        }

        /// <inheritdoc/>
        internal override ReadOnlySpan<T> AsSpan() => this._func()[base._offset..base._end];
        /// <inheritdoc/>
        internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
            => new FuncMemorySlice(this, startIndex, length);
    }
}
