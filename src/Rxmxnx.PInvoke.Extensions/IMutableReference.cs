namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// This interface exposes an object which contains a reference to a mutable <typeparamref name="T"/> object.
    /// </summary>
    /// <typeparam name="T">Type of the referenced object.</typeparam>
    public interface IMutableReference<T> : IReferenceable<T>
    {
        /// <summary>
        /// Sets the instance object.
        /// </summary>
        /// <param name="newValue">New <typeparamref name="T"/> object to set as instance object.</param>
        T SetInstance(T newValue);
    }
}