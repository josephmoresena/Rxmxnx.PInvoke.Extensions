namespace Rxmxnx.PInvoke;

/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedListAction{T}"/> and .
/// </summary>
/// <remarks>
/// This interface provides an alternative to <see cref="ReadOnlyFixedListAction"/> or <see cref="FixedListAction{T}"/>
/// and their stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
public interface IFixedPointerListAction
{
	/// <summary>
	/// Performs an operation using the specified fixed pointer list.
	/// </summary>
	/// <param name="list">The <see cref="FixedPointerValueList"/> used by the operation.</param>
	void Accept(scoped FixedPointerValueList list);
}