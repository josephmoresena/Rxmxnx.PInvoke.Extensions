namespace Rxmxnx.PInvoke;

/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedContextValue{T}"/> and returns
/// a result.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <remarks>
/// This interface provides an alternative to <see cref="FixedContextFunc{T, TResult}"/> and its stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
public interface IFixedContextFunction<T, out TResult>
{
	/// <summary>
	/// Performs an operation using the specified fixed context and returns a result.
	/// </summary>
	/// <param name="fixedContext">The <see cref="FixedContextValue{T}"/> used by the operation.</param>
	/// <returns>The result produced by the operation.</returns>
	protected internal TResult Apply(scoped FixedContextValue<T> fixedContext);
}