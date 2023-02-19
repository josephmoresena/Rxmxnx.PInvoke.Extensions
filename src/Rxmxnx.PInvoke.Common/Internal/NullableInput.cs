namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of <see cref="ValueWrapper{T}"/> for nullable <see cref="ValueType"/> objects.
/// </summary>
/// <typeparam name="TValue"><see cref="ValueType"/> of the instance object.</typeparam>
internal record NullableInput<TValue> : ValueWrapper<TValue?> where TValue : struct
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="instance">Initial value.</param>
    internal NullableInput(in TValue? instance) : base(instance) { }
}

