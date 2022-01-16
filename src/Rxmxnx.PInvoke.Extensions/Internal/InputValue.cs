using System;
using System.Runtime.CompilerServices;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    /// <summary>
    /// Creates an object which contains a single reference to an inmutable <typeparamref name="T"/> object.
    /// </summary>
    /// <typeparam name="T">Type of the referenced object.</typeparam>
    internal abstract record InputValue<T> : IReferenceable<T>
    {
        /// <summary>
        /// Internal <typeparamref name="T"/> object.
        /// </summary>
        protected T _instance;

        /// <summary>
        /// Reference to instance <typeparamref name="T"/> object.
        /// </summary>
        public ref readonly T Reference => ref this._instance;

        /// <summary>
        /// Wrapped <typeparamref name="T"/> object.
        /// </summary>
        public T Value => this._instance;

        /// <summary>
        /// Indicates whether the current object is equal to <typeparamref name="T"/> object.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the other parameter; otherwise, <see langword="false"/>.
        /// </returns>
        public Boolean Equals(T? other)
            => Object.Equals(this._instance, other);

        /// <summary>
        /// Indicates whether the current object is equal to <see cref="IReferenceable{T}"/> object.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the other parameter; otherwise, <see langword="false"/>.
        /// </returns>
        public Boolean Equals(IReferenceable<T>? other)
            => other != default && Unsafe.AreSame(ref Unsafe.AsRef(this.Reference), ref Unsafe.AsRef(other.Reference));

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="instance">Initial value.</param>
        protected InputValue(in T instance) => this._instance = instance;

        /// <summary>
        /// Internal method to set instance object.
        /// </summary>
        /// <param name="newValue">New <typeparamref name="T"/> object to set as instance object.</param>
        internal abstract void SetInstance(in T newValue);
    }
}
