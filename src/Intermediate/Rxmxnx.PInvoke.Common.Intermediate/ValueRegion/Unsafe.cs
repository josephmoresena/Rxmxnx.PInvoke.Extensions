#if NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public abstract unsafe partial class ValueRegion<T>
{
	/// <summary>
	/// Creates a new <see cref="ValueRegion{T}"/> instance from a read-only span function pointer.
	/// function.
	/// </summary>
	/// <param name="func">A read-only span function pointer.</param>
	/// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	public static ValueRegion<T> Create(delegate*<ReadOnlySpan<T>> func) => new UnsafeFuncRegion(func);
	/// <summary>
	/// Creates a new <see cref="ValueRegion{T}"/> instance from a stateful read-only funcion pointer and a
	/// <typeparamref name="TState"/> instance.
	/// </summary>
	/// <param name="state">Function state.</param>
	/// <param name="func">A stateful read-only span function pointer.</param>
	/// <param name="tryAlloc">Allocation state function pointer.</param>
	/// <returns>A new <see cref="ValueRegion{T}"/> instance.</returns>
	public static ValueRegion<T> Create<TState>(TState state, delegate*<TState, ReadOnlySpan<T>> func,
		delegate*<TState, GCHandleType, out GCHandle, Boolean> tryAlloc = default)
		=> new UnsafeFuncRegion<TState>(state, func, tryAlloc);
}
#endif