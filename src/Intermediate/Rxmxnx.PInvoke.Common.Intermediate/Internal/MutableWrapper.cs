namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Provides a mutable implementation of <see cref="Input{T}"/> that can be both retrieved and updated,
/// implementing <see cref="IMutableWrapper{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the encapsulated object.</typeparam>
internal record MutableWrapper<T> : Input<T>, IMutableWrapper<T>
{
    /// <summary>
    /// Lock object used for synchronization of write operations.
    /// </summary>
    private readonly Object _writeLock = new();

    T IMutableWrapper<T>.Value { get => base.GetInstance(); set => base.SetInstance(this._writeLock, value); }

    /// <summary>
    /// Initializes a new instance of the <see cref="MutableWrapper{T}"/> record with the specified
    /// initial value.
    /// </summary>
    /// <param name="instance">The initial value of the encapsulated object.</param>
    internal MutableWrapper(in T instance) : base(instance) { }
}
