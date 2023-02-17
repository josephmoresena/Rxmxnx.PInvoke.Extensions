using System;
using System.Runtime.CompilerServices;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// This interface exposes a read-only reference to <typeparamref name="T"/> object.
    /// </summary>
    /// <typeparam name="T">Type of the referenced object.</typeparam>
    public interface IReferenceable<T> : IEquatable<IReferenceable<T>>
    {
        /// <summary>
        /// Reference to instance <typeparamref name="T"/> object.
        /// </summary>
        ref readonly T? Reference { get; }

        Boolean IEquatable<IReferenceable<T>>.Equals(IReferenceable<T>? other)
            => other is not null && Unsafe.AreSame(ref Unsafe.AsRef(this.Reference), ref Unsafe.AsRef(other.Reference));
    }
}
