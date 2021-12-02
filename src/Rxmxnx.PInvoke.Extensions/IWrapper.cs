using System;

namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// This interface exposes an object which represents a wrapper of <typeparamref name="T"/> object.
    /// </summary>
    /// <typeparam name="T">Type of wrapped value.</typeparam>
    public interface IWrapper<T> : IEquatable<T>
    {
        /// <summary>
        /// Wrapped <typeparamref name="T"/> object.
        /// </summary>
        public T Value { get; }
    }
}
