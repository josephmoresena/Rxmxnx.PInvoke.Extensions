namespace Rxmxnx.PInvoke;

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="ReadOnlyFixedContextValue{T}"/> and returns
/// a result.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <remarks>
/// This interface provides an alternative to <see cref="ReadOnlyFixedContextFunc{T, TResult}"/> and its stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
#else
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="ReadOnlyFixedContextValue{T}"/> and returns
/// a result.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
#endif
public interface IReadOnlyFixedContextFunction<T, out TResult>
{
	/// <summary>
	/// Performs an operation using the specified read-only fixed context and returns a result.
	/// </summary>
	/// <param name="fixedContext">The <see cref="ReadOnlyFixedContextValue{T}"/> used by the operation.</param>
	/// <returns>The result produced by the operation.</returns>
	TResult Apply(scoped ReadOnlyFixedContextValue<T> fixedContext);
}