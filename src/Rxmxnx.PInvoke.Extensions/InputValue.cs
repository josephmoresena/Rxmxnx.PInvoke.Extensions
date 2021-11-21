using System;
using System.Diagnostics.CodeAnalysis;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Creates an object which contains a single reference to an inmutable <typeparamref name="T"/> object.
    /// </summary>
    /// <typeparam name="T">Type of the referenced object.</typeparam>
    public abstract record InputValue<T> : IReferenceable<T>
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
        /// Internal property to set instance object.
        /// </summary>
        internal abstract T _ { set; }

        /// <summary>
        /// Indicates whether the current object is equal to <typeparamref name="T"/> object.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the other parameter; otherwise, <see langword="false"/>.
        /// </returns>
        public Boolean Equals(T? other)
            => other == null && this._instance == null || (this._instance?.Equals(other) ?? false);

        /// <summary>
        /// Indicates whether the current object is equal to <see cref="IReferenceable{T}"/> object.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        /// <see langword="true"/> if the current object is equal to the other parameter; otherwise, <see langword="false"/>.
        /// </returns>
        public Boolean Equals(IReferenceable<T>? other)
            => other != default && ReferenceEquals(this.Reference, other.Reference);

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="instance">Initial value.</param>
        protected InputValue(in T instance) => this._instance = instance;

        /// <summary>
        /// Implicit operator. <see cref="InputValue{T}"/> -> <see cref="Nullable{T}"/>.
        /// </summary>
        /// <param name="valueInput"><see cref="InputValue{T}"/> object.</param>
        public static implicit operator T?(InputValue<T> valueInput)
            => valueInput != default ? valueInput._instance : default;

        /// <summary>
        /// Gets a <see cref="IMutableReference{TValue}"/> object which points to current instance.
        /// </summary>
        /// <returns><see cref="IMutableReference{TValue}"/> object.</returns>
        internal IMutableReference<T> GetMutableReference()
            => new ReferenceValue(this);

        /// <summary>
        /// Internal implementation of <see cref="IMutableReference{T}"/> interface.
        /// </summary>
        private struct ReferenceValue : IMutableReference<T>
        {
            /// <summary>
            /// Internal <see cref="InputValue{T}"/> object.
            /// </summary>
            private readonly InputValue<T> _inputValue;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="inputValue">Internal <see cref="InputValue{T}"/> object.</param>
            internal ReferenceValue([DisallowNull] InputValue<T> inputValue)
                => _inputValue = inputValue;

            ref readonly T IReferenceable<T>.Reference
                => ref this._inputValue.Reference;

            Boolean IEquatable<T>.Equals(T? other)
                => this._inputValue.Equals(other);

            Boolean IEquatable<IReferenceable<T>>.Equals(IReferenceable<T>? other)
                => this._inputValue.Equals(other);

            T IMutableReference<T>.SetInstance(T newValue)
                => this._inputValue._ = newValue;
        }
    }

    /// <summary>
    /// Supports a value type that can be referenced. This class cannot be inherited.
    /// </summary>
    public static class InputValue
    {
        /// <summary>
        /// Creates a new <see cref="InputValue{TValue}"/> object from a <typeparamref name="TValue"/> value.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of object.</typeparam>
        /// <param name="instance">Instance value.</param>
        /// <returns>
        /// <see cref="InputValue{TValue}"/> object which instance object is equal to 
        /// <paramref name="instance"/>.
        /// </returns>
        public static InputValue<TValue> Create<TValue>(in TValue instance) where TValue : struct
            => new ValueInput<TValue>(instance);

        /// <summary>
        /// Creates a new <see cref="InputValue{TValue}"/> object from a 
        /// <see cref="Nullable{TValue}"/> value.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of nullable object.</typeparam>
        /// <param name="instance">Instance nullable value.</param>
        /// <returns>
        /// <see cref="InputValue{TValue}"/> object which instance object is equal to 
        /// <paramref name="instance"/>.
        /// </returns>
        public static InputValue<TValue?> Create<TValue>(in TValue? instance) where TValue : struct
            => new NullableInput<TValue>(instance);

        /// <summary>
        /// Gets a <see cref="IMutableReference{TValue}"/> object which points to 
        /// the given <see cref="InputValue{TValue}"/> object.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of object.</typeparam>
        /// <param name="instance"><see cref="InputValue{TValue}"/> object.</param>
        /// <returns><see cref="IMutableReference{TValue}"/> object.</returns>
        public static IMutableReference<TValue>? GetReference<TValue>(this InputValue<TValue> instance)
            where TValue : struct
            => instance?.GetMutableReference();

        /// <summary>
        /// Gets a <see cref="IMutableReference{TValue}"/> object which points to 
        /// the given <see cref="InputValue{TValue}"/> object.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of nullable object.</typeparam>
        /// <param name="instance"><see cref="InputValue{TValue}"/> object.</param>
        /// <returns><see cref="IMutableReference{TValue}"/> object.</returns>
        public static IMutableReference<TValue?>? GetReference<TValue>(this InputValue<TValue?> instance)
            where TValue : struct
            => instance?.GetMutableReference();

        /// <summary>
        /// Retrieves the instance object from an <see cref="IReferenceable{T}"/> object.
        /// </summary>
        /// <typeparam name="T">Type of internal object.</typeparam>
        /// <param name="referenceable"><see cref="IReferenceable{T}"/> object.</param>
        /// <returns>Instance <see cref="Nullable{T}"/> object.</returns>
        public static T? GetInstance<T>(this IReferenceable<T> referenceable)
            => referenceable != default ? referenceable.Reference : default;

        /// <summary>
        /// Internal implementation of <see cref="InputValue{T}"/> for <see cref="ValueType"/> objects.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of the instance object.</typeparam>
        private sealed record ValueInput<TValue> : InputValue<TValue>
            where TValue : struct
        {
            internal override TValue _ { set => base._instance = value; }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="instance">Initial value.</param>
            internal ValueInput(in TValue instance) : base(instance) { }
        }

        /// <summary>
        /// Internal implementation of <see cref="InputValue{T}"/> for nullable <see cref="ValueType"/> objects.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of the instance object.</typeparam>
        private sealed record NullableInput<TValue> : InputValue<TValue?>
            where TValue : struct
        {
            internal override TValue? _ { set => base._instance = value; }

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="instance">Initial value.</param>
            internal NullableInput(in TValue? instance) : base(instance) { }
        }
    }
}
