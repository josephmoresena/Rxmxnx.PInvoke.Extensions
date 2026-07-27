namespace Rxmxnx.PInvoke;

#if NETSTANDARD2_1 || NETCOREAPP
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedPointerValueList"/> and returns a result.
/// </summary>
/// <typeparam name="TResult">The type of the return value.</typeparam>
/// <remarks>
/// This interface provides an alternative to <see cref="ReadOnlyFixedListFunc{TResult}"/> or
/// <see cref="FixedListFunc{TResult}"/>
/// and their stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
#else
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedPointerValueList"/> and returns a result.
/// </summary>
/// <typeparam name="TResult">The type of the return value.</typeparam>
#endif
public interface IFixedPointerListFunction<out TResult>
{
	/// <summary>
	/// Performs an operation using the specified fixed pointer list and returns a result.
	/// </summary>
	/// <param name="list">The <see cref="FixedPointerValueList"/> used by the operation.</param>
	/// <returns>The result produced by the operation.</returns>
	TResult Apply(scoped FixedPointerValueList list);
}