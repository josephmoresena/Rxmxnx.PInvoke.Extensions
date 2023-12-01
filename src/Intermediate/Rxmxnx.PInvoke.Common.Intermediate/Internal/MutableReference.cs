namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Provides an implementation of <see cref="MutableWrapper{T}"/> that can be referenced,
/// implementing <see cref="IMutableReference{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the encapsulated object.</typeparam>
internal sealed class MutableReference<T> : MutableWrapper<T>, IMutableReference<T>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MutableReference{T}"/> record with the
	/// specified initial value.
	/// </summary>
	/// <param name="instance">The initial value of the encapsulated object.</param>
	internal MutableReference(in T instance) : base(instance) { }
	ref T IMutableReference<T>.Reference => ref this.GetReference();
}