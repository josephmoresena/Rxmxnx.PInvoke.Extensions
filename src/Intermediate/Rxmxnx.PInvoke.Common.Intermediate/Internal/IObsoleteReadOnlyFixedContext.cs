#if (NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER) && OBSOLETE_FIXED_INTERFACES
namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Obsolete-safe <see cref="IFixedContext{T}"/>
/// </summary>
/// <typeparam name="T">Type of objects in the fixed memory block.</typeparam>
[Obsolete]
// ReSharper disable once PossibleInterfaceMemberAmbiguity
internal interface IObsoleteReadOnlyFixedContext<T> : IReadOnlyFixedContext<T>
{
	/// <summary>
	/// Obsolete-safe <see cref="IReadOnlyFixedContext{T}.IDisposable"/>
	/// </summary>
	// ReSharper disable once PossibleInterfaceMemberAmbiguity
	public new interface IDisposable : IReadOnlyFixedContext<T>.IDisposable;
}
#endif