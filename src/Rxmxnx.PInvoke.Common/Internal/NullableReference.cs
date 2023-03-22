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

    ref TValue? IReferenceable<TValue?>.Reference => ref base._instance;

    /// <inheritdoc/>
    internal override void SetInstance(in TValue? newValue)
    {
        lock (this._writeLock)
            base._instance = newValue;
    }

    void IMutableWrapper<TValue?>.SetInstance(TValue? newValue) => this.SetInstance(newValue);

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="instance">Initial value.</param>
    internal NullableReference(in TValue? instance) : base(instance) { }
}