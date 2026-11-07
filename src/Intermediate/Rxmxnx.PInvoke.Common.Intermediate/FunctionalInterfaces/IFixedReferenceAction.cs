namespace Rxmxnx.PInvoke;

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedReferenceValue{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <remarks>
/// This interface provides an alternative to <see cref="FixedReferenceAction{T}"/> and its stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
#else
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedReferenceValue{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
#endif
public interface IFixedReferenceAction<T>
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
	/// <summary>
	/// Performs an operation using the specified fixed reference.
	/// </summary>
	/// <param name="fixedReference">The <see cref="FixedReferenceValue{T}"/> used by the operation.</param>
	void Accept(scoped FixedReferenceValue<T> fixedReference);
}