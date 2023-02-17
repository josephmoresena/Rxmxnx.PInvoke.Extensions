namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// This interface exposes a wrapper for <typeparamref name="T"/> object that can be 
    /// referenced.
    /// </summary>
    /// <typeparam name="T">Type of both wrapped and referenced value.</typeparam>
    public interface IReferenceableWrapper<T> : IWrapper<T>, IReferenceable<T>
    {
    }
}
