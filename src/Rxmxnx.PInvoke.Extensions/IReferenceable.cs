using System;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// This interface exposes an object which contains a reference to an inmutable <typeparamref name="T"/> object.
    /// </summary>
    /// <typeparam name="T">Type of the referenced object.</typeparam>
    public interface IReferenceable<T> : IEquatable<T>, IEquatable<IReferenceable<T>>
    {
        /// <summary>
        /// Reference to instance <typeparamref name="T"/> object.
        /// </summary>
        ref readonly T Reference { get; }
    }
}
