namespace Rxmxnx.PInvoke;

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="ReadOnlyFixedReferenceValue{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <remarks>
/// This interface provides an alternative to <see cref="ReadOnlyFixedReferenceAction{T}"/> and its stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
#else
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="ReadOnlyFixedReferenceValue{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
#endif
public interface IReadOnlyFixedReferenceAction<T>
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
	/// <summary>
	/// Performs an operation using the specified read-only fixed reference.
	/// </summary>
	/// <param name="fixedReference">The <see cref="ReadOnlyFixedReferenceValue{T}"/> used by the operation.</param>
	void Accept(scoped ReadOnlyFixedReferenceValue<T> fixedReference);
}