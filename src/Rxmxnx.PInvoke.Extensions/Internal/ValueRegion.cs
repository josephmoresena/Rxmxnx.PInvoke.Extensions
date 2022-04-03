using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    /// <summary>
    /// This class represents an arbitrary memory region in which a sequence of 
    /// <typeparamref name="T"/> values is found.
    /// </summary>
    /// <typeparam name="T">Unmanaged type of sequence item.</typeparam>
    internal abstract class ValueRegion<T> where T : unmanaged
    {
        /// <summary>
        /// Gets an item from the memory region at the specified zero-based <paramref name="index"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <exception cref="IndexOutOfRangeException">
        /// <paramref name="index"/> is less then zero or greater than or equal to 
        /// memory region length.
        /// </exception>
        /// <returns>The element from the memory region.</returns>
        [IndexerName("Position")]
        public virtual T this[Int32 index] => this.AsSpan()[index];

        /// <summary>
        /// Copies the contents of this memory region into a new array.
        /// </summary>
        /// <returns>An array containing the data in the current memory region.</returns>
        public virtual T[] ToArray() => this.AsSpan().ToArray();

        /// <summary>
        /// Gets an array from this memory region.
        /// </summary>
        /// <returns>An array containing the data in the current memory region.</returns>
        protected virtual T[]? AsArray() => default;
        /// <summary>
        /// Creates a new read-only span over this memory region.
        /// </summary>
        /// <returns>The read-only span representation of the memory region.</returns>
        protected abstract ReadOnlySpan<T> AsSpan();

        /// <summary>
        /// Returns an enumerator for this memory region.
        /// </summary>
        /// <returns>An enumerator for this memory region.</returns>
        public ReadOnlySpan<T>.Enumerator GetEnumerator() => this.AsSpan().GetEnumerator();

        /// <summary>
        /// Implicit operator. <see cref="ValueRegion{T}"/> -> <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <param name="arrayWrapper"><see cref="ValueRegion{T}"/> instance.</param>
        public static implicit operator ReadOnlySpan<T>(ValueRegion<T> arrayWrapper) => arrayWrapper.AsSpan();
        /// <summary>
        /// Implicit operator. <see cref="ValueRegion{T}"/> -> Array of <typeparamref name="T"/>.
        /// </summary>
        /// <param name="arrayWrapper"><see cref="ValueRegion{T}"/> instance.</param>
        public static implicit operator T[]?(ValueRegion<T> arrayWrapper) => arrayWrapper.AsArray();

        /// <summary>
        /// Creates a new <see cref="ValueRegion{T}"/> instance from an array of 
        /// <typeparamref name="T"/> values.
        /// </summary>
        /// <param name="array">Array of <typeparamref name="T"/> values.</param>
        /// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
        public static ValueRegion<T> Create([DisallowNull] T[] array) => new ManagedRegion(array);
        /// <summary>
        /// Creates a new <see cref="ValueRegion{T}"/> instance from a pointer to memory region and 
        /// the amount of values in sequence.
        /// </summary>
        /// <param name="ptr">Pointer to memory region.</param>
        /// <param name="count">Amount of values in sequence.</param>
        /// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
        public static ValueRegion<T> Create(IntPtr ptr, Int32 count)
            => !ptr.IsZero() && count != default ? new NativeRegion(ptr, count) : NativeRegion.Empty;

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
            protected override T[] AsArray() => this._array;
            /// <summary>
            /// Creates a new read-only span over this memory region.
            /// </summary>
            /// <returns>The read-only span representation of the memory region.</returns>
            protected override ReadOnlySpan<T> AsSpan() => this._array.AsSpan();
        }

        /// <summary>
        /// This class represents a native memory region in which a sequence of <typeparamref name="T"/> 
        /// values is found.
        /// </summary>
        private sealed class NativeRegion : ValueRegion<T>
        {
            public static readonly NativeRegion Empty = new(IntPtr.Zero, default);

            /// <summary>
            /// Pointer to native memory region.
            /// </summary>
            private readonly IntPtr _ptr;
            /// <summary>
            /// Length of <typeparamref name="T"/> sequence.
            /// </summary>
            private readonly Int32 _length;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="ptr">Pointer to native memory region.</param>
            /// <param name="length">Length of <typeparamref name="T"/> sequence.</param>
            public NativeRegion(IntPtr ptr, Int32 length)
            {
                this._ptr = ptr;
                this._length = !this._ptr.Equals(IntPtr.Zero) ? length : default;
            }

            /// <summary>
            /// Creates a new read-only span over this memory region.
            /// </summary>
            /// <returns>The read-only span representation of the memory region.</returns>
            protected override ReadOnlySpan<T> AsSpan() => this._ptr.AsReadOnlySpan<T>(this._length);
        }
    }
}
