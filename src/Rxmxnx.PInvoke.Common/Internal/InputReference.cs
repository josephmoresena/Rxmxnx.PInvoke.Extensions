namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of <see cref="Input{T}"/> as <see cref="IReferenceableWrapper{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the referenced object.</typeparam>
internal record InputReference<T> : Input<T>, IReferenceableWrapper<T>
{
    ref readonly T IReadOnlyReferenceable<T>.Reference => ref base.GetReference();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="instance">Initial value.</param>
    internal InputReference(in T instance) : base(instance) { }
}