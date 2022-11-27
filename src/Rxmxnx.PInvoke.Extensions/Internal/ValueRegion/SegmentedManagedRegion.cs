using System;
using System.Diagnostics.CodeAnalysis;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    internal abstract partial class ValueRegion<T>
    {
        /// <summary>
        /// This class represents a memory region in which an segmented array of <typeparamref name="T"/> 
        /// values is found.
        /// </summary>
        private sealed class SegmentedManagedRegion : ValueRegion<T>
        {
            /// <summary>
            /// Internal <typeparamref name="T"/> array.
            /// </summary>
            private readonly T[] _array;
            /// <summary>
            /// Internal offset.
            /// </summary>
            private readonly Int32 _offset;
            /// <summary>
            /// Internal length.
            /// </summary>
            private readonly Int32 _end;

            /// <inheritdoc/>
            public override Boolean IsSegmented => this._offset != default || this._end != this._array.Length;
            /// <inheritdoc/>
            public override T this[Int32 index] => this._array[this._offset + index];

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="region"><see cref="ManagedRegion"/> instance.</param>
            /// <param name="offset">Offset for range.</param>
            /// <param name="length">Length of range.</param>
            public SegmentedManagedRegion([DisallowNull] ManagedRegion region, Int32 offset, Int32 length)
            {
                this._array = region.AsArray()!;
                this._offset = offset;
                this._end = this._offset + length;
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="region"><see cref="ManagedRegion"/> instance.</param>
            /// <param name="offset">Offset for range.</param>
            /// <param name="length">Length of range.</param>
            public SegmentedManagedRegion([DisallowNull] SegmentedManagedRegion region, Int32 offset, Int32 length)
            {
                this._array = region._array;
                this._offset = offset + region._offset;
                this._end = this._offset + length;
            }

            /// <inheritdoc/>
            protected override T[]? AsArray() => !this.IsSegmented ? this._array : default;

            /// <inheritdoc/>
            protected override ReadOnlySpan<T> AsSpan() => this._array.AsSpan()[this._offset..this._end];
        }
    }
}
