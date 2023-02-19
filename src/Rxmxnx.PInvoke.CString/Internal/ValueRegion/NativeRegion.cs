namespace Rxmxnx.PInvoke.Internal;

internal partial class ValueRegion<T>
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

        /// <inheritdoc/>
        internal override ReadOnlySpan<T> AsSpan()
        {
            unsafe
            {
                void* pointer = this._ptr.ToPointer();
                return new(pointer, this._length);
            }
        }
    }
}
