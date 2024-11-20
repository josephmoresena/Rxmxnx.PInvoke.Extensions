namespace Rxmxnx.PInvoke;

[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#pragma warning disable CS8500
public static unsafe partial class NativeUtilities
{
	/// <summary>
	/// Prevents the garbage collector from relocating a given reference and fixes its memory
	/// address until <paramref name="action"/> finishes.
	/// </summary>
	/// <typeparam name="T">Type of referenced value</typeparam>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedReferenceAction{T}"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<T>(ref T value, ReadOnlyFixedReferenceAction<T> action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &value)
		{
			FixedReference<T> ctx = new(ptr);
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
	/// Prevents the garbage collector from relocating a given reference and fixes its memory
	/// address until <paramref name="action"/> finishes.
	/// </summary>
	/// <typeparam name="T">Type of referenced value</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedReferenceAction{T, TArg}"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeReadOnlyFixed<T, TArg>(ref T value, TArg arg,
		ReadOnlyFixedReferenceAction<T, TArg> action)
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &value)
		{
			FixedReference<T> ctx = new(ptr);
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
	/// Prevents the garbage collector from relocating a given reference and fixes its memory
	/// address until <paramref name="func"/> finishes.
	/// </summary>
	/// <typeparam name="T">Type of referenced value</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedReferenceFunc{T, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeReadOnlyFixed<T, TResult>(ref T value, ReadOnlyFixedReferenceFunc<T, TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &value)
		{
			FixedReference<T> ctx = new(ptr);
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
	/// Prevents the garbage collector from relocating a given reference and fixes its memory
	/// address until <paramref name="func"/> finishes.
	/// </summary>
	/// <typeparam name="T">Type of referenced value</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedReferenceFunc{T, TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeReadOnlyFixed<T, TArg, TResult>(ref T value, TArg arg,
		ReadOnlyFixedReferenceFunc<T, TArg, TResult> func)
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &value)
		{
			FixedReference<T> ctx = new(ptr);
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
#pragma warning restore CS8500