namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
    /// <summary>
    /// This class represents a memory region in which an array of <typeparamref name="T"/> 
    /// values is found.
    /// </summary>
    private sealed class FuncRegion : ValueRegion<T>
    {
        /// <summary>
        /// Internal <see cref="ReadOnlySpanFunc{T}"/> instance. 
        /// </summary>
        private readonly ReadOnlySpanFunc<T> _func;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="func">Internal <see cref="ReadOnlySpanFunc{T}"/> instance. </param>
        public FuncRegion(ReadOnlySpanFunc<T> func) => this._func = func;

        /// <summary>
        /// Gets a function from this memory region.
        /// </summary>
        /// <returns>A function from this memory region.</returns>
        public ReadOnlySpanFunc<T>? AsReadOnlySpanFunc() => this._func;

        /// <inheritdoc/>
        public override ValueRegion<T> Slice(Int32 startIndex)
        {
            Int32 regionLength = this._func().Length;
            Int32 length = regionLength - startIndex;
            ThrowSubregionArgumentOutOfRange(regionLength, startIndex, length);
            return this.InternalSlice(startIndex, length);
        }

        /// <inheritdoc/>
        public override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
        {
            Int32 regionLength = this._func().Length;
            ThrowSubregionArgumentOutOfRange(regionLength, startIndex, length);
            return this.InternalSlice(startIndex, length);
        }

        /// <inheritdoc/>
        internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
            => new SegmentedFuncRegion(this, startIndex, length);

        /// <inheritdoc/>
        internal override ReadOnlySpan<T> AsSpan() => this._func();
    }
}
