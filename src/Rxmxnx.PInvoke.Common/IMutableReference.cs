namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a wrapper for <typeparamref name="T"/> object that can be 
/// referenced and whose value can be modified.
/// </summary>
/// <typeparam name="T">Type of both wrapped and referenced value.</typeparam>
public interface IMutableReference<T> : IReferenceableWrapper<T>, IMutableWrapper<T>
{
    /// <summary>
    /// Creates a new <see cref="IMutableWrapper{TValue}"/> object from a <typeparamref name="TValue"/> value.
    /// </summary>
    /// <typeparam name="TValue"><see cref="ValueType"/> of object.</typeparam>
    /// <param name="instance">Instance value.</param>
    /// <returns>
    /// <see cref="IMutableWrapper{TValue}"/> object which instance object is equal to 
    /// <paramref name="instance"/>.
    /// </returns>
    public static new IMutableReference<TValue> Create<TValue>(in TValue instance = default) where TValue : struct
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
    public static new IMutableReference<TValue?> Create<TValue>(in TValue? instance = default) where TValue : struct
        => new NullableReference<TValue>(instance);
}