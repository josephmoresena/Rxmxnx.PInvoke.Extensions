namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Internal implementation of <see cref="Input{T}"/> as <see cref="IMutableReference{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the referenced object.</typeparam>
internal record MutableReference<T> : MutableWrapper<T>, IMutableReference<T>
{
    ref T IReferenceable<T>.Reference => ref this.GetReference();

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="instance">Initial value.</param>
    internal MutableReference(in T instance) : base(instance) { }
}
