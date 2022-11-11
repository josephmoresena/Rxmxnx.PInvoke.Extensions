using System;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Context from memory block fixing.
    /// </summary>
    /// <typeparam name="T">Type of fixed memory block.</typeparam>
    public class ReadOnlyTransformationContext<T> : FixedContextBase where T : unmanaged
    {
        /// <summary>
        /// Principal fixed context.
        /// </summary>
        protected readonly ReadOnlyFixedContext<T> _principal;
        /// <summary>
        /// Residual fixed context.
        /// </summary>
        protected readonly ReadOnlyFixedContext<Byte> _residual;

        /// <summary>
        /// A read-only span to fixed memory block.
        /// </summary>
        public ReadOnlySpan<T> Values => this._principal.Values;
        /// <summary>
        /// A span to fixed residual memory block.
        /// </summary>
        public ReadOnlySpan<Byte> Residual => this._residual.Values;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="ptr">Pointer to fixed memory block.</param>
        /// <param name="length">Amount of <typeparamref name="T"/> in memory block.</param>
        /// <param name="rPtr">Pointer to residual fixed memory block.</param>
        /// <param name="rLength">Amount of <see cref="Byte"/> in residual memory block.</param>
        /// <param name="isValid">Indicator for current instance validation.</param>
        internal unsafe ReadOnlyTransformationContext(void* ptr, Int32 length, void* rPtr, Int32 rLength, IMutableWrapper<Boolean>? isValid = default)
            : base(isValid)
        {
            this._principal = new(ptr, length, GetIsValid(this));
            this._residual = new(rPtr, rLength, GetIsValid(this));
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="principal">A <see cref="ReadOnlyFixedContext{T}"/> instance.</param>
        /// <param name="rPtr">Pointer to residual fixed memory block.</param>
        /// <param name="rLength">Amount of <see cref="Byte"/> in residual memory block.</param>
        internal unsafe ReadOnlyTransformationContext(ReadOnlyFixedContext<T> principal, void* rPtr, Int32 rLength)
            : base(GetIsValid(principal))
        {
            this._principal = principal;
            this._residual = new(rPtr, rLength, GetIsValid(this));
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="principal">A <see cref="ReadOnlyFixedContext{T}"/> instance.</param>
        /// <param name="residual">A <see cref="ReadOnlyFixedContext{Byte}"/> instance.</param>
        internal unsafe ReadOnlyTransformationContext(ReadOnlyFixedContext<T> principal, ReadOnlyFixedContext<Byte> residual)
            : base(GetIsValid(principal))
        {
            this._principal = principal;
            this._residual = residual;
        }
    }

    /// <summary>
    /// Context from memory block fixing.
    /// </summary>
    /// <typeparam name="T">Type of fixed memory block.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    public class ReadOnlyTransformationContext<T, TArg> : ReadOnlyTransformationContext<T> where T : unmanaged
    {
        /// <summary>
        /// A state object of type <typeparamref name="TArg"/>.
        /// </summary>
        public TArg State => (this._principal as FixedContext<T, TArg>)!.State;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="ptr">Pointer to fixed memory block.</param>
        /// <param name="length">Amount of <typeparamref name="T"/> in memory block.</param>
        /// <param name="rPtr">Pointer to residual fixed memory block.</param>
        /// <param name="rLength">Amount of <see cref="Byte"/> in residual memory block.</param>
        /// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
        /// <param name="isValid">Indicator for current instance validation.</param>
        internal unsafe ReadOnlyTransformationContext(void* ptr, Int32 length, void* rPtr, Int32 rLength, in TArg arg, IMutableWrapper<Boolean>? isValid = default)
            : base(new ReadOnlyFixedContext<T, TArg>(ptr, length, arg, isValid), rPtr, rLength) { }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="principal">A <see cref="ReadOnlyFixedContext{T}"/> instance.</param>
        /// <param name="residual">A <see cref="ReadOnlyFixedContext{Byte}"/> instance.</param>
        internal unsafe ReadOnlyTransformationContext(ReadOnlyFixedContext<T, TArg> principal, ReadOnlyFixedContext<Byte> residual)
            : base(principal, residual) { }
    }
}
