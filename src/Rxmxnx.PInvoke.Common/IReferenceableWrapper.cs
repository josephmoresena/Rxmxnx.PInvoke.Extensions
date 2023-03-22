namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a wrapper for <typeparamref name="T"/> object that can be 
/// referenced.
/// </summary>
/// <typeparam name="T">Type of both wrapped and referenced value.</typeparam>
public interface IReferenceableWrapper<T> : IWrapper<T>, IReadOnlyReferenceable<T>
{
    /// <summary>
    /// Creates a new <see cref="IWrapper{TValue}"/> object from a 
    /// <typeparamref name="TValue"/> value.
    /// </summary>
    /// <typeparam name="TValue"><see cref="ValueType"/> of object.</typeparam>
    /// <param name="instance">Instance value.</param>
    /// <returns>
    /// <see cref="IWrapper{TValue}"/> object which instance object is equal to 
    /// <paramref name="instance"/>.
    /// </returns>
    public static new IReferenceableWrapper<TValue> Create<TValue>(in TValue instance) where TValue : struct
        => new Input<TValue>(instance);

    /// <summary>
    /// Creates a new <see cref="IWrapper{TValue}"/> object from a 
    /// <see cref="Nullable{TValue}"/> value.
    /// </summary>
    /// <typeparam name="TValue"><see cref="ValueType"/> of nullable object.</typeparam>
    /// <param name="instance">Instance nullable value.</param>
    /// <returns>
    /// <see cref="IWrapper{TValue}"/> object which instance object is equal to 
    /// <paramref name="instance"/>.
    /// </returns>
    public static new IReferenceableWrapper<TValue?> CreateNullable<TValue>(in TValue? instance) where TValue : struct
        => new NullableInput<TValue>(instance);
}
