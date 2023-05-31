namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of <see cref="Input{T}"/> as <see cref="IMutableWrapper{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the referenced object.</typeparam>
internal record MutableWrapper<T> : Input<T>, IMutableWrapper<T>
{
    /// <summary>
    /// Internal lock object.
    /// </summary>
    private readonly Object _writeLock = new();

    T IMutableWrapper<T>.Value { get => base.GetInstance(); set => base.SetInstance(this._writeLock, value); }

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="instance">Initial value.</param>
    internal MutableWrapper(in T instance) : base(instance) { }
}
