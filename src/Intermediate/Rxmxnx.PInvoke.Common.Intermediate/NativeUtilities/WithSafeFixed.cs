#if !NET6_0_OR_GREATER
using ArgumentNullException = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArgumentNullExceptionCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#pragma warning disable CS8500
public static unsafe partial class NativeUtilities
{
	/// <summary>
	/// Prevents the garbage collector from relocating a given read-only reference and fixes its memory
	/// address until <paramref name="action"/> finishes.
	/// </summary>
	/// <typeparam name="T">A <see cref="ValueType"/> of value.</typeparam>
	/// <param name="value">A <typeparamref name="T"/> read-only reference.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedReferenceAction{T}"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T>(in T value, ReadOnlyFixedReferenceAction<T> action)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &value)
		{
			ReadOnlyFixedReference<T> ctx = new(ptr);
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
	/// <typeparam name="T">Type of referenced value.</typeparam>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
	/// <param name="action">A <see cref="FixedReferenceAction{T}"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T>(ref T value, FixedReferenceAction<T> action)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
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
	/// Prevents the garbage collector from relocating a given read-only reference and fixes its memory
	/// address until <paramref name="action"/> finishes.
	/// </summary>
	/// <typeparam name="T">Type of referenced value.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="value">A <typeparamref name="T"/> read-only reference.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A <see cref="ReadOnlyFixedReferenceAction{T, TArg}"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TArg>(in T value, TArg arg, ReadOnlyFixedReferenceAction<T, TArg> action)
#if NET9_0_OR_GREATER
		where T : allows ref struct where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		fixed (void* ptr = &value)
		{
			ReadOnlyFixedReference<T> ctx = new(ptr);
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
	/// address until <paramref name="action"/> finishes.
	/// </summary>
	/// <typeparam name="T">Type of referenced value.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A <see cref="FixedReferenceAction{T, TArg}"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<T, TArg>(ref T value, TArg arg, FixedReferenceAction<T, TArg> action)
#if NET9_0_OR_GREATER
		where T : allows ref struct where TArg : allows ref struct
#endif
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
	/// Prevents the garbage collector from relocating a given read-only reference and fixes its memory
	/// address until <paramref name="func"/> finishes.
	/// </summary>
	/// <typeparam name="T">Type of referenced value.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="value">A <typeparamref name="T"/> read-only reference.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedReferenceFunc{T, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TResult>(in T value, ReadOnlyFixedReferenceFunc<T, TResult> func)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &value)
		{
			ReadOnlyFixedReference<T> ctx = new(ptr);
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
	/// <typeparam name="T">Type of referenced value.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
	/// <param name="func">A <see cref="FixedReferenceFunc{T, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TResult>(ref T value, FixedReferenceFunc<T, TResult> func)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
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
	/// Prevents the garbage collector from relocating a given read-only reference and fixes its memory
	/// address until <paramref name="func"/> finishes.
	/// </summary>
	/// <typeparam name="T">Type of referenced value.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="value">A <typeparamref name="T"/> read-only reference.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="ReadOnlyFixedReferenceFunc{T, TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TArg, TResult>(in T value, TArg arg,
		ReadOnlyFixedReferenceFunc<T, TArg, TResult> func)
#if NET9_0_OR_GREATER
		where T : allows ref struct where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		fixed (void* ptr = &value)
		{
			ReadOnlyFixedReference<T> ctx = new(ptr);
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
	/// <summary>
	/// Prevents the garbage collector from relocating a given reference and fixes its memory
	/// address until <paramref name="func"/> finishes.
	/// </summary>
	/// <typeparam name="T">Type of referenced value.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="value">A <typeparamref name="T"/> reference.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="FixedReferenceFunc{T, TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<T, TArg, TResult>(ref T value, TArg arg,
		FixedReferenceFunc<T, TArg, TResult> func)
#if NET9_0_OR_GREATER
		where T : allows ref struct where TArg : allows ref struct
#endif
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
	/// <summary>
	/// Prevents the garbage collector from relocating a given method delegate and fixes its memory
	/// address until <paramref name="action"/> finishes.
	/// </summary>
	/// <typeparam name="TDelegate">A <see cref="Delegate"/> type method.</typeparam>
	/// <param name="del">A <typeparamref name="TDelegate"/> method.</param>
	/// <param name="action">A <see cref="FixedMethodAction{T}"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TDelegate>(TDelegate del, FixedMethodAction<TDelegate> action)
		where TDelegate : Delegate
	{
		ArgumentNullException.ThrowIfNull(action);
		FixedDelegate<TDelegate> fdel = new(del);
		try
		{
			action(fdel);
		}
		finally
		{
			fdel.Unload();
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating a given method delegate and fixes its memory
	/// address until <paramref name="action"/> finishes.
	/// </summary>
	/// <typeparam name="TDelegate">A <see cref="Delegate"/> type method.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <param name="del">A <typeparamref name="TDelegate"/> method.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="action">A <see cref="FixedMethodAction{T, TArg}"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TDelegate, TArg>(TDelegate del, TArg arg,
		FixedMethodAction<TDelegate, TArg> action) where TDelegate : Delegate
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(action);
		FixedDelegate<TDelegate> fdel = new(del);
		try
		{
			action(fdel, arg);
		}
		finally
		{
			fdel.Unload();
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating a given method delegate and fixes its memory
	/// address until <paramref name="func"/> finishes.
	/// </summary>
	/// <typeparam name="TDelegate">A <see cref="Delegate"/> type method.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="del">A <typeparamref name="TDelegate"/> method.</param>
	/// <param name="func">A <see cref="FixedMethodFunc{TDelegate, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<TDelegate, TResult>(TDelegate del, FixedMethodFunc<TDelegate, TResult> func)
		where TDelegate : Delegate
	{
		ArgumentNullException.ThrowIfNull(func);
		FixedDelegate<TDelegate> fdel = new(del);
		try
		{
			return func(fdel);
		}
		finally
		{
			fdel.Unload();
		}
	}
	/// <summary>
	/// Prevents the garbage collector from relocating a given method delegate and fixes its memory
	/// address until <paramref name="func"/> finishes.
	/// </summary>
	/// <typeparam name="TDelegate">A <see cref="Delegate"/> type method.</typeparam>
	/// <typeparam name="TArg">The type of the object that represents the state.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="del">A <typeparamref name="TDelegate"/> method.</param>
	/// <param name="arg">A state object of type <typeparamref name="TArg"/>.</param>
	/// <param name="func">A <see cref="FixedMethodFunc{TDelegate, TArg, TResult}"/> delegate.</param>
	/// <returns>The result of <paramref name="func"/> execution.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static TResult WithSafeFixed<TDelegate, TArg, TResult>(TDelegate del, TArg arg,
		FixedMethodFunc<TDelegate, TArg, TResult> func) where TDelegate : Delegate
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
		ArgumentNullException.ThrowIfNull(func);
		FixedDelegate<TDelegate> fdel = new(del);
		try
		{
			return func(fdel, arg);
		}
		finally
		{
			fdel.Unload();
		}
	}
}
#pragma warning restore CS8500