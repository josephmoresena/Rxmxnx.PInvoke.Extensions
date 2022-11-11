using System;
using System.Diagnostics.CodeAnalysis;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Context from read-only memory block fixing.
    /// </summary>
    /// <typeparam name="T">Type of fixed memory block.</typeparam>
    public class ReadOnlyFixedContext<T> : FixedContextBase where T : unmanaged
    {
        /// <summary>
        /// Internal pointer to fixed read-only memory block.
        /// </summary>
        private readonly unsafe void* _ptr;
        /// <summary>
        /// Length of fixed memory block.
        /// </summary>
        private readonly Int32 _length;

        /// <summary>
        /// A read-only span to fixed memory block.
        /// </summary>
        public ReadOnlySpan<T> Values
        {
            get
            {
                unsafe
                {
                    this.Validate();
                    return new ReadOnlySpan<T>(this._ptr, this._length);
                }
            }
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="ptr">Pointer to fixed read-only memory block.</param>
        /// <param name="length">Amount of <typeparamref name="T"/> in memory block.</param>
        /// <param name="isValid">Indicator for current instance validation.</param>
        internal unsafe ReadOnlyFixedContext(void* ptr, Int32 length, IMutableWrapper<Boolean>? isValid = default)
            : base(isValid)
        {
            this._ptr = ptr;
            this._length = length;
        }

        /// <inheritdoc/>
        [return: NotNullIfNotNull(nameof(ctx))]
        public static implicit operator ReadOnlyFixedContext<T>?(FixedContext<T>? ctx) => ctx?.AsReadOnly();
    }

    /// <summary>
    /// Context from memory block fixing.
    /// </summary>
    /// <typeparam name="T">Type of fixed memory block.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    public class ReadOnlyFixedContext<T, TArg> : ReadOnlyFixedContext<T> where T : unmanaged
    {
        /// <summary>
        /// Internal object state.
        /// </summary>
        private readonly TArg _state;

        /// <summary>
        /// A state object of type <typeparamref name="TArg"/>.
        /// </summary>
        public TArg State => this._state;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="ptr">Pointer to fixed memory block.</param>
        /// <param name="length">Amount of <typeparamref name="T"/> in memory block.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="isValid">Indicator for current instance validation.</param>
        internal unsafe ReadOnlyFixedContext(void* ptr, Int32 length, in TArg arg, IMutableWrapper<Boolean>? isValid = default) : base(ptr, length, isValid)
        {
            this._state = arg;
        }

        /// <inheritdoc/>
        [return: NotNullIfNotNull(nameof(ctx))]
        public static implicit operator ReadOnlyFixedContext<T, TArg>?(FixedContext<T, TArg>? ctx) => ctx?.AsReadOnly();
    }
}
