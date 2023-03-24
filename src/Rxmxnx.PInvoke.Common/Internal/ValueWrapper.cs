namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Creates an object which contains a single reference to an inmutable <typeparamref name="T"/> object.
/// </summary>
/// <typeparam name="T">Type of the referenced object.</typeparam>
internal abstract record ValueWrapper<T> : IReferenceableWrapper<T>
{
    /// <summary>
    /// Internal <typeparamref name="T"/> object.
    /// </summary>
    private T _instance;

    /// <summary>
    /// Reference to instance <typeparamref name="T"/> object.
    /// </summary>
    public ref readonly T Reference => ref this._instance;

    /// <summary>
    /// Wrapped <typeparamref name="T"/> object.
    /// </summary>
    public T Value => this._instance;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="instance">Initial value.</param>
    protected ValueWrapper(in T instance) => this._instance = instance;

    /// <summary>
    /// Internal method to set instance object.
    /// </summary>
    /// <param name="wrapper">A <see cref="ValueWrapper{T}"/> instance.</param>
    /// <param name="writeLock">Wrapper's write lock.</param>
    /// <param name="newValue">New <typeparamref name="T"/> object to set as instance object.</param>
    protected static void SetInstance(ValueWrapper<T> wrapper, Object writeLock, in T newValue)
    {
        lock (writeLock)
            wrapper._instance = newValue;
    }
}