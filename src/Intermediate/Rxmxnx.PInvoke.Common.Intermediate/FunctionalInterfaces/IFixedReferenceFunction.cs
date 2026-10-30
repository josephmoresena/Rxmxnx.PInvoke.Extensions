namespace Rxmxnx.PInvoke;

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedReferenceValue{T}"/> and returns
/// a result.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <remarks>
/// This interface provides an alternative to <see cref="FixedReferenceFunc{T, TResult}"/> and its stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
#else
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedReferenceValue{T}"/> and returns
/// a result.
/// </summary>
/// <typeparam name="T">Type of the fixed reference.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
#endif
public interface IFixedReferenceFunction<T, out TResult>
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
	/// <summary>
	/// Performs an operation using the specified fixed reference and returns a result.
	/// </summary>
	/// <param name="fixedReference">The <see cref="FixedReferenceValue{T}"/> used by the operation.</param>
	/// <returns>The result produced by the operation.</returns>
	TResult Apply(scoped FixedReferenceValue<T> fixedReference);
}