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
            /// <summary>
            /// Indicates whether current instance is segmented.
            /// </summary>
            private readonly Boolean _isSegmented;

            /// <inheritdoc/>
            public override Boolean IsSegmented => this._isSegmented;
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
                this._isSegmented = offset != default || this._end != region.AsArray()!.Length;
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
                this._isSegmented = offset != default || length != region._array.Length;
            }

            /// <summary>
            /// Gets an array from this memory region.
            /// </summary>
            /// <returns>An array containing the data in the current memory region.</returns>
            protected override T[]? AsArray() => !this._isSegmented ? this._array : default;

            /// <summary>
            /// Creates a new read-only span over this memory region.
            /// </summary>
            /// <returns>The read-only span representation of the memory region.</returns>
            protected override ReadOnlySpan<T> AsSpan() => this._array.AsSpan()[this._offset..this._end];
        }
    }
}
