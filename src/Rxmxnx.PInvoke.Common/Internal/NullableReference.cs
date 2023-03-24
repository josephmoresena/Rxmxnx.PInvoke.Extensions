namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of nullable and mutable <see cref="Input{TValue}"/> object.
/// </summary>
/// <typeparam name="TValue"><see cref="ValueType"/> of the instance object.</typeparam>
internal sealed record NullableReference<TValue> : NullableInput<TValue>, IMutableReference<TValue?>
    where TValue : struct
{
    /// <summary>
    /// Internal lock object.
    /// </summary>
    private readonly Object _writeLock = new();

    TValue? IMutableWrapper<TValue?>.Value { get => base.Value; set => SetInstance(this, this._writeLock, value); }
    ref TValue? IReferenceable<TValue?>.Reference => ref Unsafe.AsRef(base.Reference);

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="instance">Initial value.</param>
    internal NullableReference(in TValue? instance) : base(instance) { }
}