namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Creates an object which contains a single reference to an inmutable <typeparamref name="T"/> object.
/// </summary>
/// <typeparam name="T">Type of the referenced object.</typeparam>
internal record Input<T> : IWrapper<T>
{
    /// <summary>
    /// Internal <typeparamref name="T"/> object.
    /// </summary>
    private T _instance;

    T IWrapper<T>.Value => this._instance;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="instance">Initial value.</param>
    public Input(in T instance) => this._instance = instance;

    /// <summary>
    /// Retrieves the value to <typeparamref name="T"/> object.
    /// </summary>
    /// <returns>The value to <typeparamref name="T"/> object.</returns>
    protected T GetInstance() => this._instance;

    /// <summary>
    /// Retrieves the reference to <typeparamref name="T"/> object.
    /// </summary>
    /// <returns>The reference to <typeparamref name="T"/> object.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected ref T GetReference() => ref this._instance;

    /// <summary>
    /// Internal method to set instance object.
    /// </summary>
    /// <param name="writeLock">Wrapper's write lock.</param>
    /// <param name="newValue">New <typeparamref name="T"/> object to set as instance object.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected void SetInstance(Object writeLock, in T newValue)
    {
        lock (writeLock)
            this._instance = newValue;
    }
}