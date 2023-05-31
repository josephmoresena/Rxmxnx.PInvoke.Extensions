namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a wrapper for an object that can be referenced.
/// </summary>
public interface IReferenceableWrapper : IWrapper
{
    /// <summary>
    /// Creates a new <see cref="IReferenceableWrapper{TValue}"/> object from a 
    /// <typeparamref name="TValue"/> value.
    /// </summary>
    /// <typeparam name="TValue"><see cref="ValueType"/> of object.</typeparam>
    /// <param name="instance">Instance value.</param>
    /// <returns>
    /// <see cref="IReferenceableWrapper{TValue}"/> object which instance object is equal to 
    /// <paramref name="instance"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new IReferenceableWrapper<TValue> Create<TValue>(in TValue instance) where TValue : struct
        => IReferenceableWrapper<TValue>.Create(instance);

    /// <summary>
    /// Creates a new <see cref="IReferenceableWrapper{TValue}"/> object from a 
    /// <see cref="Nullable{TValue}"/> value.
    /// </summary>
    /// <typeparam name="TValue"><see cref="ValueType"/> of nullable object.</typeparam>
    /// <param name="instance">Instance nullable value.</param>
    /// <returns>
    /// <see cref="IReferenceableWrapper{TValue}"/> object which instance object is equal to 
    /// <paramref name="instance"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new IReferenceableWrapper<TValue?> CreateNullable<TValue>(in TValue? instance) where TValue : struct
        => IReferenceableWrapper<TValue?>.Create(instance);
}

/// <summary>
/// This interface exposes a wrapper for <typeparamref name="T"/> object that can be 
/// referenced.
/// </summary>
/// <typeparam name="T">Type of both wrapped and referenced value.</typeparam>
public interface IReferenceableWrapper<T> : IReferenceableWrapper, IWrapper<T>, IReadOnlyReferenceable<T>
{
    /// <summary>
    /// Creates a new <see cref="IReferenceableWrapper{T}"/> object from a 
    /// <typeparamref name="T"/> value.
    /// </summary>
    /// <param name="instance">Instance value.</param>
    /// <returns>
    /// <see cref="IReferenceableWrapper{T}"/> object which instance object is equal to 
    /// <paramref name="instance"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new IReferenceableWrapper<T> Create(in T instance) => new InputReference<T>(instance);
}
