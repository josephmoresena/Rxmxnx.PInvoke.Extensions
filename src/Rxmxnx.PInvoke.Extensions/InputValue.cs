using System;
using System.Diagnostics.CodeAnalysis;

using Rxmxnx.PInvoke.Extensions.Internal;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// Supports a value type that can be referenced. This class cannot be inherited.
    /// </summary>
    public static class InputValue
    {
        /// <summary>
        /// Creates a new <see cref="IReferenceableWrapper{TValue}"/> object from a 
        /// <typeparamref name="TValue"/> value.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of object.</typeparam>
        /// <param name="instance">Instance value.</param>
        /// <returns>
        /// <see cref="IReferenceableWrapper{TValue}"/> object which instance object is equal to 
        /// <paramref name="instance"/>.
        /// </returns>
        public static IReferenceableWrapper<TValue> CreateInput<TValue>(in TValue instance) where TValue : struct
            => new ValueInput<TValue>(instance);

        /// <summary>
        /// Creates a new <see cref="IReferenceableWrapper{TValue}"/> object from a 
        /// <see cref="Nullable{TValue}"/> value.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of nullable object.</typeparam>
        /// <param name="instance">Instance nullable value.</param>
        /// <returns>
        /// <see cref="IReferenceableWrapper{TValue}"/> object which instance object is equal to 
        /// <paramref name="instance"/>.
        /// </returns>
        public static IReferenceableWrapper<TValue?> CreateInput<TValue>(in TValue? instance) where TValue : struct
            => new NullableInput<TValue>(instance);

        /// <summary>
        /// Creates a new <see cref="IMutableReference{TValue}"/> object from a <typeparamref name="TValue"/> value.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of object.</typeparam>
        /// <param name="instance">Instance value.</param>
        /// <returns>
        /// <see cref="IMutableReference{TValue}"/> object which instance object is equal to 
        /// <paramref name="instance"/>.
        /// </returns>
        public static IMutableReference<TValue> CreateReference<TValue>(in TValue instance = default) where TValue : struct
            => new Reference<TValue>(instance);

        /// <summary>
        /// Creates a new <see cref="IMutableReference{TValue}"/> object from a 
        /// <see cref="Nullable{TValue}"/> value.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of nullable object.</typeparam>
        /// <param name="instance">Instance nullable value.</param>
        /// <returns>
        /// <see cref="IMutableReference{TValue}"/> object which instance object is equal to 
        /// <paramref name="instance"/>.
        /// </returns>
        public static IMutableReference<TValue?> CreateReference<TValue>(in TValue? instance = default) where TValue : struct
            => new NullableReference<TValue>(instance);

        /// <summary>
        /// Internal implementation of <see cref="InputValue{T}"/> for <see cref="ValueType"/> objects.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of the instance object.</typeparam>
        private record ValueInput<TValue> : InputValue<TValue>
            where TValue : struct
        {
            /// <summary>
            /// Internal method to set instance object.
            /// </summary>
            /// <param name="newValue">New <typeparamref name="TValue"/> object to set as instance object.</param>
            [ExcludeFromCodeCoverage]
            internal override void SetInstance(in TValue newValue) => throw new InvalidOperationException();

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
        private record NullableInput<TValue> : InputValue<TValue?> where TValue : struct
        {
            /// <summary>
            /// Internal method to set instance object.
            /// </summary>
            /// <param name="newValue">New <typeparamref name="TValue"/> object to set as instance object.</param>
            [ExcludeFromCodeCoverage]
            internal override void SetInstance(in TValue? newValue) => throw new InvalidOperationException();

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="instance">Initial value.</param>
            internal NullableInput(in TValue? instance) : base(instance) { }
        }

        /// <summary>
        /// Internal implementation of mutable <see cref="ValueInput{TValue}"/> object.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of the instance object.</typeparam>
        private sealed record Reference<TValue> : ValueInput<TValue>, IMutableReference<TValue>
            where TValue : struct
        {
            /// <summary>
            /// Internal lock object.
            /// </summary>
            private readonly Object _writeLock = new();

            /// <summary>
            /// Internal method to set instance object.
            /// </summary>
            /// <param name="newValue">New <typeparamref name="TValue"/> object to set as instance object.</param>
            internal override void SetInstance(in TValue newValue)
            {
                lock (this._writeLock)
                    base._instance = newValue;
            }

            void IMutableWrapper<TValue>.SetInstance(TValue newValue) => this.SetInstance(newValue);

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="instance">Initial value.</param>
            internal Reference(in TValue instance) : base(instance) { }
        }

        /// <summary>
        /// Internal implementation of nullable and mutable <see cref="ValueInput{TValue}"/> object.
        /// </summary>
        /// <typeparam name="TValue"><see cref="ValueType"/> of the instance object.</typeparam>
        private sealed record NullableReference<TValue> : NullableInput<TValue>, IMutableReference<TValue?>
            where TValue : struct
        {
            /// <summary>
            /// Internal lock object.
            /// </summary>
            private readonly Object _writeLock = new();

            /// <summary>
            /// Internal method to set instance object.
            /// </summary>
            /// <param name="newValue">New <typeparamref name="TValue"/> object to set as instance object.</param>
            internal override void SetInstance(in TValue? newValue)
            {
                lock (this._writeLock)
                    base._instance = newValue;
            }

            void IMutableWrapper<TValue?>.SetInstance(TValue? newValue) => this.SetInstance(newValue);

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="instance">Initial value.</param>
            internal NullableReference(in TValue? instance) : base(instance) { }
        }
    }
}
