namespace Rxmxnx.PInvoke.Internal;

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

        /// <inheritdoc/>
        public override T this[Int32 index] => this._array[index];

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="array"><typeparamref name="T"/> array instance.</param>
        public ManagedRegion([DisallowNull] T[] array) => this._array = array;

        /// <inheritdoc/>
        protected override T[]? AsArray() => this._array;
        /// <inheritdoc/>
        internal override ReadOnlySpan<T> AsSpan() => this._array.AsSpan();
    }
}
