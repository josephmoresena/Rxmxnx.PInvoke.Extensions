namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a wrapper for an object whose value can be modified.
/// </summary>
public interface IMutableWrapper : IWrapper
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static new IMutableWrapper<TValue?> CreateNullable<TValue>(in TValue? instance = default) where TValue : struct
        => new NullableReference<TValue>(instance);
}

/// <summary>
/// This interface exposes a wrapper for <typeparamref name="T"/> object whose value 
/// can be modified.
/// </summary>
/// <typeparam name="T">Type of wrapped value.</typeparam>
public interface IMutableWrapper<T> : IMutableWrapper, IWrapper<T>
{
    /// <summary>
    /// Wrapped <typeparamref name="T"/> object.
    /// </summary>
    new T? Value { get; set; }

    [ExcludeFromCodeCoverage]
    T? IWrapper<T>.Value => this.Value;
}
