namespace Rxmxnx.PInvoke;

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

    /// <summary>
    /// Creates a new <see cref="IMutableWrapper{TValue}"/> object from a <typeparamref name="TValue"/> value.
    /// </summary>
    /// <typeparam name="TValue"><see cref="ValueType"/> of object.</typeparam>
    /// <param name="instance">Instance value.</param>
    /// <returns>
    /// <see cref="IMutableWrapper{TValue}"/> object which instance object is equal to 
    /// <paramref name="instance"/>.
    /// </returns>
    public static new IMutableWrapper<TValue> Create<TValue>(in TValue instance = default) where TValue : struct
        => new Reference<TValue>(instance);

    /// <summary>
    /// Creates a new <see cref="IMutableWrapper{TValue}"/> object from a 
    /// <see cref="Nullable{TValue}"/> value.
    /// </summary>
    /// <typeparam name="TValue"><see cref="ValueType"/> of nullable object.</typeparam>
    /// <param name="instance">Instance nullable value.</param>
    /// <returns>
    /// <see cref="IMutableWrapper{TValue}"/> object which instance object is equal to 
    /// <paramref name="instance"/>.
    /// </returns>
    public static new IMutableWrapper<TValue?> Create<TValue>(in TValue? instance = default) where TValue : struct
        => new NullableReference<TValue>(instance);
}
