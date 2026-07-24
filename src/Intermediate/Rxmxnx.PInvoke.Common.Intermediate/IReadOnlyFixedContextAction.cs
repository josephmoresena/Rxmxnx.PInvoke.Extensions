namespace Rxmxnx.PInvoke;

/// <summary>
/// Defines a callable object that performs an operation using a <see cref="ReadOnlyFixedContextValue{T}"/>.
/// </summary>
/// <typeparam name="T">Type of the fixed context.</typeparam>
/// <remarks>
/// This interface provides an alternative to <see cref="ReadOnlyFixedContextAction{T}"/> and its stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
public interface IReadOnlyFixedContextAction<T>
{
	/// <summary>
	/// Performs an operation using the specified read-only fixed context.
	/// </summary>
	/// <param name="fixedContext">The <see cref="ReadOnlyFixedContextValue{T}"/> used by the operation.</param>
	void Accept(scoped ReadOnlyFixedContextValue<T> fixedContext);
}