namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
    /// <summary>
    /// This class represents a memory region provided by a function.
    /// </summary>
    private sealed class FuncRegion : ValueRegion<T>
    {
        /// <summary>
        /// The internal function that provides the memory region.
        /// </summary>
        private readonly ReadOnlySpanFunc<T> _func;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="func">The internal function that provides the memory region.</param>
        public FuncRegion(ReadOnlySpanFunc<T> func)
        {
            this._func = func;
        }

        /// <summary>
        /// Gets the memory region provided by the function.
        /// </summary>
        /// <returns>The memory region provided by the function.</returns>
        public ReadOnlySpanFunc<T> AsReadOnlySpanFunc() => this._func;

        /// <inheritdoc/>
        public override ValueRegion<T> Slice(Int32 startIndex)
        {
            Int32 regionLength = this._func().Length;
            Int32 length = regionLength - startIndex;
            ValidationUtilities.ThrowIfInvalidSubregion(regionLength, startIndex, length);
            return this.InternalSlice(startIndex, length);
        }
        /// <inheritdoc/>
        public override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
        {
            Int32 regionLength = this._func().Length;
            ValidationUtilities.ThrowIfInvalidSubregion(regionLength, startIndex, length);
            return this.InternalSlice(startIndex, length);
        }

        /// <inheritdoc/>
        internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
            => new FuncMemorySlice(this, startIndex, length);
        /// <inheritdoc/>
        internal override ReadOnlySpan<T> AsSpan() => this._func();
    }

}
