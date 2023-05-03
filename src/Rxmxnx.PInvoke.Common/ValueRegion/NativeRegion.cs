namespace Rxmxnx.PInvoke;

public partial class ValueRegion<T>
{
    /// <summary>
    /// This class represents a native memory region in which a sequence of <typeparamref name="T"/> 
    /// values is found.
    /// </summary>
    private sealed class NativeRegion : ValueRegion<T>
    {
        /// <summary>
        /// <see cref="NativeRegion"/> empty instance.
        /// </summary>
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
        /// Constructor.
        /// </summary>
        /// <param name="region"><see cref="NativeRegion"/> instance.</param>
        /// <param name="offset">Offset for range.</param>
        /// <param name="length">Length of range.</param>
        private unsafe NativeRegion(NativeRegion region, Int32 offset, Int32 length)
        {
            T* tPtr = region.GetElementPointer(offset);
            this._ptr = new(tPtr);
            this._length = length;
        }

        /// <inheritdoc/>
        public override ValueRegion<T> Slice(Int32 startIndex)
            => this.Slice(startIndex, this._length - startIndex);

        /// <inheritdoc/>
        public override ValueRegion<T> Slice(Int32 startIndex, Int32 length)
        {
            ThrowSubregionArgumentOutOfRange(this._length, startIndex, length);
            return this.InternalSlice(startIndex, length);
        }

        /// <inheritdoc/>
        internal override unsafe ReadOnlySpan<T> AsSpan()
        {
            void* pointer = this._ptr.ToPointer();
            return new(pointer, this._length);
        }

        /// <inheritdoc/>
        internal override ValueRegion<T> InternalSlice(Int32 startIndex, Int32 length)
            => length != 0 ? new NativeRegion(this, startIndex, length) : Empty;

        /// <summary>
        /// Retrieves the pointer of the element at given index.
        /// </summary>
        /// <param name="index">Elemnt index.</param>
        /// <returns>The pointer of the element at given index.</returns>
        private unsafe T* GetElementPointer(Int32 index)
        {
            T* tPtr = (T*)this._ptr.ToPointer();
            tPtr += index;
            return tPtr;
        }
    }
}
