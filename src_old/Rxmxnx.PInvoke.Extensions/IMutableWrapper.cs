namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// This interface exposes a wrapper for <typeparamref name="T"/> object whose value 
    /// can be modified.
    /// </summary>
    /// <typeparam name="T">Type of wrapped value.</typeparam>
    public interface IMutableWrapper<T> : IWrapper<T>
    {
        /// <summary>
        /// Sets the instance object.
        /// </summary>
        /// <param name="newValue">New <typeparamref name="T"/> object to set as instance object.</param>
        void SetInstance(T? newValue);
    }
}
