namespace Rxmxnx.PInvoke;

/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedContextValue{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <remarks>
/// This interface provides an alternative to <see cref="FixedContextAction{T}"/> and its stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
public interface IFixedContextAction<T>
{
	/// <summary>
	/// Performs an operation using the specified fixed context.
	/// </summary>
	/// <param name="fixedContext">The <see cref="FixedContextValue{T}"/> used by the operation.</param>
	protected internal void Accept(scoped FixedContextValue<T> fixedContext);
}