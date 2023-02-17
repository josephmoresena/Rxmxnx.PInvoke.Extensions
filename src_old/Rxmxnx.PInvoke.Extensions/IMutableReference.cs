namespace Rxmxnx.PInvoke.Extensions
{
    /// <summary>
    /// This interface exposes a wrapper for <typeparamref name="T"/> object that can be 
    /// referenced and whose value can be modified.
    /// </summary>
    /// <typeparam name="T">Type of both wrapped and referenced value.</typeparam>
    public interface IMutableReference<T> : IReferenceableWrapper<T>, IMutableWrapper<T>
    {
    }
}