using System;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Base class of context from memory block fixing.
    /// </summary>
    public abstract class FixedContextBase
    {
        /// <summary>
        /// Indicates whether current instance remains valid.
        /// </summary>
        private readonly IMutableWrapper<Boolean> _isValid;

        /// <summary>
        /// Indicates whether current instance remains valid.
        /// </summary>
        public Boolean IsValid => this._isValid.Value;

        /// <summary>
        /// Internal constructor.
        /// </summary>
        /// <param name="isValid">Indicator for current instance validation.</param>
        internal FixedContextBase(IMutableWrapper<Boolean>? isValid = default) => this._isValid = isValid ?? InputValue.CreateReference(true);

        /// <summary>
        /// Validates current instance.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        protected void Validate()
        {
            if (!this._isValid.Value)
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Unloads current instance.
        /// </summary>
        internal void Unload() => this._isValid.SetInstance(false);

        /// <summary>
        /// Retrieves <see cref="IMutableWrapper{Boolean}"/> instance from <paramref name="ctx"/>.
        /// </summary>
        /// <param name="ctx"><see cref="FixedContextBase"/> instance.</param>
        /// <returns><see cref="IMutableWrapper{Boolean}"/> instance from <paramref name="ctx"/>.</returns>
        internal static IMutableWrapper<Boolean> GetIsValid(FixedContextBase ctx) => ctx._isValid;
    }
}
