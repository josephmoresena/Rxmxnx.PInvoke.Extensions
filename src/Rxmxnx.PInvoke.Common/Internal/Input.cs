namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of <see cref="ValueWrapper{T}"/> for <see cref="ValueType"/> objects.
/// </summary>
/// <typeparam name="TValue"><see cref="ValueType"/> of the instance object.</typeparam>
internal record Input<TValue> : ValueWrapper<TValue>
    where TValue : struct
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="instance">Initial value.</param>
    internal Input(in TValue instance) : base(instance) { }
}