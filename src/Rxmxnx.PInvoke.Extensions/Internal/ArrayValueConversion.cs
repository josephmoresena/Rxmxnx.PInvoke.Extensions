using System;
using System.Runtime.CompilerServices;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    /// <summary>
    /// This class helps to create a <typeparamref name="TDestination"/> array from
    /// a <typeparamref name="TSource"/> array.
    /// </summary>
    /// <typeparam name="TDestination">Destination <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    /// <typeparam name="TSource">Origin <see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
    public class ArrayValueConversion<TSource, TDestination>
        where TSource : unmanaged
        where TDestination : unmanaged
    {
        /// <summary>
        /// Number of bytes from input.
        /// </summary>
        private readonly Int32 _inputBytes;
        /// <summary>
        /// Size of destination value.
        /// </summary>
        private readonly Int32 _valueSize;
        /// <summary>
        /// Count of readable destination values from input.
        /// </summary>
        private readonly Int32 _readables;
        /// <summary>
        /// Number of offset bytes from input.
        /// </summary>
        private readonly Int32 _offset;
        /// <summary>
        /// Conversion array.
        /// </summary>
        private readonly TDestination[] _values;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="input">The read-only span which servers as input.</param>
        public ArrayValueConversion(ReadOnlySpan<TSource> input)
        {
            unsafe
            {
                this._inputBytes = input.Length * sizeof(TSource);
                this._valueSize = sizeof(TDestination);
            }
            this._readables = this._inputBytes / _valueSize;
            this._offset = this._inputBytes % _valueSize;
            this._values = new TDestination[this._readables + this.GetOverflowElementsCount()];

            this.Initialize(input);
        }

        /// <summary>
        /// Gets the amount of elements which overflows the input length.
        /// </summary>
        /// <returns>The amount of elements which overflows the input length.</returns>
        private Int32 GetOverflowElementsCount() => this._offset != 0 ? 1 : 0;

        /// <summary>
        /// Copies the input information to the destination and performs the conversion.
        /// </summary>
        /// <param name="input">The read-only span which servers as input.</param>
        private void Initialize(ReadOnlySpan<TSource> input)
        {
            IntPtr ptr = input.AsIntPtr();
            Span<TDestination> finalSpan = this._values.AsSpan();
            ptr.AsReadOnlySpan<TDestination>(this._readables).CopyTo(finalSpan);
            if (this._offset != 0)
            {
                Span<Byte> missing = this._valueSize <= Byte.MaxValue ? stackalloc Byte[Byte.MaxValue] : new Byte[this._valueSize];
                ptr.AsReadOnlySpan<Byte>(this._inputBytes).Slice(this.GetOffsetStart(), this._offset).CopyTo(missing);
                unsafe
                {
                    fixed (void* missingPtr = missing)
                        finalSpan[^1] = Unsafe.Read<TDestination>(missingPtr);
                }
            }
        }

        /// <summary>
        /// Gets the start index of byte offset from input.
        /// </summary>
        /// <returns>The start index of byte offset from input..</returns>
        private Int32 GetOffsetStart() => this._inputBytes - this._offset;

        /// <summary>
        /// Implicit operator. <see cref="ArrayValueConversion{TSource, TDestination}"/> -> <typeparamref name="TDestination"/> array.
        /// </summary>
        /// <param name="conversion"><typeparamref name="TDestination"/> array.</param>
        public static implicit operator TDestination[](ArrayValueConversion<TSource, TDestination> conversion) => conversion._values;
    }
}
