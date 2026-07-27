#if NETSTANDARD2_1 || (NETCOREAPP && OBSOLETE_FIXED_INTERFACES)
namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Obsolete-safe <see cref="IFixedContext{T}"/>
/// </summary>
/// <typeparam name="T">Type of objects in the fixed memory block.</typeparam>
[Obsolete]
// ReSharper disable once PossibleInterfaceMemberAmbiguity
internal interface IObsoleteFixedContext<T> : IFixedContext<T>
{
	/// <summary>
	/// Obsolete-safe <see cref="IFixedContext{T}.IDisposable"/>
	/// </summary>
	// ReSharper disable once PossibleInterfaceMemberAmbiguity
	public new interface IDisposable : IFixedContext<T>.IDisposable;
}
#endif