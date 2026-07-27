namespace Rxmxnx.PInvoke;

#if NETSTANDARD2_1 || NETCOREAPP
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedPointerValue"/>.
/// </summary>
/// <remarks>
/// This interface provides an alternative to <see cref="ReadOnlyFixedAction"/> or <see cref="FixedAction"/> and their
/// stateful variant.
/// Implementations can store the operation state directly.
/// </remarks>
#else
/// <summary>
/// Defines a callable object that performs an operation using a <see cref="FixedPointerValue"/>.
/// </summary>
#endif
public interface IFixedAction
{
	/// <summary>
	/// Performs an operation using the specified fixed pointer.
	/// </summary>
	/// <param name="fixedPointer">The <see cref="FixedPointerValue"/> used by the operation.</param>
	void Accept(scoped FixedPointerValue fixedPointer);
}