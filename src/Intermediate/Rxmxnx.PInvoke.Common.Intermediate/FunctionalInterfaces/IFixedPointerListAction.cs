namespace Rxmxnx.PInvoke;

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedPointerValueList"/> and .
/// </summary>
/// <remarks>
/// This interface provides an alternative to <see cref="ReadOnlyFixedListAction"/> or <see cref="FixedListAction{T}"/>
/// and their stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
#else
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedPointerValueList"/> and .
/// </summary>
#endif
public interface IFixedPointerListAction
{
	/// <summary>
	/// Performs an operation using the specified fixed pointer list.
	/// </summary>
	/// <param name="list">The <see cref="FixedPointerValueList"/> used by the operation.</param>
	void Accept(scoped FixedPointerValueList list);
}