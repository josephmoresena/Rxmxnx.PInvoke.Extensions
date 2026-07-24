namespace Rxmxnx.PInvoke;

/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedPointerValue"/> and returns a result.
/// </summary>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <remarks>
/// This interface provides an alternative to <see cref="ReadOnlyFixedFunc{TResult}"/> or <see cref="FixedFunc{TResult}"/>
/// and their stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
public interface IFixedFunction<out TResult>
{
	/// <summary>
	/// Performs an operation using the specified fixed pointer and returns a result.
	/// </summary>
	/// <param name="fixedPointer">The <see cref="FixedPointerValue"/> used by the operation.</param>
	/// <returns>The result produced by the operation.</returns>
	TResult Apply(scoped FixedPointerValue fixedPointer);
}