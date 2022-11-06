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
        private sealed class ManagedRegion : ValueRegion<T>
        {
            /// <summary>
            /// Internal <typeparamref name="T"/> array.
            /// </summary>
            private readonly T[] _array;

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
            /// <param name="array"><typeparamref name="T"/> array instance.</param>
            public ManagedRegion([DisallowNull] T[] array) => this._array = array;

            /// <summary>
            /// Gets an array from this memory region.
            /// </summary>
            /// <returns>An array containing the data in the current memory region.</returns>
            protected override T[]? AsArray() => this._array;
            /// <summary>
            /// Creates a new read-only span over this memory region.
            /// </summary>
            /// <returns>The read-only span representation of the memory region.</returns>
            protected override ReadOnlySpan<T> AsSpan() => this._array.AsSpan();
        }
    }
}
