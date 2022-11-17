using System;
using System.Diagnostics.CodeAnalysis;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    internal partial class ValueRegion<T>
    {
        /// <summary>
        /// This class represents a memory region in which an array of <typeparamref name="T"/> 
        /// values is found.
        /// </summary>
        private sealed class SegmentedFuncRegion : ValueRegion<T>
        {
            /// <summary>
            /// Internal <see cref="ReadOnlySpanFunc{T}"/> instance. 
            /// </summary>
            private readonly ReadOnlySpanFunc<T> _func;
            /// <summary>
            /// Internal offset.
            /// </summary>
            private readonly Int32 _offset;
            /// <summary>
            /// Internal length.
            /// </summary>
            private readonly Int32 _end;
            /// <summary>
            /// Indicates whether current instance is segmented.
            /// </summary>
            private readonly Boolean _isSegmented;

            /// <inheritdoc/>
            public override Boolean IsSegmented => this._isSegmented;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="region"><see cref="FuncRegion"/> instance. </param>
            /// <param name="offset">Offset for range.</param>
            /// <param name="length">Length of range.</param>
            public SegmentedFuncRegion(FuncRegion region, Int32 offset, Int32 length)
            {
                this._func = region.AsReadOnlySpanFunc()!;
                this._offset = offset;
                this._end = this._offset + length;
                this._isSegmented = offset != default || length != this._func().Length;
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="region"><see cref="ManagedRegion"/> instance.</param>
            /// <param name="offset">Offset for range.</param>
            /// <param name="length">Length of range.</param>
            public SegmentedFuncRegion([DisallowNull] SegmentedFuncRegion region, Int32 offset, Int32 length)
            {
                this._func = region._func;
                this._offset = offset + region._offset;
                this._end = this._offset + length;
            }

            /// <inheritdoc/>
            protected override ReadOnlySpan<T> AsSpan() => this._func()[this._offset..this._end];
            /// <inheritdoc/>
            protected override ReadOnlySpanFunc<T>? AsReadOnlySpanFunc() => this._func;
        }
    }
}
