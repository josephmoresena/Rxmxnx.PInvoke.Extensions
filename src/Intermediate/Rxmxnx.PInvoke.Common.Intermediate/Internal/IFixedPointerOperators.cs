#if NET9_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Defines a mechanism for performing <see cref="FixedPointerValue"/> casting.
/// </summary>
/// <typeparam name="TSelf">The type that implements this interface</typeparam>
internal interface IFixedPointerOperators<in TSelf> : IFixedPointer
	where TSelf : struct, IFixedPointerOperators<TSelf>, allows ref struct
{
	/// <summary>
	/// Defines an implicit conversion of a <typeparamref name="TSelf"/> to a <see cref="FixedPointerValue"/>
	/// instance.
	/// </summary>
	/// <param name="value">A pointer to implicitly convert.</param>
	static abstract implicit operator FixedPointerValue(TSelf value);
}
#endif