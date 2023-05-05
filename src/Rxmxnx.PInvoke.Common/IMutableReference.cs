namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a wrapper for an object that can be referenced and whose value can be modified.
/// </summary>
public interface IMutableReference : IMutableWrapper
{
    /// <summary>
    /// Creates a new <see cref="IMutableReference{TValue}"/> object from a <typeparamref name="TValue"/> value.
    /// </summary>
    /// <typeparam name="TValue"><see cref="ValueType"/> of object.</typeparam>
    /// <param name="instance">Instance value.</param>
    /// <returns>
    /// <see cref="IMutableReference{TValue}"/> object which instance object is equal to 
    /// <paramref name="instance"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new IMutableReference<TValue> Create<TValue>(in TValue instance = default!) where TValue : struct
        => IMutableReference<TValue>.Create(instance);

    /// <summary>
    /// Creates a new <see cref="IMutableReference{TValue}"/> object from a 
    /// <see cref="Nullable{TValue}"/> value.
    /// </summary>
    /// <typeparam name="TValue"><see cref="ValueType"/> of nullable object.</typeparam>
    /// <param name="instance">Instance nullable value.</param>
    /// <returns>
    /// <see cref="IMutableReference{TValue}"/> object which instance object is equal to 
    /// <paramref name="instance"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new IMutableReference<TValue?> CreateNullable<TValue>(in TValue? instance = default) where TValue : struct
        => IMutableReference<TValue?>.Create(instance);
}

/// <summary>
/// This interface exposes a wrapper for <typeparamref name="T"/> object that can be 
/// referenced and whose value can be modified.
/// </summary>
/// <typeparam name="T">Type of both wrapped and referenced value.</typeparam>
public interface IMutableReference<T> : IReferenceableWrapper<T>, IMutableWrapper<T>, IReferenceable<T>
{
    ref readonly T IReadOnlyReferenceable<T>.Reference => ref (this as IReferenceable<T>).Reference;

    /// <summary>
    /// Creates a new <see cref="IMutableReference{T}"/> object from a <typeparamref name="T"/> value.
    /// </summary>
    /// <param name="instance">Instance value.</param>
    /// <returns>
    /// <see cref="IMutableReference{T}"/> object which instance object is equal to 
    /// <paramref name="instance"/>.
    /// </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new IMutableReference<T> Create(in T instance = default!) => new MutableReference<T>(instance);
}