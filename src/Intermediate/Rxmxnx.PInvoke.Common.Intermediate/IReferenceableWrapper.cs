namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a wrapper for an object that can be referenced and whose value
/// can be modified.
/// </summary>
public interface IReferenceableWrapper : IWrapper
{
    /// <summary>
    /// Creates a new instance of an object that implements <see cref="IReferenceableWrapper{TValue}"/> interface.
    /// </summary>
    /// <typeparam name="TValue">The <see cref="ValueType"/> of the object to be wrapped.</typeparam>
    /// <param name="value">The value to be wrapped.</param>
    /// <returns>An instance of an object that implements <see cref="IReferenceableWrapper{TValue}"/> interface.</returns>
    /// <remarks>
    /// The newly created object wraps a value of <typeparamref name="TValue"/> type provided by <paramref name="value"/>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new IReferenceableWrapper<TValue> Create<TValue>(in TValue value) where TValue : struct
        => IReferenceableWrapper<TValue>.Create(value);
    /// <summary>
    /// Creates a new instance of an object that implements <see cref="IReferenceableWrapper{TValue}"/> interface.
    /// </summary>
    /// <typeparam name="TValue">The <see cref="ValueType"/> of the nullable object to be wrapped.</typeparam>
    /// <param name="value">The nullable value to be wrapped.</param>
    /// <returns>An instance of an object that implements <see cref="IReferenceableWrapper{TValue}"/> interface.</returns>
    /// <remarks>
    /// The newly created object wraps a nullable value of <typeparamref name="TValue"/> type provided by
    /// <paramref name="value"/>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new IReferenceableWrapper<TValue?> CreateNullable<TValue>(in TValue? value) where TValue : struct
        => IReferenceableWrapper<TValue?>.Create(value);
    /// <summary>
    /// Creates a new instance of an object that implements <see cref="IReferenceableWrapper{TObject}"/> interface.
    /// </summary>
    /// <typeparam name="TObject">The type of the object to be wrapped.</typeparam>
    /// <param name="instance">The instance to be wrapped.</param>
    /// <returns>An instance of an object that implements <see cref="IReferenceableWrapper{TObject}"/> interface.</returns>
    /// <remarks>
    /// The newly created object wraps an object of <typeparamref name="TObject"/> type provided by <paramref name="instance"/>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new IReferenceableWrapper<TObject> CreateObject<TObject>(in TObject instance) where TObject : class
        => IReferenceableWrapper<TObject>.Create(instance);
}

/// <summary>
/// This interface exposes a wrapper for <typeparamref name="T"/> object that can be 
/// referenced and whose value can be modified. The provided reference is mutable, allowing changes to the value.
/// </summary>
/// <typeparam name="T">Type of both the wrapped and referenced value.</typeparam>
/// <remarks>While the value of the object can be accessed through this reference, it cannot be modified.</remarks>
public interface IReferenceableWrapper<T> : IReferenceableWrapper, IWrapper<T>, IReadOnlyReferenceable<T>
{
    /// <summary>
    /// Creates a new instance of an object that implements <see cref="IReferenceableWrapper{T}"/> interface.
    /// </summary>
    /// <param name="instance">The value to be wrapped.</param>
    /// <returns>An instance of an object that implements <see cref="IReferenceableWrapper{T}"/> interface.</returns>
    /// <remarks>
    /// The newly created object wraps a value of <typeparamref name="T"/> type provided by
    /// <paramref name="instance"/>.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new IReferenceableWrapper<T> Create(in T instance) => new InputReference<T>(instance);
}
