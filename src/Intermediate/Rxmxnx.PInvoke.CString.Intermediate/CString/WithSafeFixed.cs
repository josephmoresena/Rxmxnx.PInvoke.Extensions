#if !NET6_0_OR_GREATER
using ArgumentNullException = Rxmxnx.PInvoke.Internal.ArgumentNullUtilities;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class CString
{
	/// <summary>
	/// Prevents the garbage collector from relocating the current instance and fixes its memory
	/// address until <paramref name="action"/> is finished.
	/// </summary>
	/// <param name="action">A <see cref="ReadOnlyFixedAction"/> delegate.</param>
	/// <remarks>
	/// The action operates on a read-only fixed memory instance.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void WithSafeFixed(ReadOnlyFixedAction action)
	{
		ArgumentNullException.ThrowIfNull(action);
		ReadOnlySpan<Byte> span = this._data;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			ReadOnlyFixedContext<Byte> ctx = new(ptr, this.Length);
			try
			{
				action(ctx);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current instance and fixes its memory
	/// address until <paramref name="action"/> is finished.
	/// </summary>
	/// <typeparam name="TArg">The type of the state object used by the action.</typeparam>
	/// <param name="arg">The state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedAction{TArg}"/> delegate.</param>
	/// <remarks>
	/// The action operates on a read-only fixed memory instance using an additional state object.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void WithSafeFixed<TArg>(TArg arg, ReadOnlyFixedAction<TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		ReadOnlySpan<Byte> span = this._data;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			ReadOnlyFixedContext<Byte> ctx = new(ptr, this.Length);
			try
			{
				action(ctx, arg);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current instance and fixes its memory
	/// address until <paramref name="func"/> is finished.
	/// </summary>
	/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
	/// <param name="func">A <see cref="ReadOnlyFixedFunc{TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	/// <remarks>
	/// The function operates on a read-only fixed memory instance.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public TResult WithSafeFixed<TResult>(ReadOnlyFixedFunc<TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		ReadOnlySpan<Byte> span = this._data;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			ReadOnlyFixedContext<Byte> ctx = new(ptr, this.Length);
			try
			{
				return func(ctx);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current instance and fixes its memory
	/// address until <paramref name="func"/> is finished.
	/// </summary>
	/// <typeparam name="TArg">The type of the state object used by the function.</typeparam>
	/// <typeparam name="TResult">The type of the return value of the function.</typeparam>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedFunc{TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	/// <remarks>
	/// The function operates on a read-only fixed memory instance using an additional state object.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public TResult WithSafeFixed<TArg, TResult>(TArg arg, ReadOnlyFixedFunc<TArg, TResult> func)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		ReadOnlySpan<Byte> span = this._data;
		fixed (void* ptr = &MemoryMarshal.GetReference(span))
		{
			ReadOnlyFixedContext<Byte> ctx = new(ptr, this.Length);
			try
			{
				return func(ctx, arg);
			}
			finally
			{
				ctx.Unload();
			}
		}
	}
}