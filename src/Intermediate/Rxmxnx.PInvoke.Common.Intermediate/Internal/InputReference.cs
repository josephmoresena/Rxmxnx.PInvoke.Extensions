namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Provides an implementation of <see cref="Input{T}"/> that can be referenced read-only,
/// implementing <see cref="IReferenceableWrapper{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the referenced object.</typeparam>
internal sealed record InputReference<T> : Input<T>, IReferenceableWrapper<T>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="InputReference{T}"/> record with
	/// the specified initial value.
	/// </summary>
	/// <param name="instance">The initial value of the encapsulated object.</param>
	internal InputReference(in T instance) : base(instance) { }
	ref readonly T IReadOnlyReferenceable<T>.Reference => ref this.GetReference();
}