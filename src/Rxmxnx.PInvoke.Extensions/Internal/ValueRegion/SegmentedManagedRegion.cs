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
            /// Segment array range.
            /// </summary>
            private readonly Range _range;
            /// <summary>
            /// Indicates whether current instance is segmented.
            /// </summary>
            private readonly Boolean _isSegmented;

            /// <summary>
            /// Gets an item from the memory region at the specified zero-based <paramref name="index"/>.
            /// </summary>
            /// <param name="index">The zero-based index of the element to get.</param>
            /// <exception cref="IndexOutOfRangeException">
            /// <paramref name="index"/> is less then zero or greater than or equal to 
            /// memory region length.
            /// </exception>
            /// <returns>The element from the memory region.</returns>
            public override T this[Int32 index] => this._array[index];

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="region"><see cref="ManagedRegion"/> instance.</param>
            /// <param name="range">Segment array range.</param>
            public SegmentedManagedRegion([DisallowNull] ManagedRegion region, Range range)
            {
                Int32 start = range.Start.Value;
                Int32 length = range.End.Value - start;

                this._array = region.AsArray()!;
                this._range = CalculateRange(region, start, length, out this._isSegmented);
            }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="region"><see cref="ManagedRegion"/> instance.</param>
            /// <param name="range">Segment array range.</param>
            public SegmentedManagedRegion([DisallowNull] SegmentedManagedRegion region, Range range)
            {
                Int32 start = region._range.Start.Value + range.Start.Value;
                Int32 length = range.End.Value - range.Start.Value;
                this._array = region._array;

                this._range = CalculateRange(region, start, length, out this._isSegmented);
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
            protected override ReadOnlySpan<T> AsSpan() => this._array.AsSpan()[this._range];


            /// <summary>
            /// Calculates range for a new <see cref="SegmentedManagedRegion"/> instance.
            /// </summary>
            /// <param name="span">Internal <see cref="ReadOnlySpan{T}"/> instance.</param>
            /// <param name="start">Start of requested range.</param>
            /// <param name="length">Length in requested range.</param>
            /// <param name="isSegmented">Output. Indicates whether output range represents segment of <paramref name="span"/>.</param>
            /// <returns>The range for the new <see cref="SegmentedManagedRegion"/> instance.</returns>
            private static Range CalculateRange(ReadOnlySpan<T> span, Int32 start, Int32 length, out Boolean isSegmented)
            {
                if (span.Length - start > length && span[start + length].Equals(default(T)))
                    length++;

                isSegmented = (start != default || length != span.Length);
                return new(start, start + length);
            }
        }
    }
}
