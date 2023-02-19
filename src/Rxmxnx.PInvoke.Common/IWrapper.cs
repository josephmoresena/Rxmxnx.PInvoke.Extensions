namespace Rxmxnx.PInvoke;

/// <summary>
/// This interface exposes a wrapper for <typeparamref name="T"/> object.
/// </summary>
/// <typeparam name="T">Type of wrapped value.</typeparam>
public interface IWrapper<T> : IEquatable<T>
{
    /// <summary>
    /// Wrapped <typeparamref name="T"/> object.
    /// </summary>
    public T? Value { get; }

    Boolean IEquatable<T>.Equals(T? other) => Object.Equals(this.Value, other);

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
    public static IWrapper<TValue> Create<TValue>(in TValue instance) where TValue : struct
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
    public static IWrapper<TValue?> CreateNullable<TValue>(in TValue? instance) where TValue : struct
        => new NullableInput<TValue>(instance);
}
