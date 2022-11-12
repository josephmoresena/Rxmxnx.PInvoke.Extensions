using System;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Context from memory block fixing.
    /// </summary>
    /// <typeparam name="T">Type of fixed memory block.</typeparam>
    public class TransformationContext<T> : FixedContextBase where T : unmanaged
    {
        /// <summary>
        /// Principal fixed context.
        /// </summary>
        protected readonly FixedContext<T> _principal;
        /// <summary>
        /// Residual fixed context.
        /// </summary>
        protected readonly FixedContext<Byte> _residual;

        /// <summary>
        /// A span to fixed memory block.
        /// </summary>
        public Span<T> Values => this._principal.Values;
        /// <summary>
        /// A span to fixed residual memory block.
        /// </summary>
        public Span<Byte> Residual => this._residual.Values;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="ptr">Pointer to fixed memory block.</param>
        /// <param name="length">Amount of <typeparamref name="T"/> in memory block.</param>
        /// <param name="rPtr">Pointer to residual fixed memory block.</param>
        /// <param name="rLength">Amount of <see cref="Byte"/> in residual memory block.</param>
        /// <param name="isValid">Indicator for current instance validation.</param>
        internal unsafe TransformationContext(void* ptr, Int32 length, void* rPtr, Int32 rLength, IMutableWrapper<Boolean>? isValid = default)
            : base(isValid)
        {
            this._principal = new(ptr, length, GetIsValid(this));
            this._residual = new(rPtr, rLength, GetIsValid(this));
        }

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="principal">A <see cref="FixedContext{T}"/> instance.</param>
        /// <param name="rPtr">Pointer to residual fixed memory block.</param>
        /// <param name="rLength">Amount of <see cref="Byte"/> in residual memory block.</param>
        internal unsafe TransformationContext(FixedContext<T> principal, void* rPtr, Int32 rLength)
            : base(GetIsValid(principal))
        {
            this._principal = principal;
            this._residual = new(rPtr, rLength, GetIsValid(this));
        }

        /// <summary>
        /// Retrieves a <see cref="ReadOnlyFixedContext{T}"/> instance from current instance.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyFixedContext{T}"/> instance from current instance.</returns>
        internal virtual ReadOnlyTransformationContext<T> AsReadOnly()
        {
            unsafe
            {
                return new(this._principal, this._residual);
            }
        }
    }

    /// <summary>
    /// Context from memory block fixing.
    /// </summary>
    /// <typeparam name="T">Type of fixed memory block.</typeparam>
    /// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
    public sealed class TransformationContext<T, TArg> : TransformationContext<T> where T : unmanaged
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
        internal unsafe TransformationContext(void* ptr, Int32 length, void* rPtr, Int32 rLength, in TArg arg, IMutableWrapper<Boolean>? isValid = default)
            : base(new FixedContext<T, TArg>(ptr, length, arg, isValid), rPtr, rLength) { }

        /// <summary>
        /// Retrieves a <see cref="ReadOnlyFixedContext{T}"/> instance from current instance.
        /// </summary>
        /// <returns>A <see cref="ReadOnlyFixedContext{T}"/> instance from current instance.</returns>
        internal override ReadOnlyTransformationContext<T, TArg> AsReadOnly()
        {
            unsafe
            {
                return new((this._principal as FixedContext<T, TArg>)!, this._residual);
            }
        }
    }
}
