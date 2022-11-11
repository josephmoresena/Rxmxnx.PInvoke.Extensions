using System;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Context from memory block fixing.
    /// </summary>
    /// <typeparam name="T">Type of fixed memory block.</typeparam>
    public class FixedContext<T> : FixedContextBase where T : unmanaged
    {
        /// <summary>
        /// Internal pointer to fixed memory block.
        /// </summary>
        protected readonly unsafe void* _ptr;
        /// <summary>
        /// Length of fixed memory block.
        /// </summary>
        protected readonly Int32 _length;

        /// <summary>
        /// A span to fixed memory block.
        /// </summary>
        public Span<T> Values
        {
            get
            {
                this.Validate();
                unsafe
                {
                    return new Span<T>(this._ptr, this._length);
                }
            }
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="ptr">Pointer to fixed memory block.</param>
        /// <param name="length">Amount of <typeparamref name="T"/> in memory block.</param>
        /// <param name="isValid">Indicator for current instance validation.</param>
        internal unsafe FixedContext(void* ptr, Int32 length, IMutableWrapper<Boolean>? isValid = default)
            : base(isValid)
        {
            this._ptr = ptr;
            this._length = length;
        }

        /// <summary>
        /// Retrieves a <see cref="ReadOnlyFixedContext{T}"/> instance from current instance.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyFixedContext{T}"/> instance from current instance.</returns>
        internal virtual ReadOnlyFixedContext<T> AsReadOnly()
        {
            unsafe
            {
                return new(this._ptr, this._length, GetIsValid(this));
            }
        }
    }

    /// <summary>
    /// Context from memory block fixing.
    /// </summary>
    /// <typeparam name="T">Type of fixed memory block.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    public class FixedContext<T, TArg> : FixedContext<T> where T : unmanaged
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
        internal unsafe FixedContext(void* ptr, Int32 length, in TArg arg, IMutableWrapper<Boolean>? isValid = default) : base(ptr, length, isValid)
        {
            this._state = arg;
        }

        /// <summary>
        /// Retrieves a <see cref="ReadOnlyFixedContext{T}"/> instance from current instance.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyFixedContext{T}"/> instance from current instance.</returns>
        internal override ReadOnlyFixedContext<T, TArg> AsReadOnly()
        {
            unsafe
            {
                return new(this._ptr, this._length, this._state, GetIsValid(this));
            }
        }
    }
}
